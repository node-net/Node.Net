using System;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Factory.Factories.SourceFactories
{
    public class StreamSourceFactory : IFactory
    {
        private Func<Stream, object> ReadFunction;
        private IFactory HelperFactory;
        public StreamSourceFactory(Func<Stream,object> read_function,IFactory helper_factory)
        {
            ReadFunction = read_function;
            HelperFactory = helper_factory;
        }

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
                if (item != null)
                {
                    if(type.IsAssignableFrom(item.GetType())) return item;
                }
                if (HelperFactory != null)
                {
                    item = HelperFactory.Create(type, item);
                    if (item != null && type.IsAssignableFrom(item.GetType())) return item;
                }
            }
            return null;

        }

        public static object DefaultReadFunction(Stream stream)
        {
            return XamlReader.Load(stream);
        }
    }
}
