using System;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Factories.Factories.TypeSourceFactories
{
    public sealed class ObjectFromStream : Generic.TypeSourceFactory<object, Stream>
    {
        public Func<Stream,object> ReadFunction;
        public override object Create(Stream source)
        {
            if (ReadFunction != null) return ReadFunction(source);
            if(source != null)
            {
                return XamlReader.Load(source);
            }
            return null;
        }
    }
}
