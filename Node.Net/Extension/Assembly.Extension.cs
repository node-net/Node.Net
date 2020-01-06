using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    /// <summary>
    /// Extension Methods for System.Reflection.Assembly
    /// </summary>
    public static class AssemblyExtension
    {
        /// <summary>
        /// Find a ManifestResourceStream in an Assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Stream? FindManifestResourceStream(this Assembly assembly, string name)
        {
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName == name)
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name))
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets a manifest resource stream from an assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Stream? GetStream(this Assembly assembly, string name)
        {
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName.Contains(name))
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            return null;
        }

        public static Dictionary<string, byte[]> GetManifestResourceData(this Assembly assembly, string prefix)
        {
            var data = new Dictionary<string, byte[]>();
            foreach (var resource_name in assembly.GetManifestResourceNames())
            {
                if (resource_name.Contains(prefix))
                {
                    var name = resource_name.Replace(prefix, string.Empty);
                    using var memory = new MemoryStream();
                    var stream = assembly.GetManifestResourceStream(resource_name);
                    if (stream != null)
                    {
                        stream.CopyTo(memory);
                        data.Add(name, memory.ToArray());
                    }
                }
            }
            return data;
        }
    }
}