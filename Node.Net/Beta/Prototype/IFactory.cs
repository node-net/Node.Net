using System;

namespace Node.Net.Factories.Prototype
{
    public interface IFactory
    {
        object Create(Type targetType, object source);
    }
}
