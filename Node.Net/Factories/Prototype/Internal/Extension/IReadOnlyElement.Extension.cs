using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Prototype
{
    public static class IReadOnlyElementExtension
    {
        public static int GetDeepCount(this Collections.IReadOnlyElement element)
        {
            var count = element.Count;
            foreach (string key in element.Keys)
            {
                var child_element = element.Get(key) as Collections.IElement;
                if (child_element != null)
                {
                    count += child_element.GetDeepCount();
                }
            }
            return count;
        }

        public static string GetJSON(this Internal.Collections.IReadOnlyElement element)
        {
            var memory = new MemoryStream();
            //{
                new Writers.JSONWriter { Format = Writers.JSONFormat.Indented }.Write(memory, element);
                memory.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(memory))
                {
                    return sr.ReadToEnd();
                }
           // }
        }
        public static T Get<T>(this Collections.IReadOnlyElement element, string name)
        {
            if (element.Contains(name))
            {
                var value = element.Get(name);
                if (value != null && typeof(T).IsAssignableFrom(value.GetType()))
                {
                    return (T)value;
                }

            }
            if (typeof(string) == typeof(T)) return (T)(object)string.Empty;
            return default(T);
        }
        /*
        public static bool Matches(this Collections.IReadOnlyElement element, string pattern = "")
        {
            if (pattern.Length == 0) return true;
            if (element.GetFullName().Contains(pattern)) return true;
            foreach (string key in element.Keys)
            {
                var string_value = element.Get<string>(key);
                if (string_value.Length > 0)
                {
                    if (string_value.Contains(pattern)) return true;
                }
            }
            return false;
        }*/
        /*
        public static IEnumerable Find(this Collections.IReadOnlyElement element, Type target_type, string pattern = "") { return _Find(element, target_type, new List<object>(), pattern); }
        public static IEnumerable Find<T>(this Collections.IReadOnlyElement element, string pattern = "") { return _Find(element, typeof(T), new List<object>(), pattern); }
        private static List<object> _Find(this Collections.IReadOnlyElement element, Type target_type, List<object> results, string pattern = "")
        {
            if (element == null) return results;
            foreach (string key in element.Keys)
            {
                var child_element = element.Get(key) as Collections.IReadOnlyElement;
                if (child_element != null)
                {
                    if (target_type.IsAssignableFrom(child_element.GetType()))
                    {
                        if (pattern.Length == 0) results.Add(child_element);
                        else
                        {
                            if (child_element.Matches(pattern)) results.Add(child_element);
                        }
                    }
                    child_element._Find(target_type, results, pattern);
                }
            }
            return results;
        }*/

        public static string GetName(this Collections.IReadOnlyElement element)
        {
            /*
            if (element.Parent != null)
            {
                foreach (string key in element.Parent.Keys)
                {
                    var test_element = element.Parent.Get<Collections.IElement>(key);
                    if (test_element != null)
                    {
                        if (object.ReferenceEquals(test_element, element)) return key;
                    }
                }
            }
            */
            return string.Empty;
        }
        /*
        public static string GetFullName(this Collections.IReadOnlyElement element)
        {
            var name = GetName(element);
            if (name != null)
            {
                var parent = element.Parent;
                if (parent != null)
                {
                    var parent_full_key = GetFullName(parent);
                    if (parent_full_key.Length > 0)
                    {
                        return $"{parent_full_key}/{name}";
                    }
                }
                return name;
            }
            return string.Empty;
        }*/
    }
}
