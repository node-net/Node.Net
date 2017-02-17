using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    public static class AssemblyExtension
    {
        public static Stream GetStream(this Assembly assembly, string name)
        {
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name)) return assembly.GetManifestResourceStream(resourceName);
            }
            return null;
        }

        public static string[] GetManifestResourceNames(this Assembly assembly,string name)
        {
            var results = new List<string>();
            foreach (string resource in assembly.GetManifestResourceNames())
            {
                if (resource.Contains(name)) results.Add(resource);
            }
            return results.ToArray();
        }

        public static Dictionary<string, Type> GetNameTypeDictionary(this Assembly assembly)
        {
            var types = new Dictionary<string, Type>();
            foreach (var type in assembly.GetTypes())
            {
                if (!types.ContainsKey(type.Name)) types.Add(type.Name, type);
            }
            return types;
        }

        public static Dictionary<string, Type> GetFullNameTypeDictionary(this Assembly assembly)
        {
            var types = new Dictionary<string, Type>();
            foreach (var type in assembly.GetTypes())
            {
                if (!types.ContainsKey(type.FullName)) types.Add(type.FullName, type);
            }
            return types;
        }
    }
}
