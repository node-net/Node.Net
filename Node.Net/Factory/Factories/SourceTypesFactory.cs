using System;
using System.Collections.Generic;

namespace Node.Net.Factory.Factories
{
    class SourceTypesFactory : IFactory
    {
        public Dictionary<Type, IFactory> SourceTypeFactories = new Dictionary<Type, IFactory>();
        public IFactory Factory { get; set; }
        public object Create(Type targetType, object source)
        {
            if (source == null) return null;
            foreach (var type in SourceTypeFactories.Keys)
            {
                if (type.IsAssignableFrom(source.GetType()))
                {
                    var item = SourceTypeFactories[type].Create(targetType, source);
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
