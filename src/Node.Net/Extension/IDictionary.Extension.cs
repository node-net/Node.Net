﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Node.Net
{
	/// <summary>
	/// Extension methods for System.Collections.IDictionary
	/// </summary>
	public static class IDictionaryExtension
	{
		/// <summary>
		/// Get the JSON string for an IDictionary
		/// </summary>
		/// <param name="dictionary"></param>
		/// <returns></returns>
		public static string ToJson(this IDictionary dictionary)
		{
			return new Internal.JSONWriter().WriteToString(dictionary);
		}

		/// <summary>
		/// Save to a stream
		/// </summary>
		/// <param name="dictionary"></param>
		/// <param name="stream"></param>
		public static void Save(this IDictionary dictionary, Stream stream)
		{
			new Internal.JSONWriter().Write(stream, dictionary);
		}

		/// <summary>
		/// Deep Update Parent references
		/// </summary>
		/// <param name="dictionary"></param>
		public static void DeepUpdateParents(this IDictionary dictionary)
		{
			foreach (var value in dictionary.Values)
			{
				var child = value as IDictionary;
				if (child != null)
				{
					child.SetParent(dictionary);
					DeepUpdateParents(child);
				}
			}
		}

		public static void DeepClean(this IDictionary dictionary)
		{
			foreach (var value in dictionary.Values)
			{
				var child = value as IDictionary;
				if (child != null)
				{
					child.DeepClean();
					child = null;
				}
			}
			dictionary.Clear();
			dictionary.ClearMetaData();
			ObjectExtension.CleanMetaData();
		}

		/// <summary>
		/// Get the Length in meters from a value in a dictionary
		/// </summary>
		/// <param name="dictionary"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static double GetLengthMeters(this IDictionary dictionary, string name)
		{
			var svalue = dictionary.Get<string>(name);
			return Internal.Length.GetMeters(svalue);
		}

		/// <summary>
		/// Get the angle in degrees from an IDictionary value
		/// </summary>
		/// <param name="dictionary"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static double GetAngleDegrees(this IDictionary dictionary, string name)
		{
			var svalue = dictionary.Get<string>(name);
			return Internal.Angle.GetDegrees(svalue);
		}

		private static bool UseValueHash(object value)
		{
			if (value is bool ||
				value is double ||
				value is float ||
				value is int ||
				value is long ||
				value is string) { return true; }
			return false;
		}

		/// <summary>
		/// Copmute the HashCode for an IDictionary
		/// </summary>
		/// <param name="dictionary"></param>
		/// <returns></returns>
		public static int ComputeHashCode(this IDictionary dictionary)
		{
			var hashCode = dictionary.Count;
			foreach (var key in dictionary.Keys)
			{
				hashCode = hashCode ^ key.GetHashCode();
				var value = dictionary[key];
				if (value != null)
				{
					if (value is IDictionary) hashCode = hashCode ^ (value as IDictionary).ComputeHashCode();
					else
					{
						if (value is IEnumerable) hashCode = hashCode ^ (value as IEnumerable).ComputeHashCode();
						else hashCode = hashCode ^ value.GetHashCode();
					}
					/*
					if (UseValueHash(value)) hashCode = hashCode ^ value.GetHashCode();
					else
					{
						if (value is IDictionary) hashCode = hashCode ^ (value as IDictionary).ComputeHashCode();
						else
						{
							if (value is IEnumerable) hashCode = hashCode ^ (value as IEnumerable).ComputeHashCode();
						}
					}*/
				}
			}
			return hashCode;
		}

		/// <summary>
		/// Get a Stream with a serialized IDictionary
		/// </summary>
		/// <param name="dictionary"></param>
		/// <returns></returns>
		public static Stream GetStream(this IDictionary dictionary)
		{
			var memory = new MemoryStream();
			dictionary.Save(memory);
			memory.Flush();
			memory.Seek(0, SeekOrigin.Begin);
			return memory;
		}

		/// <summary>
		/// Collect IDictionary with a specific 'Type' value
		/// </summary>
		/// <param name="idictionary"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IList Collect(this IDictionary idictionary, string type)
		{
			var results = new List<object>();
			_Collect(idictionary, type, results);
			return results;
		}

		/// <summary>
		/// Collection IDictionary of a specific 'Type' value, matching a search pattern
		/// </summary>
		/// <param name="idictionary"></param>
		/// <param name="type"></param>
		/// <param name="search"></param>
		/// <returns></returns>
		public static IList Collect(this IDictionary idictionary, Type type, string search = null)
		{
			var results = new List<object>();
			_Collect(idictionary, type, search, results);
			return results;
		}

		public static IList<T> Collect<T>(this IDictionary idictionary, string search = null)
		{
			var results = new List<T>();
			_Collect<T>(idictionary, search, results, MatchesSearch);

			return results;
		}

		public static IList<T> Collect<T>(this IDictionary dictionary, Func<IDictionary, string, bool> matchFunction, string search = null)
		{
			var results = new List<T>();
			_Collect<T>(dictionary, search, results, matchFunction);

			return results;
		}

		public static IList<T> Collect<T>(this IDictionary dictionary, KeyValuePair<string, string> kvp) where T : IDictionary
		{
			var results = new List<T>();
			var tmp = dictionary.Collect<T>();
			foreach (var result in tmp)
			{
				var d = result as IDictionary;

				if (d != null && d.Contains(kvp.Key))
				{
					var value = d[kvp.Key];
					if (value != null && value.ToString() == kvp.Value)
					{
						results.Add(result);
					}
				}
			}
			return results;
		}

		private static void _Collect<T>(this IDictionary idictionary, string search, IList results, Func<IDictionary, string, bool> matchFunction)
		{
			foreach (var item in idictionary.Values)
			{
				if (item != null && item is T)//typeof(T).IsAssignableFrom(item.GetType()))
				{
					if (!results.Contains(item) && (
						search == null || matchFunction(item as IDictionary, search)))// MatchesSearch(item as IDictionary, search)))
					{
						results.Add(item);
					}
				}
				var child_idictionary = item as IDictionary;
				if (child_idictionary != null) _Collect<T>(child_idictionary, search, results, matchFunction);
			}
		}

		private static bool MatchesSearch(this IDictionary idictionary, string search)
		{
			if (search == null || search.Length == 0 || idictionary == null) return true;
			if (search.Contains(" "))
			{
				// All parts must match
				var matchesValue = false;
				var matchesKey = false;
				var words = search.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				foreach (var word in words)
				{
					matchesValue = MatchesValue(idictionary, word);
					matchesKey = idictionary.GetFullName().Contains(word);
					if (!matchesValue && !matchesKey) return false;
				}
				if (matchesValue || matchesKey) return true;
				return false;
			}
			else
			{
				if (MatchesValue(idictionary, search)) return true;
			}
			return false;
		}

		private static bool MatchesValue(this IDictionary idictionary, string search)
		{
			foreach (var key in idictionary.Keys)
			{
				var value = idictionary[key];
				if (value != null && value is string && value.ToString().Contains(search)) return true;
			}
			var mkey = ObjectExtension.GetName(idictionary);
			if (mkey.Length > 0 && search.Length > 0 && mkey.Contains(search)) return true;

			return false;
		}

		private static void _Collect(this IDictionary idictionary, Type type, string search, IList results)
		{
			foreach (var item in idictionary.Values)
			{
				if (item != null)
				{
					if (type.IsAssignableFrom(item.GetType()) && !results.Contains(item))
					//if(item.GetType().IsInstanceOfType(type) && !results.Contains(item))
					{
						if (search == null || MatchesSearch((item as IDictionary), search))
						{
							results.Add(item);
						}
					}
					var child_idictionary = item as IDictionary;
					if (child_idictionary != null) _Collect(child_idictionary, type, search, results);
				}
			}
		}

		private static void _Collect(this IDictionary idictionary, string type, IList results)
		{
			foreach (var item in idictionary.Values)
			{
				if (item != null)
				{
					if (item.GetType().Name == type && !results.Contains(item)) results.Add(item);
					else
					{
						var d = item as IDictionary;
						if (d != null && d.Contains("Type") && d["Type"].ToString() == type && !results.Contains(item))
						{
							results.Add(item);
						}
					}
					var child_idictionary = item as IDictionary;
					if (child_idictionary != null) _Collect(child_idictionary, type, results);
				}
			}
		}

		public static IList<T> CollectValues<T>(this IDictionary dictionary, string key)
		{
			var results = new List<T>();
			_CollectValues<T>(dictionary, key, results);
			return results;
		}

		private static void _CollectValues<T>(this IDictionary dictionary, string key, List<T> results)
		{
			if (dictionary.Contains(key))
			{
				var value = dictionary.Get<T>(key);
				if (!results.Contains(value)) results.Add(value);
			}
			foreach (var child_key in dictionary.Keys)
			{
				var child_dictionary = dictionary[child_key] as IDictionary;
				if (child_dictionary != null)
				{
					_CollectValues<T>(child_dictionary, key, results);
				}
			}
		}

		public static IDictionary Copy(this IDictionary dictionary, IDictionary source)
		{
			dictionary.Clear();
			foreach (var key in source.Keys)
			{
				var value = source[key];
				var child_dictionary = value as IDictionary;
				if (child_dictionary != null)
				{
					dictionary[key] = new Dictionary<object, dynamic>().Copy(child_dictionary);
				}
				else
				{
					dictionary[key] = value;
				}
			}
			return dictionary;
		}

		public static IDictionary Copy(this IDictionary dictionary, IDictionary source, Func<object, bool> valueFilterFunction, Func<object, bool> keyFilterFunction = null)
		{
			dictionary.Clear();
			foreach (var key in source.Keys)
			{
				if (keyFilterFunction == null || keyFilterFunction(key))
				{
					var value = source[key];
					if (valueFilterFunction(value))
					{
						var child_dictionary = value as IDictionary;
						if (child_dictionary != null)
						{
							dictionary[key] = new Dictionary<object, dynamic>().Copy(child_dictionary, valueFilterFunction, keyFilterFunction);
						}
						else
						{
							dictionary[key] = value;
						}
					}
				}
			}
			return dictionary;
		}

		public static IDictionary Clone(this IDictionary source)
		{
			var clone = Activator.CreateInstance(source.GetType()) as IDictionary;
			clone.Copy(source);
			return clone;
		}

		public static T Find<T>(this IDictionary dictionary, string name, bool exact = false, bool deepUpdateParents = false)
		{
			var items = dictionary.Collect<T>();
			foreach (var item in items)
			{
				if (deepUpdateParents && item.GetParent() != dictionary) { dictionary.DeepUpdateParents(); }
#if DEBUG
				var fullName = item.GetFullName();
#endif
				if (item.GetFullName() == name) return item;
			}
			foreach (var item in items)
			{
#if DEBUG
				var _name = item.GetName();
#endif
				if (item.GetName() == name) return item;
			}
			if (!exact)
			{
				foreach (var item in items)
				{
					if (item.GetName().Contains(name)) return item;
				}
				foreach (var item in items)
				{
					if (item.GetFullName().Contains(name)) return item;
				}
			}
			return default(T);
		}

		public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default(T), bool search = false)
		{
			if (name == null) return defaultValue;
			if (name.IndexOf(',') > -1)
			{
				int startIndex = 0;
				int nextIndex = name.IndexOf(',');
				while (startIndex < name.Length)
				{
					if (dictionary.Contains(name.Substring(startIndex, nextIndex - startIndex))) return dictionary.Get<T>(name.Substring(startIndex, nextIndex - startIndex));
					startIndex = nextIndex + 1;
					if (startIndex < name.Length)
					{
						nextIndex = name.IndexOf(',', startIndex);
						if (nextIndex < 0) nextIndex = name.Length - 1;
					}
				}
			}
			if (dictionary.Contains(name))
			{
				var value = dictionary[name];
				if (value != null)
				{
					var templateType = typeof(T);
					if (value is T) return (T)value;
					if (templateType == typeof(double))
					{
						return (T)(object)Convert.ToDouble(value);
					}
					if (templateType == typeof(float))
					{
						return (T)(object)Convert.ToSingle(value);
					}
				}
			}

			// Search for matching name
			if (search)
			{
				var items = dictionary.Collect<T>();
				foreach (var item in items)
				{
					if (item.GetFullName() == name) return item;
				}
				foreach (var item in items)
				{
					if (item.GetName() == name) return item;
				}
				foreach (var item in items)
				{
					if (item.GetName().Contains(name)) return item;
				}
				foreach (var item in items)
				{
					if (item.GetFullName().Contains(name)) return item;
				}
			}

			if (typeof(T) == typeof(string) && defaultValue == null) return (T)(object)string.Empty;
			return defaultValue;
		}

		public static void Set(this IDictionary dictionary, string key, object value)
		{
			if (key.Contains("/")) SetValue(dictionary, key, value);
			else
			{
				if (value != null && value.GetType() == typeof(DateTime))
				{
					dictionary[key] = ((DateTime)value).ToString("o");
				}
				else { dictionary[key] = value; }
			}
		}

		private static void SetValue(IDictionary dictionary, string key, object value)
		{
			if (dictionary != null)
			{
				if (key.Contains("/"))
				{
					var parts = key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					if (parts.Length > 1)
					{
						var child_key = parts[0];
						var child_subkey = String.Join("/", parts, 1, parts.Length - 1);
						IDictionary child = null;
						if (dictionary.Contains(child_key)) child = dictionary[child_key] as IDictionary;
						if (child == null)
						{
							child = new Dictionary<string, dynamic>();
							//dictionary[parts[0]] = child;
						}
						SetValue(child, child_subkey, value);
						dictionary[child_key] = child;
					}
				}
				else Set(dictionary, key, value);
				//dictionary[key] = value;
			}
		}

		public static string GetName(this IDictionary dictionary)
		{
			var parent = dictionary.GetParent() as IDictionary;// GetParent(dictionary) as IDictionary;
			if (parent != null)
			{
				foreach (string key in parent.Keys)
				{
					var test_element = parent.Get<IDictionary>(key);
					if (test_element != null)
					{
						if (object.ReferenceEquals(test_element, dictionary)) return key;
					}
				}
			}
			return ObjectExtension.GetName(dictionary);
			//return string.Empty;
		}

		public static string GetFullName(this IDictionary dictionary)
		{
			var key = GetName(dictionary);
			if (key != null)
			{
				var parent = dictionary.GetParent() as IDictionary;// GetParent(dictionary) as IDictionary;
				if (parent != null)
				{
					var parent_full_key = GetFullName(parent);
					if (parent_full_key.Length > 0)
					{
						return $"{parent_full_key}/{key.ToString()}";
					}
				}
				return key.ToString();
			}
			return string.Empty;
		}

		public static string GetTypeName(this IDictionary source, string typeKey = "Type")
		{
			if (source.Contains(typeKey)) return source.Get<string>(typeKey);
			return string.Empty;
		}

		public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type")
		{
			return ConvertTypes(source, types, typeof(Dictionary<string, dynamic>), typeKey);
		}

		public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, Type defaultType, string typeKey = "Type")
		{
			if (source == null) return null;
			if (types == null) return source;
			var copy = Activator.CreateInstance(source.GetType()) as IDictionary;
			if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
			var typename = source.Get<string>(typeKey, "");
			//if (typename.Length > 0)// && types.ContainsKey(typename))
			//{
			var targetType = defaultType;
			if (types.ContainsKey(typename))
			{
				targetType = types[typename];
				if (targetType == null) throw new Exception($"types['{typename}'] was null");
			}
			if (source.GetType() != targetType)
			{
				copy = Activator.CreateInstance(targetType) as IDictionary;
				if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
			}
			//}
			foreach (string key in source.Keys)
			{
				var value = source[key];
				var childDictionary = value as IDictionary;
				if (childDictionary != null)
				{
					copy[key] = ConvertTypes(childDictionary, types, defaultType, typeKey);
				}
				else
				{
					var childEnumerable = value as IEnumerable;
					if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
					{
						copy[key] = childEnumerable.ConvertTypes(types, defaultType, typeKey);
					}
					else
					{
						copy[key] = value;
					}
				}
			}
			return copy;
		}

		public static Matrix3D GetLocalToParent(this IDictionary dictionary)
		{
			var matrix3D = new Matrix3D();
			if (dictionary != null)
			{
				matrix3D = new Factory().Create<Matrix3D>(dictionary);
			}
			return matrix3D;
		}

		public static Matrix3D GetLocalToWorld(this IDictionary dictionary)
		{
			var localToWorld = GetLocalToParent(dictionary);
			if (dictionary != null)
			{
				var parent = dictionary.GetParent();
				if (parent != null)
				{
					localToWorld.Append(GetLocalToWorld(parent as IDictionary));
				}
			}

			return localToWorld;
		}

		public static Matrix3D GetWorldToLocal(this IDictionary dictionary)
		{
			var m = GetLocalToWorld(dictionary);
			m.Invert();
			return m;
		}

		public static Point3D GetWorldOrigin(this IDictionary dictionary)
		{
			return GetLocalToWorld(dictionary).Transform(new Point3D(0, 0, 0));
		}

		public static void SetWorldOrigin(this IDictionary dictionary, Point3D world_origin)
		{
			var parent = dictionary.GetNearestAncestor<IDictionary>();
			var local_origin = world_origin;
			if (parent != null)
			{
				var parent_world_origin = parent.GetWorldOrigin();
				var delta = world_origin - parent_world_origin;
				local_origin = new Point3D(delta.X, delta.Y, delta.Z);
			}

			dictionary["X"] = $"{local_origin.X} m";
			dictionary["Y"] = $"{local_origin.Y} m";
			dictionary["Z"] = $"{local_origin.Z} m";
		}

		public static Point3D GetOrigin(this IDictionary dictionary)
		{
			return GetLocalToParent(dictionary).Transform(new Point3D(0, 0, 0));
		}

		public static void SetOrigin(this IDictionary dictionary, Point3D origin)
		{
			dictionary["X"] = $"{origin.X} m";
			dictionary["Y"] = $"{origin.Y} m";
			dictionary["Z"] = $"{origin.Z} m";
		}

		public static Vector3D GetWorldRotations(this IDictionary dictionary)
		{
			return GetLocalToWorld(dictionary).GetRotationsXYZ();
		}

		public static Vector3D GetRotations(this IDictionary dictionary)
		{
			return GetLocalToParent(dictionary).GetRotationsXYZ();
		}

		public static IDictionary GetAncestor(this IDictionary child, string key, string value)
		{
			var parent = child.GetParent() as IDictionary;
			if (child != null && parent != null)
			{
				if (parent.Contains(key))
				{
					if (parent[key].ToString() == value) return parent;
				}
				return parent.GetAncestor(key, value);
			}
			return null;
		}

		public static T GetNearestAncestor<T>(this IDictionary child)
		{
			var parent = child.GetParent() as IDictionary;
			if (child != null && parent != null)
			{
				if (typeof(T).IsAssignableFrom(parent.GetType()))
				{
					var ancestor = (T)parent;
					if (ancestor != null) return ancestor;
				}
				return GetNearestAncestor<T>(parent);
			}
			return default(T);
		}

		public static T GetFurthestAncestor<T>(this IDictionary child)
		{
			if (child != null)
			{
				var ancestor = GetNearestAncestor<T>(child) as IDictionary;
				if (ancestor != null)
				{
					var further_ancestor = GetFurthestAncestor<T>(ancestor);
					if (further_ancestor != null) return further_ancestor;
				}
				if (ancestor == null)
				{
					if (typeof(T).IsAssignableFrom(child.GetType()))
					{
						ancestor = (IDictionary)(T)(object)child;
					}
				}
				return (T)ancestor;
			}
			return default(T);
		}

		public static object GetRootAncestor(this IDictionary child)
		{
			return GetFurthestAncestor<IDictionary>(child);
		}

		public static int CompareTo(this IDictionary a, IDictionary b)
		{
			// Less than zero The current instance precedes the object specified by the CompareTo
			// method in the sort order.
			//
			//
			// Greater than zero This current instance follows the object specified by the CompareTo
			// method in the sort order.
			if (b == null) return 1;
			var countCompare = a.Count.CompareTo(b.Count);
			if (countCompare != 0) return countCompare;

			if (a.Count > 0)
			{
				var aEnumerator = a.Keys.GetEnumerator();
				var bEnumerator = b.Keys.GetEnumerator();

				while (aEnumerator.MoveNext())
				{
					bEnumerator.MoveNext();
					var aKey = aEnumerator.Current as IComparable;
					var bKey = bEnumerator.Current as IComparable;
					var keyCompare = 0;
					if (aKey != null)
					{
						keyCompare = aKey.CompareTo(bKey);
						if (keyCompare != 0) return keyCompare;

						var aValue = a[aKey] as IComparable;
						var bValue = b[bKey] as IComparable;
						var valueCompare = 0;
						if (aValue != null)
						{
							valueCompare = aValue.CompareTo(bValue);
							if (valueCompare != 0) return valueCompare;
						}
					}
				}
			}
			return 0;
		}

		public static T GetCurrent<T>(this IDictionary dictionary) where T : IDictionary
		{
			var metaData = Internal.MetaData.Default.GetMetaData(dictionary);
			if (metaData.Contains("Currents"))
			{
				var currents = Internal.MetaData.Default.GetMetaData(dictionary)["Currents"] as IDictionary;
				if (currents != null)
				{
					if (currents.Contains(typeof(T)))
					{
						var current_name = currents[typeof(T)].ToString();
						return dictionary.Find<T>(current_name);
					}
				}
			}

			var items = dictionary.Collect<T>();
			if (items.Count > 0) return items[0];
			return default(T);
		}

		public static void SetCurrent<T>(this IDictionary dictionary, string name) where T : IDictionary
		{
			var metaData = Internal.MetaData.Default.GetMetaData(dictionary);
			if (!metaData.Contains("Currents")) { metaData.Add("Currents", new Dictionary<Type, string>()); }
			var currents = Internal.MetaData.Default.GetMetaData(dictionary)["Currents"] as IDictionary;
			currents[typeof(T)] = name;
		}

		public static IList<string> CollectNames<T>(this IDictionary element)
		{
			var items = element.Collect<T>();
			var names = new List<string>();
			foreach (object child in items)
			{
				if (child != null)
				{
					var name = Node.Net.ObjectExtension.GetName(child);
					if (name != null && name.Length > 0 && !names.Contains(name))
					{
						names.Add(name);
					}
				}
			}
			return names;
		}

		public static IList<T> Collect<T>(this IDictionary dictionary, Func<object, bool> filter)
			=> new List<T>(dictionary.Collect<T>().Where(x => filter(x)));

		public static string GetUniqueKey(this IDictionary dictionary, string baseName)
		{
			if (!dictionary.Contains(baseName)) return baseName;
			for (int i = 1; i < 10000; ++i)
			{
				var key = $"{baseName}{i}";
				if (!dictionary.Contains(key)) return key;
			}
			return baseName;
		}
	}
}