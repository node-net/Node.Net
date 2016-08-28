using System;

namespace Node.Net.Factory.Factories.Generic
{
    public class FunctionAdapter3<T, S> : ITargetType, ISourceType, IFactory, IFactoryAdapter
    {
        public Type TargetType { get { return typeof(T); } }
        public Type SourceType { get { return typeof(S); } }
        public IFactory Parent { get; set; }
        public string Name { get { return $"{TargetType.Name}.{SourceType.Name}"; } }
        private readonly Func<S ,IFactory,T> FactoryFunction;
        public FunctionAdapter3(Func<S, IFactory,T> factoryFunction) { FactoryFunction = factoryFunction; }

        public object Create(Type type, object source)
        {
            var instance = FactoryFunction((S)source,this.GetRootAncestor());
            if (instance != null)
            {
                if (type.IsAssignableFrom(instance.GetType())) return instance;
            }
            return null;
        }
    }
}
