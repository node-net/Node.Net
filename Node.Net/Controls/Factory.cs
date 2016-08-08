namespace Node.Net.Controls
{
    public class Factory
    {
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

        public T Create<T>(object value)
        {
            return default(T);
        }
    }
}
