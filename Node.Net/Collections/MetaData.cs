using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class MetaData : System.Collections.Generic.Dictionary<object,System.Collections.IDictionary>, IMetaData
    {
        private static readonly MetaData _default = new MetaData();
        public static MetaData Default => _default;

        public MetaData()
        {

        }
        
        public object GetMetaData(object item, string key)
        {
            if (ContainsKey(item) && this[item].Contains(key))
            {
                return this[item][key];
            }
            return null;
        }

        public void SetMetaData(object item, string key, object value)
        {
            if (!ContainsKey(item))
            {
                Add(item, new Dictionary<string, dynamic>());
            }
            this[item][key] = value;
        }
        
    }
}
