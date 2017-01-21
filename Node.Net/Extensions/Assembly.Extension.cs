// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Extensions
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
    }
}
