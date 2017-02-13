using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories
{
    public sealed class AbstractFactory : Dictionary<Type, Type>, IFactory
    {
        public object Create(Type target_type, object source)
        {
            foreach (var targetType in Keys)
            {
                var concreteType = this[targetType];
                if (targetType.IsAssignableFrom(target_type))
                {
                    if (source == null)
                    {
                        return Activator.CreateInstance(concreteType);
                    }
                    else
                    {
                        Type[] parameterTypes = { source.GetType() };
                        var constructor = concreteType.GetConstructor(parameterTypes);
                        if (constructor != null)
                        {
                            object[] parameters = { source };
                            return constructor.Invoke(parameters);
                        }
                    }
                }
            }
            return null;
        }
    }
}
