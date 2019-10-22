using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Deprecated.Internal
{
    static class IFactoryExtension
    {
        public static T Create<T>(IFactory factory,object value,IFactory helper)
        {
            return (T)(object)factory.Create(typeof(T), value,helper);
        }
        public static object Create(IFactory factory,object value,IFactory helper)
        {
            return factory.Create(null, value,helper);
        }
    }
}
