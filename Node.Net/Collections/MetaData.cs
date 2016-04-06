namespace Node.Net.Collections
{
    public class MetaData : System.Collections.Generic.Dictionary<object,System.Collections.IDictionary>
    {
        private static MetaData _default = new MetaData();
        public static MetaData Default => _default;
    }
}
