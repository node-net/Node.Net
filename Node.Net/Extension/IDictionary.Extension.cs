﻿using System;
using System.Collections;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Media.Media3D;
using static System.Math;

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
            return new Internal.JsonWriter().WriteToString(dictionary);
        }

        /// <summary>
        /// Save to a stream
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="stream"></param>
        public static void Save(this IDictionary dictionary, Stream stream)
        {
            new Internal.JsonWriter().Write(stream, dictionary);
        }

        /// <summary>
        /// Deep Update Parent references
        /// </summary>
        /// <param name="dictionary"></param>
        public static void DeepUpdateParents(this IDictionary dictionary)
        {
            if (dictionary is null)
            {
                return;
            }

            List<object>? values = new List<object>();
            foreach (object? value in dictionary.Values)
            {
                if (value != null)
                {
                    values.Add(value);
                }
            }
            foreach (object? value in values)
            {
                if (value is IDictionary child)
                {
                    child.SetParent(dictionary);
                    DeepUpdateParents(child);
                }
            }
        }

        public static void DeepClean(this IDictionary dictionary)
        {
            foreach (object? value in dictionary.Values)
            {
                if (value is IDictionary child)
                {
                    child.DeepClean();
                }
            }
            dictionary.Clear();
            dictionary.ClearMetaData();
        }

        /// <summary>
        /// Get the Length in meters from a value in a dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double GetLengthMeters(this IDictionary dictionary, string name)
        {
            string? svalue = dictionary.Get<string>(name);
            return Length.GetMeters(svalue);
        }

        /// <summary>
        /// Get the angle in degrees from an IDictionary value
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static double GetAngleDegrees(this IDictionary dictionary, string name)
        {
            string? svalue = dictionary.Get<string>(name);
            return Internal.Angle.GetDegrees(svalue);
        }

        /*
		private static bool UseValueHash(object value)
		{
			if (value is bool
				|| value is double
				|| value is float
				|| value is int
				|| value is long
				|| value is string) { return true; }
			return false;
		}*/

        /// <summary>
        /// Copmute the HashCode for an IDictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static int ComputeHashCode(this IDictionary dictionary)
        {
            int hashCode = dictionary.Count;
            foreach (object? key in dictionary.Keys)
            {
                if (key != null)
                {
                    hashCode ^= key.GetHashCode();
                    object? value = dictionary[key];
                    if (value != null)
                    {
                        if (value is IDictionary vdictionary)
                        {
                            hashCode ^= vdictionary.ComputeHashCode();
                        }
                        else
                        {
                            if (value is IEnumerable venumerable)
                            {
                                hashCode ^= venumerable.ComputeHashCode();
                            }
                            else
                            {
                                hashCode ^= value.GetHashCode();
                            }
                        }
                    }
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
            MemoryStream? memory = new MemoryStream();
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
        public static IList<object> Collect(this IDictionary idictionary, string type)
        {
            List<object>? results = new List<object>();
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
        public static IList<object> Collect(this IDictionary idictionary, Type type, string? search = null)
        {
            List<object>? results = new List<object>();
            _Collect(idictionary, type, search, results);
            return results;
        }

        public static IList<T> Collect<T>(this IDictionary idictionary, string? search = null)
        {
            List<T>? results = new List<T>();
            _Collect<T>(idictionary, search, results, MatchesSearch);

            return results;
        }

        public static IList<T> Collect<T>(this IDictionary dictionary, Func<IDictionary, string, bool> matchFunction, string? search = null)
        {
            List<T>? results = new List<T>();
            _Collect<T>(dictionary, search, results, matchFunction);

            return results;
        }

        public static IList<T> Collect<T>(this IDictionary dictionary, KeyValuePair<string, string> kvp) where T : IDictionary
        {
            List<T>? results = new List<T>();
            IList<T>? tmp = dictionary.Collect<T>();
            foreach (T result in tmp)
            {
                if (result is IDictionary d && d.Contains(kvp.Key))
                {
                    object? value = d[kvp.Key];
                    if (value != null && value.ToString() == kvp.Value)
                    {
                        results.Add(result);
                    }
                }
            }
            return results;
        }

        private static void _Collect<T>(this IDictionary idictionary, string? search, IList results, Func<IDictionary, string, bool> matchFunction)
        {
            foreach (object? item in idictionary.Values)
            {
                if (item != null && item is T && !results.Contains(item) && (
#pragma warning disable CS8604 // Possible null reference argument.
                        search == null || matchFunction(item as IDictionary, search)))
#pragma warning restore CS8604 // Possible null reference argument.
                {
                    results.Add(item);
                }
                if (item is IDictionary child_idictionary)
                {
                    _Collect<T>(child_idictionary, search, results, matchFunction);
                }
            }
        }

        public static bool MatchesSearch(this IDictionary idictionary, string search)
        {
            if (string.IsNullOrEmpty(search) || idictionary == null)
            {
                return true;
            }

            if (search.Contains(" "))
            {
                // All parts must match
                bool matchesValue = false;
                bool matchesKey = false;
                string[]? words = search.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string? word in words)
                {
                    matchesValue = MatchesValue(idictionary, word);
                    matchesKey = idictionary.GetFullName().Contains(word);
                    if (!matchesValue && !matchesKey)
                    {
                        return false;
                    }
                }
                return matchesValue || matchesKey;
            }
            else
            {
                if (MatchesValue(idictionary, search))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool MatchesValue(this IDictionary idictionary, string search)
        {
            foreach (object? key in idictionary.Keys)
            {
                if (key != null)
                {
                    object? value = idictionary[key];
                    if (value is string svalue)
                    {
                        if (svalue.Contains(search)) return true;
                    }
                }
            }
            string? mkey = ObjectExtension.GetName(idictionary);
            return mkey.Length > 0 && search.Length > 0 && mkey.Contains(search);
        }

        private static void _Collect(this IDictionary idictionary, Type type, string? search, IList results)
        {
            foreach (object? item in idictionary.Values)
            {
                if (item != null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (type.IsInstanceOfType(item) && !results.Contains(item) && (search == null || MatchesSearch(item as IDictionary, search)))
#pragma warning restore CS8604 // Possible null reference argument.
                    //if(item.GetType().IsInstanceOfType(type) && !results.Contains(item))
                    {
                        results.Add(item);
                    }
                    if (item is IDictionary child_idictionary)
                    {
                        _Collect(child_idictionary, type, search, results);
                    }
                }
            }
        }

        private static void _Collect(this IDictionary idictionary, string type, IList results)
        {
            foreach (object? item in idictionary.Values)
            {
                if (item != null)
                {
                    if (item.GetType().Name == type && !results.Contains(item))
                    {
                        results.Add(item);
                    }
                    else
                    {
                        if (item is IDictionary d && d.Contains("Type"))// && d["Type"].ToString() == type && !results.Contains(item))
                        {
                            if (d["Type"] is string svalue)
                            {
                                if (svalue == type && !results.Contains(item))
                                {
                                    results.Add(item);
                                }
                            }

                            //results.Add(item);
                        }
                    }
                    if (item is IDictionary child_idictionary)
                    {
                        _Collect(child_idictionary, type, results);
                    }
                }
            }
        }

        public static IDictionary<string, int> CollectKeys(this IDictionary dictionary)
        {
            Dictionary<string, int>? results = new Dictionary<string, int>();
            foreach (object? okey in dictionary.Keys)
            {
                if (okey is string key)
                {
                    if (key != null)
                    {
                        if (key.Length > 0)
                        {
                            if (!results.ContainsKey(key))
                            {
                                results.Add(key, 1);
                            }
                            else
                            {
                                results[key]++;
                            }

                            if (dictionary[key] is IDictionary subDictionary)
                            {
                                IDictionary<string, int>? subKeys = subDictionary.CollectKeys();
                                foreach (string? subKey in subKeys.Keys)
                                {
                                    if (!results.ContainsKey(key))
                                    {
                                        results.Add(key, subKeys[subKey]);
                                    }
                                    else
                                    {
                                        results[key] += subKeys[subKey];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return results;
        }

        public static IList<T> CollectValues<T>(this IDictionary dictionary, string key)
        {
            List<T>? results = new List<T>();
            _CollectValues<T>(dictionary, key, results);
            return results;
        }

        private static void _CollectValues<T>(this IDictionary dictionary, string key, List<T> results)
        {
            if (dictionary.Contains(key))
            {
                T value = dictionary.Get<T>(key);
                if (!results.Contains(value))
                {
                    results.Add(value);
                }
            }
            foreach (object? child_key in dictionary.Keys)
            {
                if (child_key != null)
                {
                    if (dictionary[child_key] is IDictionary child_dictionary)
                    {
                        _CollectValues<T>(child_dictionary, key, results);
                    }
                }
            }
        }

        public static IDictionary Copy(this IDictionary dictionary, IDictionary source)
        {
            dictionary.Clear();
            foreach (object? key in source.Keys)
            {
                if (key != null)
                {
                    object? value = source[key];
                    if (value is IDictionary child_dictionary)
                    {
                        dictionary[key] = new Dictionary<object, dynamic>().Copy(child_dictionary);
                    }
                    else
                    {
                        dictionary[key] = value;
                    }
                }
            }
            return dictionary;
        }

        public static IDictionary Copy(this IDictionary dictionary, IDictionary source, Func<object, bool> valueFilterFunction, Func<object, bool>? keyFilterFunction = null)
        {
            dictionary.Clear();
            foreach (object? key in source.Keys)
            {
                if (key != null)
                {
                    if (keyFilterFunction == null || keyFilterFunction(key))
                    {
                        object? value = source[key];
                        if (value != null)
                        {
                            if (valueFilterFunction(value))
                            {
                                if (value is IDictionary child_dictionary)
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
                }
            }
            return dictionary;
        }

        public static IDictionary Clone(this IDictionary source)
        {
            IDictionary? clone = Activator.CreateInstance(source.GetType()) as IDictionary;
#pragma warning disable CS8604 // Possible null reference argument.
            clone.Copy(source);
#pragma warning restore CS8604 // Possible null reference argument.
            return clone!;
        }

        public static T Find<T>(this IDictionary dictionary, string name, bool exact = false, bool deepUpdateParents = false)
        {
            IList<T>? items = dictionary.Collect<T>();
            foreach (T item in items)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                if (deepUpdateParents && item.GetParent() != dictionary) { dictionary.DeepUpdateParents(); }
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                if (item.GetFullName() == name)
#pragma warning restore CS8604 // Possible null reference argument.
                {
                    return item;
                }
            }
            foreach (T item in items)
            {
#if DEBUG
#pragma warning disable CS8604 // Possible null reference argument.
                string? _name = item.GetName();
#pragma warning restore CS8604 // Possible null reference argument.
#endif
#pragma warning disable CS8604 // Possible null reference argument.
                if (item.GetName() == name)
#pragma warning restore CS8604 // Possible null reference argument.
                {
                    return item;
                }
            }
            if (!exact)
            {
                foreach (T item in items)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (item.GetName().Contains(name))
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return item;
                    }
                }
                foreach (T item in items)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (item.GetFullName().Contains(name))
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return item;
                    }
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default, bool search = false)
        {
            if (name == null)
            {
                return defaultValue;
            }

            if (name.IndexOf(',') > -1)
            {
                int startIndex = 0;
                int nextIndex = name.IndexOf(',');
                while (startIndex < name.Length)
                {
                    string? subname = name.Substring(startIndex, nextIndex - startIndex);
                    if (dictionary.Contains(subname))
                    {
                        return dictionary.Get<T>(subname);
                    }

                    startIndex = nextIndex + 1;
                    if (startIndex < name.Length)
                    {
                        nextIndex = name.IndexOf(',', startIndex);
                        if (nextIndex < 0)
                        {
                            nextIndex = name.Length;// - 1;
                        }
                    }
                }
            }
            if (dictionary.Contains(name))
            {
                object? value = dictionary[name];
                if (value != null)
                {
                    Type? templateType = typeof(T);
                    if (value is T t)
                    {
                        return t;
                    }

                    if (templateType == typeof(double))
                    {
                        return (T)(object)System.Convert.ToDouble(value);
                    }
                    if (templateType == typeof(float))
                    {
                        return (T)(object)System.Convert.ToSingle(value);
                    }
                }
            }

            // Search for matching name
            if (search)
            {
                IList<T>? items = dictionary.Collect<T>();
                foreach (T item in items)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (item.GetFullName() == name)
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return item;
                    }
                }
                foreach (T item in items)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (item.GetName() == name)
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return item;
                    }
                }
                foreach (T item in items)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (item.GetName().Contains(name))
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return item;
                    }
                }
                foreach (T item in items)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (item.GetFullName().Contains(name))
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return item;
                    }
                }
            }

            return typeof(T) == typeof(string) && EqualityComparer<T>.Default.Equals(
                defaultValue,
#pragma warning disable CS8604 // Possible null reference argument.
                default)
#pragma warning restore CS8604 // Possible null reference argument.
                ? (T)(object)string.Empty
                : defaultValue;
        }

        public static void Set(this IDictionary dictionary, string key, object value)
        {
            if (key.Contains("/"))
            {
                SetValue(dictionary, key, value);
            }
            else
            {
                if (value != null && (value is DateTime))
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
                    string[]? parts = key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 1)
                    {
                        string? child_key = parts[0];
                        string? child_subkey = String.Join("/", parts, 1, parts.Length - 1);
                        IDictionary? child = null;
                        if (dictionary.Contains(child_key))
                        {
                            child = dictionary[child_key] as IDictionary;
                        }

                        if (child == null)
                        {
                            child = new Dictionary<string, dynamic>();
                        }
                        SetValue(child, child_subkey, value);
                        dictionary[child_key] = child;
                    }
                }
                else
                {
                    Set(dictionary, key, value);
                }
            }
        }

        public static string GetName(this IDictionary dictionary)
        {
            if (dictionary.GetParent() is IDictionary parent)
            {
                foreach (object? okey in parent.Keys)
                {
                    if (okey is string key)
                    {
                        IDictionary? test_element = parent.Get<IDictionary>(key);
                        if (test_element != null && object.ReferenceEquals(test_element, dictionary))
                        {
                            return key;
                        }
                    }
                }
            }
            return ObjectExtension.GetName(dictionary);
        }

        public static string GetFullName(this IDictionary dictionary)
        {
            string? key = GetName(dictionary);
            if (key != null)
            {
                if (dictionary.GetParent() is IDictionary parent)
                {
                    string? parent_full_key = GetFullName(parent);
                    if (parent_full_key.Length > 0)
                    {
                        return $"{parent_full_key}/{key}";
                    }
                }
                return key;
            }
            return string.Empty;
        }

        public static string GetTypeName(this IDictionary source, string typeKey = "Type")
        {
            if (source.Contains(typeKey))
            {
                return source.Get<string>(typeKey);
            }

            return string.Empty;
        }

        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            return ConvertTypes(source, types, typeof(Dictionary<string, dynamic>), typeKey);
        }

        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, Type defaultType, string typeKey = "Type")
        {
            if (types == null)
            {
                return source;
            }

            if (!(Activator.CreateInstance(source.GetType()) is IDictionary copy))
            {
                throw new InvalidOperationException($"failed to create instance of type {source.GetType().FullName}");
            }

            string? typename = source.Get<string>(typeKey, "");
            Type? targetType = defaultType;
            if (types.ContainsKey(typename))
            {
                targetType = types[typename];
                if (targetType == null)
                {
                    throw new InvalidOperationException($"types['{typename}'] was null");
                }
            }
            if (source.GetType() != targetType)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                copy = Activator.CreateInstance(targetType) as IDictionary;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (copy == null)
                {
                    throw new InvalidOperationException($"failed to create instance of type {targetType.FullName}");
                }
            }
            List<string>? keys = new List<string>();
            foreach (string? key in source.Keys)
            {
                if (key != null)
                {
                    keys.Add(key);
                }
            }
            //foreach (string key in source.Keys)
            foreach (string key in keys)
            {
                object? value = source[key];
                if (value is IDictionary childDictionary)
                {
                    copy[key] = ConvertTypes(childDictionary, types, defaultType, typeKey);
                }
                else
                {
                    if (value is IEnumerable childEnumerable && !(childEnumerable is string))
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

        public static object Convert(this IDictionary value, Type type)
        {
            SerializationInfo serializationInfo = value.GetSerializationInfo(type);

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                IDictionary? data = serializationInfo.GetPersistentData();
                ConstructorInfo? defaultConstructor = type.GetConstructor(Type.EmptyTypes);
                if (defaultConstructor is null)
                {
                    throw new InvalidOperationException($"no default constructor availabe for type {type.FullName}");
                }
                IDictionary newDictionary = (defaultConstructor.Invoke(Array.Empty<object>()) as IDictionary)!;
                foreach (object? key in data.Keys)
                {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                    newDictionary.Add(key, data[key]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                }
                return newDictionary!;
            }

            StreamingContext streamingContext = new StreamingContext();
            Type[]? paramTypes = new Type[] { typeof(SerializationInfo), typeof(StreamingContext) };
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            ConstructorInfo ci = type.GetConstructor(
                                    BindingFlags.Instance | BindingFlags.NonPublic,
                                    null, paramTypes, null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
            return ci?.Invoke(new object[] { serializationInfo, streamingContext });
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static T Convert<T>(this IDictionary value)
        {
            object? item = value.Convert(typeof(T));
            return (T)item;
        }

        public static SerializationInfo GetSerializationInfo(this IDictionary dictionary, Type type)
        {
            SerializationInfo? info = new SerializationInfo(type, new FormatterConverter());
            foreach (string? key in dictionary.Keys)
            {
                if (key != null)
                {
                    info.AddValue(key, dictionary[key]);
                }
            }
            return info;
        }

        public static Matrix3D GetLocalToParent(this IDictionary dictionary)
        {
            Matrix3D matrix3D = new Matrix3D();
            if (dictionary != null)
            {
                matrix3D = new Factory().Create<Matrix3D>(dictionary);
            }
            return matrix3D;
        }

        public static Matrix3D GetLocalToWorld(this IDictionary dictionary)
        {
            Matrix3D localToWorld = GetLocalToParent(dictionary);
            if (dictionary != null)
            {
                object? parent = dictionary.GetParent();
                if (parent != null)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    localToWorld.Append(GetLocalToWorld(parent as IDictionary));
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }

            return localToWorld;
        }

        public static Matrix3D GetParentToWorld(this IDictionary dictionary)
        {
            Matrix3D parentToWorld = new Matrix3D();
            if (dictionary != null)
            {
                if (dictionary.GetParent() is IDictionary parent)
                {
                    return parent.GetLocalToWorld();
                }
            }
            return parentToWorld;
        }

        public static Matrix3D GetWorldToParent(this IDictionary dictionary)
        {
            Matrix3D worldToParent = new Matrix3D();
            if (dictionary != null)
            {
                if (dictionary.GetParent() is IDictionary parent)
                {
                    return parent.GetWorldToLocal();
                }
            }
            return worldToParent;
        }

        public static Matrix3D GetWorldToLocal(this IDictionary dictionary)
        {
            Matrix3D m = GetLocalToWorld(dictionary);
            m.Invert();
            return m;
        }

        public static Point3D GetWorldOrigin(this IDictionary dictionary)
        {
            return GetLocalToWorld(dictionary).Transform(new Point3D(0, 0, 0));
        }

        public static void SetWorldOrigin(this IDictionary dictionary, Point3D world_origin)
        {
            IDictionary? parent = dictionary.GetNearestAncestor<IDictionary>();
            Point3D local_origin = world_origin;
            if (parent != null)
            {
                Point3D parent_world_origin = parent.GetWorldOrigin();
                Vector3D delta = world_origin - parent_world_origin;
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

        public static void SetRotations(this IDictionary dictionary, Vector3D rotations)
        {
            dictionary["RotationX"] = $"{rotations.X} deg";
            dictionary["RotationY"] = $"{rotations.Y} deg";
            dictionary["RotationZ"] = $"{rotations.Z} deg";
            if (dictionary.Contains("Orientation")) { dictionary.Remove("Orientation"); }
            if (dictionary.Contains("Tilt")) { dictionary.Remove("Tilt"); }
            if (dictionary.Contains("Spin")) { dictionary.Remove("Spin"); }
        }

        public static void SetRotationsOTS(this IDictionary dictionary, Vector3D ots)
        {
            const double zero_tolerance = 0.001;
            if (Abs(ots.X) > zero_tolerance)
            {
                dictionary["Orientation"] = $"{ots.X} deg";
            }
            else
            {
                if (dictionary.Contains("Orientation")) { dictionary.Remove("Orientation"); }
            }
            if (Abs(ots.Y) > zero_tolerance)
            {
                dictionary["Tilt"] = $"{ots.Y} deg";
            }
            else
            {
                if (dictionary.Contains("Tilt")) { dictionary.Remove("Tilt"); }
            }

            if (Abs(ots.Z) > zero_tolerance)
            {
                dictionary["Spin"] = $"{ots.Z} deg";
            }
            else
            {
                if (dictionary.Contains("Spin")) { dictionary.Remove("Spin"); }
            }
            if (dictionary.Contains("RotationX")) { dictionary.Remove("RotationX"); }
            if (dictionary.Contains("RotationY")) { dictionary.Remove("RotationY"); }
            if (dictionary.Contains("RotationZ")) { dictionary.Remove("RotationZ"); }
            if (dictionary.Contains("XDirection")) { dictionary.Remove("XDirection"); }
            if (dictionary.Contains("YDirection")) { dictionary.Remove("YDirection"); }
            if (dictionary.Contains("ZDirection")) { dictionary.Remove("ZDirection"); }
        }

        public static Vector3D GetRotationsOTS(this IDictionary dictionary)
        {
            return new Vector3D(dictionary.GetLocalToParent().GetOrientation(),
                dictionary.GetLocalToParent().GetTilt(),
                dictionary.GetLocalToParent().GetSpin());
        }

        public static IDictionary? GetAncestor(this IDictionary child, string key, string value)
        {
            if (child?.GetParent() is IDictionary parent)
            {
                if (parent.Contains(key) && parent[key]!.ToString() == value)
                {
                    return parent;
                }
                return parent.GetAncestor(key, value);
            }
            return null;
        }

        public static T GetNearestAncestor<T>(this IDictionary child)
        {
            if (child?.GetParent() is IDictionary parent)
            {
                if (parent is T ancestor && !EqualityComparer<T>.Default.Equals(
                    ancestor,
#pragma warning disable CS8604 // Possible null reference argument.
                    default))
#pragma warning restore CS8604 // Possible null reference argument.
                {
                    return ancestor;
                }
                return GetNearestAncestor<T>(parent);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static T GetFurthestAncestor<T>(this IDictionary child)
        {
            if (child != null)
            {
                IDictionary? ancestor = GetNearestAncestor<T>(child) as IDictionary;
                if (ancestor != null)
                {
                    T further_ancestor = GetFurthestAncestor<T>(ancestor);
#pragma warning disable CS8604 // Possible null reference argument.
                    if (!EqualityComparer<T>.Default.Equals(further_ancestor, default))
#pragma warning restore CS8604 // Possible null reference argument.
                    {
                        return further_ancestor;
                    }
                }
                if (ancestor == null && child is T)
                {
                    ancestor = (IDictionary)(T)child;
                }
#pragma warning disable CS8603 // Possible null reference return.
                return (T)ancestor;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
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
            if (b == null)
            {
                return 1;
            }

            int countCompare = a.Count.CompareTo(b.Count);
            if (countCompare != 0)
            {
                return countCompare;
            }

            if (a.Count > 0)
            {
                IEnumerator? aEnumerator = a.Keys.GetEnumerator();
                IEnumerator? bEnumerator = b.Keys.GetEnumerator();

                while (aEnumerator.MoveNext())
                {
                    bEnumerator.MoveNext();
                    IComparable? bKey = bEnumerator.Current as IComparable;
                    int keyCompare = 0;
                    if (aEnumerator.Current is IComparable aKey)
                    {
                        keyCompare = aKey.CompareTo(bKey);
                        if (keyCompare != 0)
                        {
                            return keyCompare;
                        }

#pragma warning disable CS8604 // Possible null reference argument.
                        IComparable? bValue = b[bKey] as IComparable;
#pragma warning restore CS8604 // Possible null reference argument.
                        int valueCompare = 0;
                        if (a[aKey] is IComparable aValue)
                        {
                            valueCompare = aValue.CompareTo(bValue);
                            if (valueCompare != 0)
                            {
                                return valueCompare;
                            }
                        }
                    }
                }
            }
            return 0;
        }

        public static T GetCurrent<T>(this IDictionary dictionary) where T : IDictionary
        {
            IDictionary? metaData = Internal.MetaData.Default.GetMetaData(dictionary);
            if (metaData.Contains("Currents") && Internal.MetaData.Default.GetMetaData(dictionary)["Currents"] is IDictionary currents && currents.Contains(typeof(T)))
            {
                string? current_name = currents[typeof(T)]?.ToString();
                return dictionary.Find<T>(current_name!);
            }

            IList<T>? items = dictionary.Collect<T>();
            if (items.Count > 0)
            {
                return items[0];
            }

#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static void SetCurrent<T>(this IDictionary dictionary, string name) where T : IDictionary
        {
            IDictionary? metaData = Internal.MetaData.Default.GetMetaData(dictionary);
            if (!metaData.Contains("Currents")) { metaData.Add("Currents", new Dictionary<Type, string>()); }
            IDictionary? currents = Internal.MetaData.Default.GetMetaData(dictionary)["Currents"] as IDictionary;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            currents[typeof(T)] = name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public static IList<string> CollectNames<T>(this IDictionary element)
        {
            IList<T>? items = element.Collect<T>();
            List<string>? names = new List<string>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            foreach (object child in items)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            {
                if (child != null)
                {
                    string? name = child.GetName();
                    if (name?.Length > 0 && !names.Contains(name))
                    {
                        names.Add(name);
                    }
                }
            }
            return names;
        }

        public static IList<T> Collect<T>(this IDictionary dictionary, Func<object, bool> filter)
#pragma warning disable CS8604 // Possible null reference argument.
            => new List<T>(dictionary.Collect<T>().Where(x => filter(x)));
#pragma warning restore CS8604 // Possible null reference argument.

        public static string GetUniqueKey(this IDictionary dictionary, string baseName)
        {
            if (!dictionary.Contains(baseName))
            {
                return baseName;
            }

            for (int i = 1; i < 10000; ++i)
            {
                string? key = $"{baseName}{i}";
                if (!dictionary.Contains(key))
                {
                    return key;
                }
            }
            return baseName;
        }

        public static IDictionary<string, object> ConvertRotationsXYZtoOTS(this IDictionary<string, object> dictionary)
        {
            Dictionary<string, object>? result = new Dictionary<string, object>();
            foreach (string key in dictionary.Keys)
            {
                if (key != "RotationX" && key != "RotationY" && key != "RotationZ")
                {
                    result.Add(key, dictionary[key]);
                }
            }
#pragma warning disable CS8604 // Possible null reference argument.
            Matrix3D localToParent = (dictionary as IDictionary).GetLocalToParent();
#pragma warning restore CS8604 // Possible null reference argument.
            Vector3D ots = localToParent.GetRotationsOTS();
            (result as IDictionary).SetRotationsOTS(ots);
            /*
            if (Abs(ots.X) > 0.01)
            {
                result["Orientation"] = $"{Round(ots.X, 4)} deg";
            }
            if (Abs(ots.Y) > 0.01)
            {
                result["Tilt"] = $"{Round(ots.Y, 4)} deg";
            }
            if (Abs(ots.Z) > 0.01)
            {
                result["Spin"] = $"{Round(ots.Z, 4)} deg";
            }
            */
            return result;
        }

        public static Vector3D GetWorldRotationsOTS(this IDictionary dictionary)
        {
            return dictionary.GetLocalToWorld().GetRotationsOTS();
        }

        public static IDictionary<string, string> GetLocalToWorldTransforms(this IDictionary dictionary, string type)
        {
            Dictionary<string, string>? results = new Dictionary<string, string>();
            IList<object>? items = dictionary.Collect(type);
            foreach (object? item in items)
            {
                if (item is IDictionary idictionary)
                {
                    string? name = idictionary.GetName();
                    if (name.Length > 0 && !results.ContainsKey(name))
                    {
                        results.Add(name,
                            String.Join(",", idictionary.GetLocalToWorld().GetValues(4)));
                    }
                }
            }
            return results;
        }

        /*
        public static IDictionary<string, string> GetLocalToWorldTransforms(this IDictionary idictionary, string type)
        {
            var items = idictionary.Collect(type);
            var results = new Dictionary<string, string>();
            foreach (var item in items)
            {
                if (item is IDictionary dictionary)
                {
                    var name = dictionary.GetName();
                    if (name.Length > 0 && !results.ContainsKey(name))
                    {
                        results.Add(name, dictionary.GetLocalToWorld().ToString());
                    }
                }
            }
            return results;
        }*/
    }
}