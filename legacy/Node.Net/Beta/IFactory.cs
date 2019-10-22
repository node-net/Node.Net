using System;

namespace Node.Net.Beta
{
    public interface IFactory
    {
        object Create(Type targetType, object source);
    }
}
