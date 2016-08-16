using System;
using System.Collections.Generic;

namespace Node.Net.Factory.Factories
{
    public class TargetTypesFactory : IFactory
    {
        public Dictionary<Type, IFactory> TargetTypeFactories = new Dictionary<Type, IFactory>();
        public IFactory Factory { get; set; }
        public object Create(Type targetType, object source)
        {
            foreach (var type in TargetTypeFactories.Keys)
            {
                if (type.IsAssignableFrom(targetType))
                {
                    var item = TargetTypeFactories[type].Create(targetType, source);
                    if (item != null && targetType.IsAssignableFrom(item.GetType()))
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
