using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Node.Net.Collections
{
    public static class IDictionaryExtension
    {
        public static Dictionary<string, T> Collect<T>(IDictionary dictionary,
                                                       Func<object, bool?> valueFilter = null,
                                                       Func<object, bool?> keyFilter = null, 
                                                       Func<object,bool?> deepFilter = null, 
                                                       bool deep = true)
        {
            return new Internal.Collector { KeyFilter = keyFilter, ValueFilter = valueFilter, DeepFilter = deepFilter }.Collect<T>(dictionary,deep);
        }
        public static Dictionary<string, IDictionary> Collect(IDictionary dictionary,
                                                       Func<object, bool?> valueFilter = null,
                                                       Func<object, bool?> keyFilter = null,
                                                       Func<object, bool?> deepFilter = null,
                                                       bool deep = true)
        {
            return new Internal.Collector { KeyFilter = keyFilter, ValueFilter = valueFilter, DeepFilter = deepFilter }.Collect<IDictionary>(dictionary, deep);
        }
        public static string[] CollectKeys(this IDictionary dictionary) => Internal.Collector.CollectKeys(dictionary);
        public static T[] CollectValues<T>(IDictionary dictionary, string key) => Internal.Collector.CollectValues<T>(dictionary, key);
        public static Type[] CollectTypes(IDictionary dictionary) => Internal.Collector.CollectTypes(dictionary);


        
        public static Matrix3D GetLocalToParent(IDictionary dictionary)
        {
            if (GlobalFunctions.GetLocalToParentFunction != null) return GlobalFunctions.GetLocalToParentFunction(dictionary);
            return new Matrix3D();
        }
        public static Matrix3D GetLocalToWorld(IDictionary dictionary)
        {
            if (GlobalFunctions.GetLocalToWorldFunction != null) return GlobalFunctions.GetLocalToWorldFunction(dictionary);
            return new Matrix3D();
        }
        public static Rect3D GetBounds(IDictionary dictionary)
        {
            if (GlobalFunctions.GetBoundsFunction != null) return GlobalFunctions.GetBoundsFunction(dictionary);
            return new Rect3D();
        }
        

        public static void RemoveKey(IDictionary dictionary,string key)
        {
            if(key.Contains("/"))
            {

            }
            else
            {
                dictionary.Remove(key);
            }
        }
        public static void RemoveKeys(IDictionary dictionary,string[] keys)
        {
            foreach (var key in keys) { RemoveKey(dictionary, key); }
        }

       
        public static void Remove<T>(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                var remove_keys = new List<string>();
                foreach (var key in dictionary.Keys)
                {
                    var child = dictionary[key];
                    if (child != null)
                    {
                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {

                            var instance = (T)child;
                            if (instance != null)
                            {
                                remove_keys.Add(key.ToString());
                            }
                        }
                    }
                }
                foreach(var remove_key in remove_keys)
                {
                    dictionary.Remove(remove_key);
                }
            }
        }

        public static void DeepRemove<T>(IDictionary dictionary)
        {
            Remove<T>(dictionary);
            foreach(var key in dictionary.Keys)
            {
                var child_dictionary = dictionary[key] as IDictionary;
                if(child_dictionary != null)
                {
                    DeepRemove<T>(child_dictionary);
                }
            }
        }
        public static void DeepUpdateParents(IDictionary dictionary)
        {
            var visited = new List<object>();
            DeepUpdateParents(visited, dictionary);
        }
        private static void DeepUpdateParents(List<object> visited,IDictionary dictionary)
        {
            if (dictionary == null) return;
            foreach (var child_key in dictionary.Keys)
            {
                var child = dictionary[child_key];
                if (child != null)
                {
                    if (!visited.Contains(child))
                    {
                        visited.Add(child);
                        ObjectExtension.SetParent(child as IDictionary, dictionary);
                        DeepUpdateParents(visited,child as IDictionary);
                    }
                }
            }
        }

        

        public static void Copy(IDictionary destination, IDictionary source)
        {
            destination.Clear();
            foreach (string key in source.Keys)
            {
                destination[key] = CopyChild(source[key]);
            }
        }

        public static object CopyChild(object instance)
        {
            var copy = instance;
            var child_dictionary = instance as IDictionary;
            var child_enumerable = instance as IEnumerable;
            if (child_dictionary != null)
            {
                var new_child_dictionary = Activator.CreateInstance(child_dictionary.GetType()) as IDictionary;
                IDictionaryExtension.Copy(new_child_dictionary, child_dictionary);
                copy = new_child_dictionary;
            }
            else if (child_enumerable != null && child_enumerable.GetType() != typeof(string))
            {
                var new_child_list = new List<dynamic>();
                Copy(new_child_list, child_enumerable);
                copy = new_child_list;
            }
            return copy;
        }

        public static void Copy(IList destination, IEnumerable source)
        {
            foreach (var child in source)
            {
                destination.Add(CopyChild(child));
            }
        }

        public static string[] Search<T>(IDictionary dictionary,string search_pattern)
        {
            var results = new List<string>();
            return results.ToArray();
        }

        public static T Find<T>(IDictionary dictionary,string key)
        {
            var items = Collect<T>(dictionary);
            foreach(var child_key in items.Keys)
            {
                if (child_key == key) return items[child_key];
            }

            foreach (var child_key in items.Keys)
            {
                if (child_key.Contains(key)) return items[child_key];
            }

            return default(T);
        }

        public static T Get<T>(IDictionary dictionary, string name)
        {
            if (!dictionary.Contains(name))
            {
                var value = GetValue(dictionary, name);
                if(value != null && typeof(T).IsAssignableFrom(value.GetType()))
                {
                    return (T)value;
                }
                return default(T);
            }
            else
            {
                var value = dictionary[name];
                if (value == null) return default(T);

                if (typeof(T) == typeof(DateTime) && value.GetType() == typeof(string))
                {
                    return (T)((object)DateTime.Parse(value.ToString()));
                }
                return (T)dictionary[name];
            }
        }
        private static object GetValue(IDictionary dictionary,string key)
        {
            if (dictionary.Contains(key)) return dictionary[key];
            if(key.Contains("/"))
            {
                var parts = key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length > 1 && dictionary.Contains(parts[0]))
                {
                    var child = dictionary[parts[0]] as IDictionary;

                    if (child != null) return GetValue(child, String.Join("/", parts, 1, parts.Length - 1));  
                }
            }
            return null;
        }

        public static void Set(IDictionary dictionary, string key, object value)
        {
            if (key.Contains("/")) SetValue(dictionary, key, value);
            else
            {
                if (value != null && value.GetType() == typeof(DateTime))
                {
                    dictionary[key] = ((DateTime)value).ToString("o");
                }
                else { dictionary[key] = value; }
            }
        }
        private static void SetValue(IDictionary dictionary,string key,object value)
        {
            if(dictionary != null)
            {
                if(key.Contains("/"))
                {
                    var parts = key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if(parts.Length > 1)
                    {
                        var child_key = parts[0];
                        var child_subkey = String.Join("/", parts, 1, parts.Length - 1);
                        IDictionary child = null;
                        if (dictionary.Contains(child_key)) child = dictionary[child_key] as IDictionary;
                        if(child == null)
                        {
                            child = new Dictionary<string, dynamic>();
                            //dictionary[parts[0]] = child;
                        }
                        SetValue(child, child_subkey,value);
                        dictionary[child_key] = child;
                    }
                }
                else Set(dictionary, key, value);
                //dictionary[key] = value;
            }
        }
        public static object[] GetItemsSource(this IDictionary dictionary)
        {
            var items = new List<object>();
            if(dictionary != null)
            {
                foreach(string key in dictionary.Keys)
                {
                    var item = dictionary[key];
                    if(item != null)
                    {
                        if(item.GetParent() != dictionary)
                        {
                            item.SetParent(dictionary);
                        }
                        items.Add(item);
                    }
                }
            }
            return items.ToArray();
        }
    }
}
