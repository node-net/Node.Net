using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Readers
{
    public static class AssemblyExtension
    {
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

        public static Stream GetStream(this Assembly assembly,string name)
        {
            foreach(string resource in assembly.GetManifestResourceNames())
            {
                if (resource.Contains(name)) return assembly.GetManifestResourceStream(resource);
            }
            return null;
        }
    }
}
