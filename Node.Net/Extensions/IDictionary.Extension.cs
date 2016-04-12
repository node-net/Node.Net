using System.Collections;

namespace Node.Net.Extensions
{
    public class IDictionaryExtension
    {
        public static IDictionary Find(IDictionary source, string key, string value)
        {
            if (object.ReferenceEquals(null, source)) return null;
            if (source.Contains(key) && source[key].ToString() == value)
            {
                return source;
            }
            foreach (string child_key in source.Keys)
            {
                IDictionary childDictionary = source[child_key] as IDictionary;
                if (!object.ReferenceEquals(null, childDictionary))
                {
                    IDictionary result = Find(childDictionary, key, value);
                    if (!object.ReferenceEquals(null, result)) return result;
                }
            }
            return null;
        }

    }
}
