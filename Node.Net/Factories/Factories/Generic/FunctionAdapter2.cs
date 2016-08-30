using System;

namespace Node.Net.Factories.Factories.Generic
{
    public class FunctionAdapter2<T, S> : ITargetType, ISourceType, IFactory, IFactoryAdapter
    {
        public Type TargetType { get { return typeof(T); } }
        public Type SourceType { get { return typeof(S); } }
        public IFactory Parent { get; set; }
        public string Name { get { return $"{TargetType.Name}.{SourceType.Name}"; } }
        private readonly Func<S, T> FactoryFunction;
        public FunctionAdapter2(Func<S, T> factoryFunction) { FactoryFunction = factoryFunction; }

        public object Create(Type type, object source)
        {
            var instance = FactoryFunction((S)source);
            if (instance != null)
            {
                if (type.IsAssignableFrom(instance.GetType())) return instance;
            }
            return null;
        }
    }
}
