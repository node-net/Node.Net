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
#pragma warning disable CS8604 // Possible null reference argument.
                Stream? stream = StreamFactory.Create(source.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                if (stream != null)
                {
                    object? instance = Create(targetType, stream);
                    if (instance is IDictionary idictionary)
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        idictionary.SetFileName(source.ToString());
#pragma warning restore CS8604 // Possible null reference argument.
                    }

                    return instance;
                }
            }
            if (targetType == typeof(IDictionary) && source is Stream s)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return JSONReader.Read(s) as IDictionary;
#pragma warning restore CS8603 // Possible null reference return.
            }
            if (targetType == typeof(IList) && source is Stream s2)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return JSONReader.Read(s2) as IList;
#pragma warning restore CS8603 // Possible null reference return.
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public StreamFactory StreamFactory { get; set; } = new StreamFactory();
        private readonly Internal.JsonReader JSONReader = new Internal.JsonReader();
    }
}