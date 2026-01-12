namespace Node.Net
{
    public static class IFactoryExtension
    {
        public static T Create<T>(this IFactory factory) => Create<T>(factory, null);

        public static T Create<T>(this IFactory factory, object? source)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            object? instance = factory.Create(typeof(T), source);
#pragma warning restore CS8604 // Possible null reference argument.
            if (instance != null && instance is T)
            {
                return (T)instance;
            }
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}