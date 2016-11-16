namespace Node.Net.View
{
    public class KeyValuePair
    {
        public static bool IsKeyValuePair(object item)
        {
            if (!ReferenceEquals(null, item))
            {
                if (item.GetType().IsGenericType &&
                    !ReferenceEquals(null, item.GetType().GetGenericTypeDefinition()))
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
                var valueInfo = item.GetType().GetProperty("Key");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }

        public static object GetValue(object item)
        {
            if (IsKeyValuePair(item))
            {
                var valueInfo = item.GetType().GetProperty("Value");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }
    }
}
