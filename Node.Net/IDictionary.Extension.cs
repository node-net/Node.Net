using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net
{
    public static class IDictionaryExtension
    {
        public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default(T))
        {
            if (name.IndexOf(',') > -1)
            {
                var names = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach(var subname in names)
                {
                    if (dictionary.Contains(subname)) { return dictionary.Get<T>(subname); }
                }
            }
            if (dictionary.Contains(name))
            {
                return (T)dictionary[name];
            }

            if (typeof(T) == typeof(string) && EqualityComparer<T>.Default.Equals(defaultValue, default(T)!))
            {
                return (T)(object)string.Empty;
            }

            return defaultValue;
        }

        public static List<T> Collect<T>(this IDictionary idictionary)
        {
            var results = new List<T>();
            foreach (var item in idictionary.Values)
            {
                if (item != null && item is T && !results.Contains((T)item))
                {
                    results.Add((T)item);
                }
                if (item is IDictionary child_idictionary)
                {
                    foreach(var childItem in child_idictionary.Collect<T>())
                    {
                        results.Add(childItem);
                    }
                }
            }
            return results;
        }
    }
}
