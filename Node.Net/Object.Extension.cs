using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Node.Net
{
    public static class ObjectExtension
    {
        public static void SetParent(this object instance, object? parent)
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
                var metaData = MetaData.Default.GetMetaData(instance);
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

        public static object? GetParent(this object instance)
        {
            var parent = MetaData.Default.GetMetaData<object>(instance, "Parent");
            if (instance.HasPropertyValue("Parent"))
            {
                return instance.GetPropertyValue("Parent");
            }
            return parent;
        }

        public static string GetName(this object instance)
        {
            if (instance.HasPropertyValue("Name")) return instance.GetPropertyValue("Name")!.ToString();
            if (instance.GetParent() is IDictionary parent)
            {
                foreach (string key in parent.Keys)
                {
                    var test_element = parent.Get<IDictionary>(key);
                    if (test_element != null && object.ReferenceEquals(test_element, instance))
                    {
                        return key;
                    }
                }
            }
            return string.Empty;
        }

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

        public static object? GetPropertyValue(this object item, string propertyName, object? defaultValue = null)
        {
            var propertyInfo = item.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(item);
            }

            return defaultValue;
        }

        public static T GetPropertyValue<T>(this object item, string propertyName, T defaultValue = default)
        {
            return (T)(item.GetPropertyValue(propertyName, (object)defaultValue!))!;
        }

        public static void SetPropertyValue(this object item, string propertyName, object? propertyValue)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                propertyInfo?.SetValue(item, propertyValue);
            }
        }
    }
}
