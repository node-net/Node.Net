using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class KeyValuePair
    {
        public static bool IsKeyValuePair(object item)
        {
            if (!object.ReferenceEquals(null, item))
            {
                if (item.GetType() == typeof(System.Collections.DictionaryEntry)) return true;
                if (item.GetType().IsGenericType &&
                    !object.ReferenceEquals(null, item.GetType().GetGenericTypeDefinition()))
                {
                    if (item.GetType().GetGenericTypeDefinition() == typeof(System.Collections.Generic.KeyValuePair<,>))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static object GetKey(object item)
        {
            if (IsKeyValuePair(item))
            {
                if (IsKeyValuePair(item) && item.GetType() == typeof(System.Collections.DictionaryEntry))
                {
                    return ((System.Collections.DictionaryEntry)item).Key;
                }
                if (item.GetType() == typeof(KeyValuePair<string, dynamic>))
                {
                    KeyValuePair<string, dynamic> kvp = (KeyValuePair<string, dynamic>)(item);
                    return kvp.Key;
                }
                System.Reflection.PropertyInfo valueInfo = item.GetType().GetProperty("Key");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }

        public static object GetValue(object item)
        {
            if (IsKeyValuePair(item))
            {
                if (IsKeyValuePair(item) && item.GetType() == typeof(System.Collections.DictionaryEntry))
                {
                    return ((System.Collections.DictionaryEntry)item).Value;
                }
                if (item.GetType() == typeof(KeyValuePair<string, dynamic>))
                {
                    KeyValuePair<string, dynamic> kvp = (KeyValuePair<string, dynamic>)(item);
                    return kvp.Value;
                }
                System.Reflection.PropertyInfo valueInfo = item.GetType().GetProperty("Value");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }
    }
}
