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
            foreach (string? resourceName in assembly.GetManifestResourceNames())
            {
                if (resourceName == name)
                {
                    return assembly.GetManifestResourceStream(resourceName);
                }
            }
            foreach (string? resourceName in assembly.GetManifestResourceNames())
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
            foreach (string? resourceName in assembly.GetManifestResourceNames())
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
            Dictionary<string, byte[]>? data = new Dictionary<string, byte[]>();
            foreach (string? resource_name in assembly.GetManifestResourceNames())
            {
                if (resource_name.Contains(prefix))
                {
                    string? name = resource_name.Replace(prefix, string.Empty);
                    using MemoryStream? memory = new MemoryStream();
                    Stream? stream = assembly.GetManifestResourceStream(resource_name);
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