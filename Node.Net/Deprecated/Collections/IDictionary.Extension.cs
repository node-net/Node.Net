using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Collections
{
    public static class IDictionaryExtension
    {
        public static IList Collect(this IDictionary dictionary, string typeName) => new Internal.Collector().Collect(dictionary, typeName);
        public static IList Collect(this IDictionary dictionary, Type type) => new Internal.Collector().Collect(dictionary, type);
        public static Dictionary<string, T> Collect<T>(this IDictionary dictionary,
                                                       Func<object, bool?> valueFilter = null,
                                                       Func<object, bool?> keyFilter = null, 
                                                       Func<object,bool?> deepFilter = null, 
                                                       bool deep = true)
        {
            return new Internal.Collector { KeyFilter = keyFilter, ValueFilter = valueFilter, DeepFilter = deepFilter }.Collect<T>(dictionary,deep);
        }
        public static Dictionary<string, dynamic> Collect(this IDictionary dictionary,
                                                       Func<object, bool?> valueFilter = null,
                                                       Func<object, bool?> keyFilter = null,
                                                       Func<object, bool?> deepFilter = null,
                                                       bool deep = true)
        {
            return new Internal.Collector { KeyFilter = keyFilter, ValueFilter = valueFilter, DeepFilter = deepFilter }.Collect<dynamic>(dictionary, deep);
        }
        public static string[] CollectKeys(this IDictionary dictionary) => Internal.Collector.CollectKeys(dictionary);
        public static T[] CollectValues<T>(this IDictionary dictionary, string key) => Internal.Collector.CollectValues<T>(dictionary, key);
        public static Type[] CollectTypes(this IDictionary dictionary) => Internal.Collector.CollectTypes(dictionary);


        
        public static Matrix3D GetLocalToParent(this IDictionary dictionary)
        {
            if (GlobalFunctions.GetLocalToParentFunction != null) return GlobalFunctions.GetLocalToParentFunction(dictionary);
            return new Matrix3D();
        }
        public static Matrix3D GetLocalToWorld(this IDictionary dictionary)
        {
            if (GlobalFunctions.GetLocalToWorldFunction != null) return GlobalFunctions.GetLocalToWorldFunction(dictionary);
            return new Matrix3D();
        }
        public static Rect3D GetBounds(this IDictionary dictionary)
        {
            if (GlobalFunctions.GetBoundsFunction != null) return GlobalFunctions.GetBoundsFunction(dictionary);
            return new Rect3D();
        }
        

        public static void RemoveKey(this IDictionary dictionary,string key)
        {
            if(key.Contains("/"))
            {

            }
            else
            {
                dictionary.Remove(key);
            }
        }
        public static void RemoveKeys(this IDictionary dictionary,string[] keys)
        {
            foreach (var key in keys) { RemoveKey(dictionary, key); }
        }

       
        public static void Remove<T>(this IDictionary dictionary)
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

        public static void DeepRemove<T>(this IDictionary dictionary)
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
        public static void DeepUpdateParents(this IDictionary dictionary)
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

        public static T Get<T>(this IDictionary dictionary, string name)
        {
            if (!dictionary.Contains(name))
            {
                var value = GetValue(dictionary, name);
                if(value != null && typeof(T).IsAssignableFrom(value.GetType()))
                {
                    return (T)value;
                }
                if (typeof(string) == typeof(T)) return (T)(object)string.Empty;
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
        private static object GetValue(this IDictionary dictionary,string key)
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

        public static void Set(this IDictionary dictionary, string key, object value)
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
                    if (item != null)
                    {
                        if (item.GetParent() != dictionary)
                        {
                            item.SetParent(dictionary);
                        }
                        if((item as IDictionary) != null) items.Add(item);
                        else items.Add(new Item { Key = key, Value = item });
                        //items.Add(item);
                    }
                    else
                    {
                        //items.Add(new Item { Key = key, Value = item });
                    }
                }
            }
            return items.ToArray();
        }

        public static object ConvertTypes(IDictionary source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;
            if (types == null) return source;
            var copy = Activator.CreateInstance(source.GetType());// as IDictionary;
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
            var typename = GetTypeName(source, typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (targetType == null) throw new Exception($"types['{typename}'] was null");
                if (source.GetType() != targetType)
                {
                    copy = Activator.CreateInstance(targetType);// as IDictionary;
                    if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
                }
            }
            foreach (var key in source.Keys)
            {
                var copy_dictionary = copy as IDictionary;
                var copy_element = copy as IElement;
                var value = source[key];
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    if (copy_dictionary != null)
                    {
                        copy_dictionary[key] = ConvertTypes(childDictionary, types, typeKey);
                    }
                    else
                    {
                        if(copy_element != null)
                        {
                            copy_element.Set(key.ToString(), ConvertTypes(childDictionary, types, typeKey));
                        }
                    }
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        if (copy_dictionary != null)
                        {
                            copy_dictionary[key] = IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey);
                        }
                        else
                        {
                            if(copy_element != null)
                            {
                                copy_element.Set(key.ToString(), IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey));
                            }
                        }
                    }
                    else
                    {
                        if (copy_dictionary != null)
                        {
                            if (copy_dictionary.Contains(key)) copy_dictionary[key] = value;
                            else copy_dictionary.Add(key, value);
                        }
                        else
                        {
                            if(copy_element != null)
                            {
                                if (copy_element.Contains(key.ToString())) copy_element.Set(key.ToString(),value);
                                else copy_element.Set(key.ToString(), value);
                            }
                        }
                    }
                }
            }
            return copy;
        }

        private static string GetTypeName(IDictionary source, string typeKey)
        {
            if (source != null)
            {
                if (source.Contains(typeKey)) return source[typeKey].ToString();
            }
            return string.Empty;
        }
    }
}
