using System;

namespace Node.Net.Factories.Factories.Generic
{
    public abstract class TypeSourceFactory<T, S> : ITargetType, ISourceType, IFactory
    {
        public Type TargetType { get { return typeof(T); } }
        public Type SourceType { get { return typeof(S); } }

        public object Create(Type type,object source)
        {
            var instance = Create((S)source);
            if(instance != null)
            {
                if (type.IsAssignableFrom(instance.GetType())) return instance;
            }
            return null;
        }

        public abstract T Create(S source);
    }
}
