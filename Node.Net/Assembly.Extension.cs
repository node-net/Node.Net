using System;
using System.Collections.Generic;
using System.Reflection;

namespace Node.Net
{
    public static class AssemblyExtension
    {
        public static Dictionary<string, Type> GetNameTypeDictionary(this Assembly assembly) => Readers.AssemblyExtension.GetNameTypeDictionary(assembly);
        public static Dictionary<string, Type> GetFullNameTypeDictionary(this Assembly assembly) => Readers.AssemblyExtension.GetFullNameTypeDictionary(assembly);
    }
}
