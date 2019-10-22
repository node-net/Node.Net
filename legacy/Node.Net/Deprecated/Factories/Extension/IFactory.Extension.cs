using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Factories
{
    public static class IFactoryExtension
    {
        public static T Create<T>(this IFactory factory) => Create<T>(factory, null);
        public static T Create<T>(this IFactory factory, object source)
        {
            return (T)(factory.Create(typeof(T), source));
        }
    }
}
