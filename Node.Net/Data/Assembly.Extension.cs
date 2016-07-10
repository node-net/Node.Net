using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data
{
    public static class AssemblyExtension
    {
        public static Stream GetStream(Assembly assembly,string name)
        {
            foreach (var resource_name in assembly.GetManifestResourceNames())
            {
                if (resource_name.Contains(name)) return assembly.GetManifestResourceStream(resource_name);
            }
            return null;
        }
    }
}
