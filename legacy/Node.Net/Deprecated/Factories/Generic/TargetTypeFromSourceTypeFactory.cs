using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Factories.Generic
{
    public abstract class TargetTypeFromSourceTypeFactory<T,S> : IFactory, IFactoryHelper
    {
        public Type TargetType { get { return typeof(T); } }
        public IFactory Helper { get; set; }
        public object Create(Type targetType, object source)
        {
            return Create((S)source);
        }

        public abstract T Create(S source);
    }
}
