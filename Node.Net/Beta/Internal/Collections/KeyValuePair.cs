using System.Collections.Generic;

namespace Node.Net.Beta.Internal.Collections
{
    public static class KeyValuePair
    {
        public static bool IsKeyValuePair(object item)
        {
            if (!object.ReferenceEquals(null, item))
            {
                if (item is System.Collections.DictionaryEntry) return true;
                if (item.GetType().IsGenericType && item.GetType().GetGenericTypeDefinition() == typeof(System.Collections.Generic.KeyValuePair<,>)) return true;
            }
            return false;
        }

        public static object GetKey(object item)
        {
            if (IsKeyValuePair(item))
            {
                if (item is System.Collections.DictionaryEntry)
                {
                    return ((System.Collections.DictionaryEntry)item).Key;
                }
                if (item is KeyValuePair<string, dynamic>)
                {
                    var kvp = (KeyValuePair<string, dynamic>)(item);
                    return kvp.Key;
                }
                var valueInfo = item.GetType().GetProperty("Key");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }

        public static object GetValue(object item)
        {
            if (IsKeyValuePair(item))
            {
                if (item is System.Collections.DictionaryEntry)
                {
                    return ((System.Collections.DictionaryEntry)item).Value;
                }
                if (item is KeyValuePair<string, dynamic>)
                {
                    var kvp = (KeyValuePair<string, dynamic>)(item);
                    return kvp.Value;
                }
                var valueInfo = item.GetType().GetProperty("Value");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }
    }
}
