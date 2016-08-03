using System.Collections.Generic;

namespace Node.Net.Deprecated.Collections
{
    public class MetaDataManager : System.Collections.Generic.Dictionary<object,System.Collections.IDictionary>, IMetaDataManager
    {
        private static readonly MetaDataManager _default = new MetaDataManager();
        public static MetaDataManager Default => _default;

        public MetaDataManager()
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
