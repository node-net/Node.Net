using System;

namespace Node.Net.Deprecated.Factories
{
    public interface IFactory
    {
        object Create(Type targetType, object source);
    }

    public interface IFactoryHelper
    {
        IFactory Helper { get; set; }
    }

    public interface ITargetType
    {
        Type TargetType { get; }
    }
}
