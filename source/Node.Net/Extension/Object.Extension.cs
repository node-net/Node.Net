using System;
using System.Collections;
using System.Collections.Generic;
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
            string? fullName = GetFullName(instance);
            if (fullName.Contains("/"))
            {
                string[]? parts = fullName.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                return parts[parts.Length - 1];
            }
            if (fullName.Length == 0)
            {
                string? filename = instance.GetFileName();
                if (filename?.Length > 0)
                {
                    try
                    {
                        FileInfo? fi = new FileInfo(filename);
                        return fi.Name;
                    }
                    catch (Exception exception)
                    {
                        throw new InvalidOperationException($"filename:{filename}", exception);
                    }
                }
            }
            if (fullName.Length == 0)
            {
                IDictionary? dictionary = instance as IDictionary;
                if(dictionary != null)
                {
                    if (dictionary.Contains("Name"))
                    {
                        var nameValue = dictionary["Name"];
                        if (nameValue != null)
                        {
                            // Handle dynamic types properly - check for string first, then convert
                            if (nameValue is string str && !string.IsNullOrEmpty(str))
                            {
                                return str;
                            }
                            var nameStr = nameValue.ToString();
                            if (!string.IsNullOrEmpty(nameStr))
                            {
                                return nameStr;
                            }
                        }
                    }
                }
#pragma warning disable CS8604 // Possible null reference argument.
                if (dictionary.GetParent() is IDictionary parent)
#pragma warning restore CS8604 // Possible null reference argument.
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    foreach (string key in parent.Keys)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        IDictionary? test_element = parent.Get<IDictionary>(key);
#pragma warning restore CS8604 // Possible null reference argument.
                        if (test_element != null && object.ReferenceEquals(test_element, dictionary))
                        {
#pragma warning disable CS8603 // Possible null reference return.
                            return key;
#pragma warning restore CS8603 // Possible null reference return.
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
        public static object? GetParent(this object instance)
        {
            object? parent = Internal.MetaData.Default.GetMetaData<object>(instance, "Parent");
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
            if (instance is null)
            {
                return;
            }

            if (instance.HasPropertyValue("Parent"))
            {
                instance.SetPropertyValue("Parent", parent);
            }
            else
            {
                IDictionary? metaData = Internal.MetaData.Default.GetMetaData(instance);
                if (metaData != null)
                {
                    if (metaData.Contains("Parent"))
                    {
                        metaData["Parent"] = parent;
                    }
                    else
                    {
                        metaData.Add("Parent", parent);
                    }
                }
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
                System.Reflection.PropertyInfo? propertyInfo = item.GetType().GetProperty(propertyName);
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
        public static object GetPropertyValue(this object item, string propertyName, object? defaultValue = null)
        {
            if (item != null)
            {
                System.Reflection.PropertyInfo? propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return propertyInfo.GetValue(item);
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return defaultValue;
#pragma warning restore CS8603 // Possible null reference return.
        }

        /// <summary>
        /// Get the value of a specific Property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
#pragma warning disable CS8601 // Possible null reference assignment - defaultValue parameter can be null for reference types
        public static T GetPropertyValue<T>(this object item, string propertyName, T defaultValue = default)
#pragma warning restore CS8601
        {
            if (item != null)
            {
                System.Reflection.PropertyInfo? propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    return (T)propertyInfo.GetValue(item);
#pragma warning restore CS8600
#pragma warning restore CS8601
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return defaultValue;
#pragma warning restore CS8600
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
                System.Reflection.PropertyInfo? propertyInfo = item.GetType().GetProperty(propertyName);
                propertyInfo?.SetValue(item, propertyValue);
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

        public static string GetShortFileName(this object instance)
        {
            string? filename = instance.GetFileName();
            if (filename.Length > 0)
            {
                return new FileInfo(filename).Name;
            }
            return string.Empty;
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
            IDictionary? metaData = Internal.MetaData.Default.GetMetaData(instance);
            if (metaData?.Contains("FullName") == true)
            {
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return metaData["FullName"].ToString();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.
            }

            return string.Empty;
        }

        public static void SetMetaData(this object instance, string name, object value)
        {
            Internal.MetaData.Default.GetMetaData(instance)[name] = value;
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

        public static T Get<T>(this object[] items, int index)
        {
            object? item = items[index];
            if (item != null)
            {
                if (typeof(T).IsAssignableFrom(item.GetType())) return (T)item;
                if (typeof(T) == typeof(int)) return (T)(object)System.Convert.ToInt32(item);
                if (typeof(T) == typeof(long)) return (T)(object)System.Convert.ToInt64(item);
                if (typeof(T) == typeof(float)) return (T)(object)System.Convert.ToSingle(item);
                if (typeof(T) == typeof(double)) return (T)(object)System.Convert.ToDouble(item);
                if (typeof(T) == typeof(IDictionary<string, string>)) return Convert<T>(item);
            }
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public static T Convert<T>(object value)
        {
            if (typeof(T) == typeof(IDictionary<string, string>))
            {
                if (value is IDictionary dictionary)
                {
                    Dictionary<string, string>? result = new Dictionary<string, string>();
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    foreach (string key in dictionary.Keys)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    {

/* Unmerged change from project 'Node.Net (net46)'
Before:
                        result.Add(key, dictionary[key]?.ToString());
                    }
After:
#pragma warning disable CS8604 // Possible null reference argument.
                        result.Add(key, dictionary[key]?.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                    }
*/

/* Unmerged change from project 'Node.Net (net48)'
Before:
                        result.Add(key, dictionary[key]?.ToString());
                    }
After:
#pragma warning disable CS8604 // Possible null reference argument.
                        result.Add(key, dictionary[key]?.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                    }
*/
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                        result.Add(key, dictionary[key]?.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                    return (T)(object)result;
                }
            }
            return (T)value;
        }
    }
}