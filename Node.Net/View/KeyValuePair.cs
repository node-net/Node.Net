namespace Node.Net.View
{
    public class KeyValuePair
    {
        public static bool IsKeyValuePair(object item)
        {
            if (!object.ReferenceEquals(null, item))
            {
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
                System.Reflection.PropertyInfo valueInfo = item.GetType().GetProperty("Key");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }

        public static object GetValue(object item)
        {
            if (IsKeyValuePair(item))
            {
                System.Reflection.PropertyInfo valueInfo = item.GetType().GetProperty("Value");
                return valueInfo.GetValue(item, null);
            }
            return item;
        }
    }
}
