using System;
using System.Windows;

namespace Node.Net.Beta.Internal.Factories
{
    class ResourceFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null && Resources.Contains(source))
            {
                var instance = Resources[source];
                if (instance != null && target_type.IsAssignableFrom(instance.GetType()))
                {
                    return instance;
                }

            }
            return null;
        }
        public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
    }
}
