namespace Node.Net.Factory
{
    public class Factory : IFactory
    {
        public T Create<T>(object value)
        {
            return default(T);
        }

        public static Factory Default { get; set; } = new Factory();
    }
}
