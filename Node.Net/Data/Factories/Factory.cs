namespace Node.Net.Data.Factories
{
    public class Factory : IFactory
    {
        public T Create<T>(object value)
        {
            return default(T);
        }

        private static Factory _default;
        public static Factory Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new Factory();
                }
                return _default;
            }
        }
    }
}
