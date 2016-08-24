using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Factory.Factories.TypeFactories
{
    public class StreamFactory : Generic.TypeFactory<Stream>, IFactory
    {
        public StreamFactory(Func<Stream, object> read_function, IFactory helper_factory)
        {
            streamSourceFactory = new SourceFactories.StreamSourceFactory(read_function, helper_factory);
        }
        public StreamFactory(Func<Stream, object> read_function, IFactory helper_factory, Assembly resource_assembly)
        {
            streamSourceFactory = new SourceFactories.StreamSourceFactory(read_function, helper_factory);
            ResourceAssemblies.Add(resource_assembly);
        }
        public List<Assembly> ResourceAssemblies = new List<Assembly>();
        private SourceFactories.StreamSourceFactory streamSourceFactory = null;

        public object Create(Type type, object value)
        {
            if (value == null) return null;
            if (typeof(string) == value.GetType())
            {
                var item = Create(value.ToString());
                if (type == typeof(Stream)) return item;
                return streamSourceFactory.Create(type, item);
            }
            return null;
        }

        public Stream Create(string name)
        {
            if (File.Exists(name))
            {
                return new FileStream(name, FileMode.Open);
            }
            foreach (var assembly in ResourceAssemblies)
            {
                foreach (var resource_name in assembly.GetManifestResourceNames())
                {
                    if (resource_name.Contains(name)) { return assembly.GetManifestResourceStream(resource_name); }
                }
            }
            return null;
        }
    }
}
