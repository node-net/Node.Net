namespace Node.Net.Factory.Internal
{
    class MaterialFactory : IFactory
    {
        public T Create<T>(object value)
        {
            return default(T);
        }
    }
}
