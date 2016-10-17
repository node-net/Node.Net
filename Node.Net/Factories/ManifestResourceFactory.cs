using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Factories
{
    public sealed class ManifestResourceFactory : IFactory
    {
        public Func<Stream, object> ReadFunction { get; set; } = DefaultRead;
        public bool RequireExactMatch { get; set; } = false;
        public object Create(Type targetType, object source)
        {
            if(source != null)
            {
                if (source.GetType() == typeof(string)) return Create(targetType,source.ToString());
            }
            return null;
        }

        public List<string> ManifestResourceNameIgnorePatterns { get; set; } = new List<string>();
        public List<Assembly> Assemblies { get; set; } = new List<Assembly>();
        public Dictionary<string, object> Cache = new Dictionary<string, object>();
        private object Create(Type targetType,string name)
        {
            if(Cache.ContainsKey(name))
            {
                var instance = Cache[name];
                if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;
            }
            if (Assemblies != null)
            {
                foreach (var assembly in Assemblies)
                {
                    foreach (var manifestResourceName in assembly.GetManifestResourceNames())
                    {
                        bool ignore = false;
                        if(ManifestResourceNameIgnorePatterns != null)
                        {
                            foreach(var ignore_pattern in ManifestResourceNameIgnorePatterns)
                            {
                                if (manifestResourceName.Contains(ignore_pattern)) ignore = true;
                            }
                        }
                        if (!ignore)
                        {
                            if (manifestResourceName == name || (manifestResourceName.Contains(name) && !RequireExactMatch))
                            {
                                object instance = null;
                                try
                                {
                                    instance = ReadFunction(assembly.GetManifestResourceStream(manifestResourceName));
                                }
                                catch (Exception e)
                                {
                                    throw new Exception($"ReadFunction failed on ManifestResourceName '{manifestResourceName}'", e);
                                }
                                if (instance != null)
                                {
                                    if (targetType.IsAssignableFrom(instance.GetType()))
                                    {
                                        Cache[name] = instance;
                                        return instance;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static object DefaultRead(Stream stream)
        {
            return XamlReader.Load(stream);
        }
    }
}
