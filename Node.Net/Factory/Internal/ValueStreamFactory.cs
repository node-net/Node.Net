using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Factory.Internal
{
    class ValueStreamFactory : IFactory
    {
        public object Create(Type type,object value)
        {
            return Create(type, value as Stream);
        }
        public static object Create(Type type,Stream stream)
        {
            if (stream == null) return null;
            return XamlReader.Load(stream);
        }
    }
}
