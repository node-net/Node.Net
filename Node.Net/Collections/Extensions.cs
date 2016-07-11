using System.Collections;

namespace Node.Net.Collections
{
    public static class Extensions
    {
        public static T Get<T>(this IDictionary dictionary, string name) => IDictionaryExtension.Get<T>(dictionary, name);
        public static void Set(this IDictionary dictionary, string name, object value) => IDictionaryExtension.Set(dictionary, name, value);
    }
}
