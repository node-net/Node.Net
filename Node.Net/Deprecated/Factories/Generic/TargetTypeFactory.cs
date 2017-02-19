using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated.Factories.Generic
{
    public abstract class TargetTypeFactory<T> : IFactory, IFactoryHelper
    {
        public Type TargetType { get { return typeof(T); } }
        public IFactory Helper { get; set; }
        public object Create(Type targetType,object source)
        {
            if (typeof(T).IsAssignableFrom(targetType))
            {
                return Create(source);
            }
            return null;
        }

        public abstract T Create(object source);
    }
}
