using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public static class Extensions
    {
        public static T Get<T>(this IDictionary dictionary, string name) => IDictionaryExtension.Get<T>(dictionary, name);
        public static void Set(this IDictionary dictionary, string name, object value) => IDictionaryExtension.Set(dictionary, name, value);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary) => IDictionaryExtension.Collect<T>(dictionary);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary,IFilter filter) => IDictionaryExtension.Collect<T>(dictionary,filter);
        public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary) => IDictionaryExtension.DeepCollect<T>(dictionary);
        public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary,IFilter filter) => IDictionaryExtension.DeepCollect<T>(dictionary,filter);
        public static string[] CollectUniqueStrings(this IDictionary dictionary, string key) => IDictionaryExtension.CollectUniqueStrings(dictionary, key);
    }
}
