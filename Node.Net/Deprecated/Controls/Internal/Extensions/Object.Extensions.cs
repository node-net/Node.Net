namespace Node.Net.Deprecated.Controls.Internal.Extensions
{
    static class ObjectExtensions
    {
        public static string GetKey(object value)
        {
            return KeyValuePair.GetKey(value).ToString();
        }
        public static object GetValue(object value)
        {
            return KeyValuePair.GetValue(value);
        }
    }
}
