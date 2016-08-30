using System;
using System.Collections.Generic;
using System.IO;


namespace Node.Net.Extensions
{
    public class TypeExtension
    {
        public static Stream GetStream(Type type, string name)
        {
            return StreamExtension.GetStream(name, type);
        }
        public static string[] GetManifestResourceNames(Type type, string name)
        {
            var results = new List<string>();
            foreach (string resource_name in type.Assembly.GetManifestResourceNames())
            {
                if (resource_name.Contains(name))
                {
                    results.Add(resource_name);
                }
            }
            return results.ToArray();
        }

        public static Dictionary<string, T> CollectManifestResources<T>(Type type, string pattern)
        {
            var results = new Dictionary<string, T>();
            foreach (var manifest_resource_name in type.Assembly.GetManifestResourceNames())
            {
                if (manifest_resource_name.Contains(pattern))
                {
                    var item = (T)Reader.Default.Read(type.GetStream(manifest_resource_name));
                    if (item != null)
                    {
                        results.Add(manifest_resource_name, item);
                    }
                }
            }
            return results;
        }
    }
}
