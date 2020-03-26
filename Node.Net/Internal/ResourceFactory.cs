using System;
using System.Windows;

namespace Node.Net.Internal
{
    internal class ResourceFactory : IFactory
    {
        public object Create(Type targetType, object source)
        {
            if (source != null && Resources.Contains(source))
            {
                object? instance = Resources[source];
                if (instance != null && targetType.IsInstanceOfType(instance))
                {
                    return instance;
                }
            }
            return null;
        }

        public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
    }
}