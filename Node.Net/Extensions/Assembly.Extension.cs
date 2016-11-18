using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }
}
