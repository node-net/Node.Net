using System;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Factory.Factories
{
    public class StreamSourceFactory : IFactory
    {
        public Func<Stream, object> ReadFunction;
        public IFactory Factory { get; set; }
        public object Create(Type type, object value)
        {
            return Create(type, value as Stream);
        }
        private object Create(Type type, Stream stream)
        {
            if (stream == null) return null;
            if (ReadFunction != null)
            {
                var item = ReadFunction(stream);
                if (item != null && type.IsAssignableFrom(item.GetType()))
                {
                    return item;
                }
            }
            else
            {
                var item = XamlReader.Load(stream);
                if (item != null)
                {
                    if (type.IsAssignableFrom(item.GetType())) return item;
                    else
                    {
                        if (Factory != null)
                        {
                            item = Factory.Create(type, item);
                            if (item != null && type.IsAssignableFrom(item.GetType())) return item;
                        }
                    }
                }
            }
            return null;

        }
    }
}
