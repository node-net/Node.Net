using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Factories.Factories.TypeSourceFactories
{
    public sealed class StreamFromString : Generic.TypeSourceFactory<Stream, string>
    {
        public List<Assembly> ResourceAssemblies { get; set; } = new List<Assembly>();
        public override Stream Create(string source)
        {
            if (source == null) return null;
            if (File.Exists(source))
            {
                return File.OpenRead(source);
            }
            foreach (var assembly in ResourceAssemblies)
            {
                foreach (var resource_name in assembly.GetManifestResourceNames())
                {
                    if (resource_name == source) return assembly.GetManifestResourceStream(resource_name);
                }
            }
            /*
            if (source.Length > 3)
            {
                foreach (var assembly in ResourceAssemblies)
                {
                    foreach (var resource_name in assembly.GetManifestResourceNames())
                    {
                        if (resource_name.Contains(source)) return assembly.GetManifestResourceStream(resource_name);
                    }
                }
            }*/
            return null;
        }
    }
}
