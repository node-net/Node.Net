using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory
{
    public static class Extensions
    {
        public static T Create<T>(this IFactory factory, object value) => Internal.IFactoryExtension.Create<T>(factory, value);
    }
}
