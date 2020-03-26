namespace Node.Net
{
    public static class IFactoryExtension
    {
        public static T Create<T>(this IFactory factory) => Create<T>(factory, null);

        public static T Create<T>(this IFactory factory, object? source)
        {
            object? instance = factory.Create(typeof(T), source);
            if (instance != null && instance is T)
            {
                return (T)instance;
            }
            return default;
        }
    }
}