using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Factories.Deprecated.Factories.TypeSourceFactories
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
        public bool CacheReadItems = true;
        private Dictionary<string, dynamic> readCache = new Dictionary<string, dynamic>();
        public override object Create(string source)
        {
            if(GetFunction != null)
            {
                var value = GetFunction(source);
                if (value != null) return value;
            }
            if (readCache.ContainsKey(source)) return readCache[source];
            var stream = streamFromString.Create<Stream>(source,null);
            if(stream != null)
            {
                var item =  objectFromStream.Create<object>(stream,null);
                if(item != null && CacheReadItems)
                {
                    readCache.Add(source, item);
                }
                return item;
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
                            if (readCache.ContainsKey(source))
                            {
                                var instance = readCache[source];
                                if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;
                                //return readCache[source];
                            }
                            var stream = assembly.GetManifestResourceStream(resource_name);
                            if (stream != null && targetType.IsAssignableFrom(stream.GetType())) return stream;
                            try
                            {
                                var instance = objectFromStream.Create<object>(stream,null);
                                if (instance != null && targetType.IsAssignableFrom(instance.GetType()))
                                {
                                    readCache.Add(source, instance);
                                    return instance;
                                }
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
