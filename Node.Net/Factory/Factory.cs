using System;
using System.Collections.Generic;

namespace Node.Net.Factory
{
    public class Factory : IFactory
    {
        public static Factory Default { get; set; } = new Factories.DefaultFactory();
        public Factory() { }

        public List<IFactory> Factories = new List<IFactory>();
        public object Create(Type targetType, object value)
        {
            foreach (var factory in Factories)
            {
                var result = factory.Create(targetType, value);
                if (result != null)
                {
                    if (targetType.IsAssignableFrom(result.GetType()))
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
