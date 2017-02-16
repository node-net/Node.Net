namespace Node.Net.Factories.Prototype.Internal
{
    static class IFactoryExtension
    {
        public static T Create<T>(this IFactory factory) => Create<T>(factory, null);
        public static T Create<T>(this IFactory factory, object source)
        {
            var instance = factory.Create(typeof(T), source);
            if (instance != null) return (T)instance;
            return default(T);
        }
    }
}
