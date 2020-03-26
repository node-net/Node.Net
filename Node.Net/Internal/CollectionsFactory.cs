using System;
using System.Collections;
using System.IO;

namespace Node.Net.Internal
{
    internal sealed class CollectionsFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null && source is string)
            {
                Stream? stream = StreamFactory.Create(source.ToString());
                if (stream != null)
                {
                    object? instance = Create(targetType, stream);
                    if (instance is IDictionary idictionary)
                    {
                        idictionary.SetFileName(source.ToString());
                    }

                    return instance;
                }
            }
            if (targetType == typeof(IDictionary) && source is Stream s)
            {
                return JSONReader.Read(s) as IDictionary;
            }
            if (targetType == typeof(IList) && source is Stream s2)
            {
                return JSONReader.Read(s2) as IList;
            }
            return null;
        }

        public StreamFactory StreamFactory { get; set; } = new StreamFactory();
        private readonly Internal.JsonReader JSONReader = new Internal.JsonReader();
    }
}