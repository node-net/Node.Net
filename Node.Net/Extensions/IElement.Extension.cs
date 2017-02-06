using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class IElementExtension
    {
        public static int GetDeepCount(this IElement element)
        {
            var count = element.Count;
            foreach (string key in element.Keys)
            {
                var child_element = element[key] as IElement;
                if (child_element != null)
                {
                    count += child_element.GetDeepCount();
                }
            }
            return count;
        }
        public static string GetJSON(this IElement element)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                new Writers.JsonWriter { Format = Writers.JsonFormat.Indented }.Write(memory, element);
                memory.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(memory))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public static T Get<T>(this IElement element, string name)
        {
            if (element.Contains(name))
            {
                var value = element[name];
                if (value != null && typeof(T).IsAssignableFrom(value.GetType()))
                {
                    return (T)value;
                }

            }
            if (typeof(string) == typeof(T)) return (T)(object)string.Empty;
            return default(T);
        }

        public static bool Matches(this IElement element, string pattern = "")
        {
            if (pattern.Length == 0) return true;
            if (element.GetFullName().Contains(pattern)) return true;
            foreach (var key in element.Keys)
            {
                var string_value = element.Get<string>(key.ToString());
                if (string_value.Length > 0)
                {
                    if (string_value.Contains(pattern)) return true;
                }
            }
            return false;
        }
        public static IList Find(this IElement element, Type target_type, string pattern = "") { return _Find(element, target_type, new List<object>(), pattern); }
        public static IList Find<T>(this IElement element, string pattern = "") { return _Find(element, typeof(T), new List<object>(), pattern); }
        private static List<object> _Find(this IElement element, Type target_type, List<object> results, string pattern = "")
        {
            if (element == null) return results;
            foreach (var key in element.Keys)
            {
                var child_element = element[key] as IElement;
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
        }

        public static string GetName(this IElement element)
        {
            if (element.Parent != null)
            {
                var eparent = element.Parent as IElement;
                if (eparent != null)
                {
                    foreach (string key in eparent.Keys)
                    {
                        var test_element = eparent.Get<IElement>(key);
                        if (test_element != null)
                        {
                            if (object.ReferenceEquals(test_element, element)) return key;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static string GetFullName(this IElement element)
        {
            var name = GetName(element);
            if (name != null)
            {
                var parent = element.Parent as IElement;
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
        }

        public static IDocument GetDocument(this IElement element)
        {
            var parent = element.Parent as IElement;
            if (parent != null)
            {
                var document = parent as IDocument;
                if (document != null) return document;
                return GetDocument(parent);
            }
            return null;
        }

        public static T GetAncestor<T>(this IElement element)
        {
            var parent = element.Parent as IElement;
            if (element != null && parent != null)
            {
                if (typeof(T).IsAssignableFrom(parent.GetType()))
                {
                    var ancestor = (T)parent;
                    if (ancestor != null) return ancestor;
                }
                return GetAncestor<T>(parent);
            }
            return default(T);
        }

        public static IElement ConvertTypes(this IElement source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;
            if (types == null) return source;
            var copy = Activator.CreateInstance(source.GetType()) as IElement;
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
            var typename = GetTypeName(source, typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (targetType == null) throw new Exception($"types['{typename}'] was null");
                if (source.GetType() != targetType)
                {
                    copy = Activator.CreateInstance(targetType) as IElement;
                    if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
                }
            }
            foreach (var key in source.Keys)
            {
                var value = source[key];
                var childDictionary = value as IElement;
                if (childDictionary != null)
                {
                    copy.Set(key.ToString(), ConvertTypes(childDictionary, types, typeKey));
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        copy.Set(key.ToString(), IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey));
                    }
                    else
                    {
                        if (copy.Contains(key)) copy.Set(key.ToString(), value);
                        else copy.Set(key.ToString(), value);
                    }
                }
            }
            return copy;
        }

        private static string GetTypeName(this IElement source, string typeKey)
        {
            if (source != null)
            {
                if (source.Contains(typeKey)) return source.Get<string>(typeKey);
            }
            return string.Empty;
        }

        public static Matrix3D GetLocalToParent(this IElement element) => Node.Net.Factories.ObjectExtension.GetLocalToParent(element);
        public static Matrix3D GetLocalToWorld(this IElement element) => Node.Net.Factories.ObjectExtension.GetLocalToWorld(element);

        public static void DeepUpdateParents(this IElement element)
        {
            foreach(var key in element.Keys)
            {
                var child_dictionary = element[key] as IDictionary;
                if(child_dictionary != null)
                {
                    Node.Net.Collections.ObjectExtension.SetParent(child_dictionary, element);
                    DeepUpdateParents(child_dictionary);
                }
                var child_element = element[key] as IElement;
                if(child_element != null)
                {
                    child_element.Parent = element;
                    DeepUpdateParents(child_element);
                }
            }
        }
        private static void DeepUpdateParents(IDictionary dictionary)
        {
            foreach(string key in dictionary.Keys)
            {
                var child_dictionary = dictionary[key] as IDictionary;
                if (child_dictionary != null)
                {
                    Node.Net.Collections.ObjectExtension.SetParent(child_dictionary, dictionary);
                    DeepUpdateParents(child_dictionary);
                }
                var child_element = dictionary[key] as IElement;
                if (child_element != null)
                {
                    child_element.Parent = dictionary;
                    DeepUpdateParents(child_element);
                }
            }
        }
    }
}
