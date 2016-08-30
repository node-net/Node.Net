using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Internal
{
    static class IFactoryExtension
    {
        public static T Create<T>(IFactory factory,object value)
        {
            return (T)(object)factory.Create(typeof(T), value);
        }
        public static object Create(IFactory factory,object value)
        {
            return factory.Create(null, value);
        }
    }
}
