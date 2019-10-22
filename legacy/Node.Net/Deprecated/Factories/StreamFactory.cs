using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Factories
{
    class StreamFactory : IFactory
    {
        public List<Assembly> ResourceAssemblies { get; set; } = new List<Assembly>();
        public bool ExactMatch { get; set; } = false;
        public object Create(Type target_type, object source)
        {
            if (target_type == typeof(Stream))
            {
                if (source != null)
                {
                    if (source.GetType() == typeof(string)) return Create(source.ToString());
                }
            }
            return null;
        }

        public Stream Create(string name)
        {
            if (File.Exists(name)) return new FileStream(name, FileMode.Open);
            foreach (var assembly in ResourceAssemblies)
            {
                foreach (var manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (manifestResourceName == name) return assembly.GetManifestResourceStream(manifestResourceName);
                    if (!ExactMatch && manifestResourceName.Contains(name)) return assembly.GetManifestResourceStream(manifestResourceName);
                }
            }
            return null;
        }
    }
}
