using System;
using System.IO;

namespace Node.Net.Beta.Internal
{
    static class ObjectExtension
    {
        public static object GetPropertyValue(this object item, string propertyName)
        {
            if (item != null)
            {
                var propertyInfo = item.GetType().GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    return propertyInfo.GetValue(item);
                }
            }
            return null;
        }
        public static T GetPropertyValue<T>(this object item, string propertyName)
        {
            return (T)GetPropertyValue(item, propertyName);
        }
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
        public static object GetKey(this object instance) => Collections.KeyValuePair.GetKey(instance);
        public static object GetValue(this object instance) => Collections.KeyValuePair.GetValue(instance);
        public static bool IsKeyValuePair(this object instance) => Collections.KeyValuePair.IsKeyValuePair(instance);
        public static void SetFullName(this object instance, string fullname)
        {
            Internal.Collections.MetaData.Default.GetMetaData(instance)["FullName"] = fullname;
        }
        public static string GetFullName(this object instance)
        {
            var metaData = Internal.Collections.MetaData.Default.GetMetaData(instance);
            if (metaData != null && metaData.Contains("FullName")) return metaData["FullName"].ToString();
            return string.Empty;
        }
        public static string GetName(this object instance)
        {
            var fullName = GetFullName(instance);
            if(fullName.Contains("/"))
            {
                var parts = fullName.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                return parts[parts.Length - 1];
            }
            if(fullName.Length == 0)
            {
                var filename = instance.GetFileName();
                if(filename != null && filename.Length > 0)
                {
                    try
                    {
                        var fi = new FileInfo(filename);
                        return fi.Name;
                    }
                    catch(Exception exception)
                    {
                        throw new Exception($"filename:{filename}", exception);
                    }
                }
            }
            return fullName;
        }
        public static void SetFileName(this object instance, string filename) { Internal.Collections.MetaData.Default.GetMetaData(instance)["FileName"] = filename; }
        public static string GetFileName(this object instance) { return Internal.Collections.MetaData.Default.GetMetaData<string>(instance, "FileName"); }
        public static void SetMetaData(this object instance,string name,object value)
        {
            var idictionary = Internal.Collections.MetaData.Default.GetMetaData(instance)[name] = value;
        }
        public static T GetMetaData<T>(this object instance,string name)
        {
            return Internal.Collections.MetaData.Default.GetMetaData<T>(instance, name);
        }
    }
}
