using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public static class IDictionaryExtension
    {
        public static T Get<T>(IDictionary dictionary, string name)
        {
            if (!dictionary.Contains(name)) return default(T);
            var value = dictionary[name];
            if (value == null) return default(T);

            if (typeof(T) == typeof(DateTime) && value.GetType()== typeof(string))
            {
                return (T)((object)DateTime.Parse(value.ToString()));
            }
            return (T)dictionary[name];

        }

        public static void Set(IDictionary dictionary,string key,object value)
        {
            if (value != null && value.GetType() == typeof(DateTime))
            {
                dictionary[key] = ((DateTime)value).ToString("o");
            }
            else { dictionary[key] = value; }
        }

        public static Dictionary<string,T> Collect<T>(IDictionary dictionary,IFilter filter = null)
        {
            var children = new Dictionary<string, T>();
            if (dictionary != null)
            {
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
                                if (filter == null || filter.Include(instance))
                                {
                                    children.Add(key.ToString(), instance);
                                }
                            }
                        }
                    }
                }
            }
            return children;
        }

        public static Dictionary<string, T> DeepCollect<T>(IDictionary dictionary,IFilter filter = null)
        {
            var children = new Dictionary<string, T>();
            if (dictionary != null)
            {
                foreach (var child_key in dictionary.Keys)
                {
                    var child = dictionary[child_key];
                    if (child != null)
                    {
                        if (typeof(T).IsAssignableFrom(child.GetType()))
                        {
                            var instance = (T)child;
                            if (instance != null)
                            {
                                if (filter == null || filter.Include(instance))
                                {
                                    children.Add(child_key.ToString(), instance);
                                }
                            }
                        }

                        var deep_children = DeepCollect<T>(child as IDictionary,filter);
                        foreach (var deep_child_key in deep_children.Keys)
                        {
                            var deep_child = deep_children[deep_child_key];
                            children.Add($"{child_key}/{deep_child_key}", deep_child);
                        }
                    }
                }
            }
            return children;
        }

        public static string[] CollectUniqueStrings(IDictionary dictionary,string key)
        {
            var results = new List<string>();
            if(dictionary.Contains(key))
            {
                var value = dictionary[key];
                if (value != null)
                {
                    if (!results.Contains(value.ToString()))
                    {
                        results.Add(value.ToString());
                    }
                }
            }
            foreach(var child_key in dictionary.Keys)
            {
                var child_dictionary = dictionary[child_key] as IDictionary;
                if(child_dictionary != null)
                {
                    foreach(var value in CollectUniqueStrings(child_dictionary,key))
                    {
                        if (!results.Contains(value)) results.Add(value);
                    }
                }
            }
            return results.ToArray();
        }
    }
}
