using System;
using System.Collections;
using System.IO;

namespace Node.Net
{
	/// <summary>
	/// Extension methods for System.Object
	/// </summary>
	public static class ObjectExtension
	{
		/// <summary>
		/// Get the Name of an Object
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static string GetName(this object instance)
		{
			var fullName = GetFullName(instance);
			if (fullName.Contains("/"))
			{
				var parts = fullName.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				return parts[parts.Length - 1];
			}
			if (fullName.Length == 0)
			{
				var filename = instance.GetFileName();
				if (filename != null && filename.Length > 0)
				{
					try
					{
						var fi = new FileInfo(filename);
						return fi.Name;
					}
					catch (Exception exception)
					{
						throw new Exception($"filename:{filename}", exception);
					}
				}
			}
			if (fullName.Length == 0)
			{
				var dictionary = instance as IDictionary;
				var parent = dictionary.GetParent() as IDictionary;
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
			}
			return fullName;
		}

		/// <summary>
		/// Set the Name of an Object
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		public static void SetName(this object instance, string name)
		{
			Internal.MetaData.Default.SetMetaData(instance, "Name", name);
		}

		/// <summary>
		/// Get the parent of an Object
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static object GetParent(this object instance)
		{
			var parent = Internal.MetaData.Default.GetMetaData<object>(instance, "Parent");
			if (instance.HasPropertyValue("Parent"))
			{
				return instance.GetPropertyValue("Parent");
			}
			return parent;
		}

		/// <summary>
		/// Set the parent of an object
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="parent"></param>
		public static void SetParent(this object instance, object parent)
		{
			if (instance.HasPropertyValue("Parent"))
			{
				instance.SetPropertyValue("Parent", parent);
			}
			else
			{
				Internal.MetaData.Default.GetMetaData(instance)["Parent"] = parent;
				//Internal.MetaData.Default.SetMetaData(instance, "Parent", parent);
			}
		}

		/// <summary>
		/// Check if IDictionary has a specific Property
		/// </summary>
		/// <param name="item"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static bool HasPropertyValue(this object item, string propertyName)
		{
			if (item != null)
			{
				var propertyInfo = item.GetType().GetProperty(propertyName);
				if (propertyInfo != null)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Get the value of a specific Property
		/// </summary>
		/// <param name="item"></param>
		/// <param name="propertyName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static object GetPropertyValue(this object item, string propertyName, object defaultValue = null)
		{
			if (item != null)
			{
				var propertyInfo = item.GetType().GetProperty(propertyName);
				if (propertyInfo != null)
				{
					return propertyInfo.GetValue(item);
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Get the value of a specific Property
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="item"></param>
		/// <param name="propertyName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T GetPropertyValue<T>(this object item, string propertyName, T defaultValue = default(T))
		{
			if (item != null)
			{
				var propertyInfo = item.GetType().GetProperty(propertyName);
				if (propertyInfo != null)
				{
					return (T)propertyInfo.GetValue(item);
				}
			}
			return defaultValue;
		}

		/// <summary>
		/// Set the value of a specific Property
		/// </summary>
		/// <param name="item"></param>
		/// <param name="propertyName"></param>
		/// <param name="propertyValue"></param>
		public static void SetPropertyValue(this object item, string propertyName, object propertyValue)
		{
			if (item != null)
			{
				var propertyInfo = item.GetType().GetProperty(propertyName);
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(item, propertyValue);
				}
			}
		}

		/// <summary>
		/// Get the FileName of an Object
		/// </summary>
		/// <param name="instance"></param>
		/// <returns></returns>
		public static string GetFileName(this object instance)
		{
			return Internal.MetaData.Default.GetMetaData<string>(instance, "FileName");
		}

		/// <summary>
		/// Set the FileName of an Object
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="name"></param>
		public static void SetFileName(this object instance, string name)
		{
			Internal.MetaData.Default.SetMetaData(instance, "FileName", name);
		}

		public static void SetFullName(this object instance, string fullname)
		{
			Internal.MetaData.Default.GetMetaData(instance)["FullName"] = fullname;
		}

		public static string GetFullName(this object instance)
		{
			var metaData = Internal.MetaData.Default.GetMetaData(instance);
			if (metaData != null && metaData.Contains("FullName")) return metaData["FullName"].ToString();
			return string.Empty;
		}

		public static void SetMetaData(this object instance, string name, object value)
		{
			var idictionary = Internal.MetaData.Default.GetMetaData(instance)[name] = value;
		}

		public static T GetMetaData<T>(this object instance, string name)
		{
			return Internal.MetaData.Default.GetMetaData<T>(instance, name);
		}

		public static void ClearMetaData(this object instance)
		{
			Internal.MetaData.Default.ClearMetaData(instance);
		}

		public static void CleanMetaData()
		{
			Internal.MetaData.Default.Clean();
		}
	}
}