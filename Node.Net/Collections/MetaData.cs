namespace Node.Net.Collections
{
    public class MetaData : System.Collections.Generic.Dictionary<object,System.Collections.IDictionary>
    {
        private static MetaData _default = new MetaData();
        public static MetaData Default => _default;

        /*
        public Dictionary<object, Dictionary<string, dynamic>> MetaData = new Dictionary<object, Dictionary<string, dynamic>>();
        public dynamic GetMetaData(object item, string key)
        {
            if (MetaData.ContainsKey(item) && MetaData[item].ContainsKey(key))
            {
                return MetaData[item][key];
            }
            return null;
        }

        public void SetMetaData(object item, string key, object value)
        {
            if (!MetaData.ContainsKey(item))
            {
                MetaData.Add(item, new Dictionary<string, dynamic>());
            }
            MetaData[item][key] = value;
        }
        */
    }
}
