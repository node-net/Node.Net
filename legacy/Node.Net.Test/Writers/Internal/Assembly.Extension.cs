using System;
using System.Collections.Generic;
using System.Reflection;

namespace Node.Net.Readers
{
    public static class AssemblyExtension
    {
        public static Dictionary<string, Type> GetNameTypeDictionary(Assembly assembly)
        {
            var types = new Dictionary<string, Type>();
            foreach (var type in assembly.GetTypes())
            {
                if (!types.ContainsKey(type.Name)) types.Add(type.Name, type);
            }
            return types;
        }

        public static Dictionary<string, Type> GetFullNameTypeDictionary(Assembly assembly)
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
