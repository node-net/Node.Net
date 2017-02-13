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
            if (target_type == null) return null;
            foreach (var targetType in Keys)
            {
                var concreteType = this[targetType];
                if (targetType.IsAssignableFrom(target_type))
                {
                    return Construct(concreteType,source);
                }
            }
            return Construct(target_type,source);
        }

        public static object Construct(Type type, object value)
        {
            object[] parameters = { value };
            return Construct(type, parameters);
        }
        public static object Construct(Type type, object[] parameters = null)
        {
            Type[] types = Type.EmptyTypes;
            if (parameters != null)
            {
                var typesList = new List<Type>();
                foreach (var item in parameters)
                {
                    if (item != null)
                    {
                        typesList.Add(item.GetType());
                    }
                }
                if (typesList.Count == parameters.Length) types = typesList.ToArray();
            }
            var constructor = type.GetConstructor(types);
            if (constructor != null)
            {
                if (parameters == null || parameters.Length == 0 || parameters[0] == null) return constructor.Invoke(null);
                else return constructor.Invoke(parameters);
            }
            return null;
        }
    }
}
