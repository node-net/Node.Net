using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Factory.Factories.TypeSourceFactories
{
    public sealed class ObjectFromString : Generic.TypeSourceFactory<object, string>
    {
        public Func<string, object> GetFunction;
        private readonly StreamFromString streamFromString = new StreamFromString();
        private readonly ObjectFromStream objectFromStream = new ObjectFromStream();
        public Func<Stream, object> ReadFunction
        {
            get { return objectFromStream.ReadFunction; }
            set { objectFromStream.ReadFunction = value; }
        }
        public List<Assembly> ResourceAssemblies
        {
            get { return streamFromString.ResourceAssemblies; }
            set { streamFromString.ResourceAssemblies = value; }
        }
        public override object Create(string source)
        {
            if(GetFunction != null)
            {
                var value = GetFunction(source);
                if (value != null) return value;
            }
            var stream = streamFromString.Create<Stream>(source);
            if(stream != null)
            {
                return objectFromStream.Create<object>(stream);
            }

           

            return null;
        }

        public object CreatePartialMatch(Type targetType,string source)
        {
            // Partial matches
            if (source.Length > 3)
            {
                foreach (var assembly in ResourceAssemblies)
                {
                    foreach (var resource_name in assembly.GetManifestResourceNames())
                    {
                        if (resource_name.Contains(source))
                        {
                            var stream = assembly.GetManifestResourceStream(resource_name);
                            try
                            {
                                var instance = objectFromStream.Create<object>(stream);
                                if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;
                            }
                            catch { }
                        }
                    }
                }
            }
            return null;
        }
    }
}
