using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net
{
    public static class IDictionaryExtension
    {
        public static T Get<T>(this IDictionary dictionary, string name, T defaultValue = default)
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
                var value = dictionary[name];
                if(typeof(T) == typeof(double))
                {
                    return (T)(object)Convert.ToDouble(value);
                }

                if(value != null && typeof(T).IsAssignableFrom(value.GetType()))
                {
                    return (T)value;
                }
                return default(T);
            }

            if (typeof(T) == typeof(string) && EqualityComparer<T>.Default.Equals(defaultValue, default!))
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

        public static Math.Matrix3D GetLocalToParent(this IDictionary dictionary)
        {
            return IDictionarySpatialConverter.Default.GetLocalToParent(dictionary);
        }

        public static void DeepUpdateParents(this IDictionary dictionary)
        {
            if (dictionary is null)
            {
                return;
            }

            var values = new List<object>();
            foreach (var value in dictionary.Values) { values.Add(value); }
            foreach (var value in values)
            {
                if (value is IDictionary child)
                {
                    child.SetParent(dictionary);
                    DeepUpdateParents(child);
                }
            }
        }
    }
}
