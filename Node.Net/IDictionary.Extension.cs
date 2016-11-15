using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class IDictionaryExtension
    {
        public static string[] CollectKeys(this IDictionary dictionary) => Collections.IDictionaryExtension.CollectKeys(dictionary);
        public static T Get<T>(this IDictionary dictionary, string name) => Collections.IDictionaryExtension.Get<T>(dictionary, name);
        public static void Set(this IDictionary dictionary, string name, object value) => Collections.IDictionaryExtension.Set(dictionary, name, value);
        public static void RemoveKeys(this IDictionary dictionary, string[] keys) => Collections.IDictionaryExtension.RemoveKeys(dictionary, keys);

        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary, Func<object, bool?> filter = null) => Collections.IDictionaryExtension.Collect<T>(dictionary, filter);
        //public static Dictionary<string, T> DeepCollect<T>(this IDictionary dictionary, Func<object, bool?> filter = null, Func<object, bool?> deepFilter = null)
          //  => Collections.IDictionaryExtension.DeepCollect<T>(dictionary, filter, deepFilter);

        public static void DeepUpdateParents(this IDictionary dictionary) => Collections.IDictionaryExtension.DeepUpdateParents(dictionary);
        public static void Remove<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.Remove<T>(dictionary);
        public static void DeepRemove<T>(this IDictionary dictionary) => Collections.IDictionaryExtension.DeepRemove<T>(dictionary);
        //public static string[] CollectUniqueStrings(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.CollectUniqueStrings(dictionary, key);

        public static void Copy(this IDictionary destination, IDictionary source) => Collections.IDictionaryExtension.Copy(destination, source);
        public static T Find<T>(this IDictionary dictionary, string key) => Collections.IDictionaryExtension.Find<T>(dictionary, key);
        public static Matrix3D GetLocalToParent(this IDictionary dictionary) => Factories.Extension.IDictionaryExtension.GetLocalToParent(dictionary);
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary) => Factories.Extension.IDictionaryExtension.GetLocalToWorld(dictionary);
    }
}
