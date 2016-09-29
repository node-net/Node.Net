using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Collections
{
    public static class Extensions
    {
        public static T Get<T>(this IDictionary dictionary, string name) => IDictionaryExtension.Get<T>(dictionary, name);
        public static void Set(this IDictionary dictionary, string name, object value) => IDictionaryExtension.Set(dictionary, name, value);
        public static void RemoveKeys(this IDictionary dictionary, string[] keys) => IDictionaryExtension.RemoveKeys(dictionary, keys);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary) => IDictionaryExtension.Collect<T>(dictionary);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary, IFilter filter) => IDictionaryExtension.Collect<T>(dictionary, filter);
        public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary) => IDictionaryExtension.DeepCollect<T>(dictionary);
        public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary, IFilter filter) => IDictionaryExtension.DeepCollect<T>(dictionary, filter);
        public static void Remove<T>(this IDictionary dictionary) => IDictionaryExtension.Remove<T>(dictionary);
        public static void DeepRemove<T>(this IDictionary dictionary) => IDictionaryExtension.DeepRemove<T>(dictionary);
        public static string[] CollectUniqueStrings(this IDictionary dictionary, string key) => IDictionaryExtension.CollectUniqueStrings(dictionary, key);
        public static object GetParent(this IDictionary dictionary) => IDictionaryExtension.GetParent(dictionary);
        public static void Copy(this IDictionary destination, IDictionary source) => IDictionaryExtension.Copy(destination, source);
        public static T GetNearestAncestor<T>(this IDictionary child) => IDictionaryExtension.GetNearestAncestor<T>(child);
        public static T GetFurthestAncestor<T>(this IDictionary child) => IDictionaryExtension.GetFurthestAncestor<T>(child);
        public static IDictionary GetRootAncestor(this IDictionary child) => IDictionaryExtension.GetRootAncestor(child);
        public static T Find<T>(this IDictionary dictionary, string key) => IDictionaryExtension.Find<T>(dictionary, key);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => IDictionaryExtension.GetLocalToParent(dictionary);
    }
}
