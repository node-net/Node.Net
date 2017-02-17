using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Factories.Prototype.Internal
{
    static class IDictionaryExtension
    {
        public static string GetJSON(this IDictionary dictionary) { return new Internal.Writers.JSONWriter().WriteToString(dictionary); }// return Writers.JSONWriter.Default.WriteToString(dictionary); }
        public static void Save(this IDictionary dictionary, Stream stream) { new Internal.Writers.JSONWriter().Write(stream, dictionary); }
        public static void SetParent(this IDictionary dictionary, object parent) { Internal.Collections.MetaData.Default.GetMetaData(dictionary)["Parent"] = parent; }
        public static object GetParent(this IDictionary dictionary) { return Internal.Collections.MetaData.Default.GetMetaData(dictionary, "Parent"); }
        public static void SetFileName(this IDictionary dictionary,string filename) { Internal.Collections.MetaData.Default.GetMetaData(dictionary)["FileName"] = filename; }
        public static string GetFileName(this IDictionary dictionary) { return Internal.Collections.MetaData.Default.GetMetaData<string>(dictionary, "FileName"); }
        public static void DeepUpdateParents(this IDictionary dictionary)
        {
            foreach (var value in dictionary.Values)
            {
                var child = value as IDictionary;
                if (child != null)
                {
                    child.SetParent(dictionary); DeepUpdateParents(child);
                }
            }
        }
        public static int ComputeHashCode(this IDictionary dictionary)
        {
            var hashCode = dictionary.Count;
            foreach (var key in dictionary.Keys)
            {
                hashCode = hashCode ^ key.GetHashCode();
                var value = dictionary[key];
                if (value != null)
                {
                    if (value.GetType() == typeof(bool) ||
                       value.GetType() == typeof(double) ||
                       value.GetType() == typeof(float) ||
                       value.GetType() == typeof(int) ||
                       value.GetType() == typeof(long) ||
                       value.GetType() == typeof(string))
                    {
                        hashCode = hashCode ^ value.GetHashCode();
                    }
                    else
                    {
                        if (typeof(IDictionary).IsAssignableFrom(value.GetType())) hashCode = hashCode ^ (value as IDictionary).ComputeHashCode();
                        else
                        {
                            if (typeof(IDictionary).IsAssignableFrom(value.GetType())) hashCode = hashCode ^ (value as IEnumerable).ComputeHashCode();
                        }
                    }
                }
            }
            return hashCode;
        }
        public static Stream GetStream(this IDictionary dictionary)
        {
            var memory = new MemoryStream();
            dictionary.Save(memory);
            memory.Flush();
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }
        public static ICollection Collect<T>(this IDictionary idictionary)
        {
            var results = new List<object>();
            _Collect<T>(idictionary, results);

            return results;
        }
        private static void _Collect<T>(this IDictionary idictionary, IList results)
        {
            foreach (var item in idictionary.Values)
            {
                if (item != null)
                {
                    if (typeof(T).IsAssignableFrom(item.GetType()))
                    {
                        if (!results.Contains(item)) results.Add(item);
                    }
                    var child_idictionary = item as IDictionary;
                    if (child_idictionary != null) _Collect<T>(child_idictionary, results);
                }
            }
        }

        public static IDictionary Copy(this IDictionary dictionary, IDictionary source)
        {
            dictionary.Clear();
            foreach (var key in source)
            {
                var value = source[key];
                var child_dictionary = value as IDictionary;
                if (child_dictionary != null)
                {
                    dictionary[key] = new Dictionary<object, dynamic>().Copy(child_dictionary);
                }
                else
                {
                    dictionary[key] = value;
                }
            }
            return dictionary;
        }
        public static T Get<T>(this IDictionary dictionary, string name)
        {
            if (dictionary.Contains(name))
            {
                var value = dictionary[name];
                if (value != null)
                {
                    if (typeof(T).IsAssignableFrom(value.GetType())) return (T)value;
                }
            }
            if (name.Contains(","))
            {
                var names = name.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var n in names)
                {
                    if (dictionary.Contains(n)) return dictionary.Get<T>(n);
                }
            }
            if (typeof(T) == typeof(string)) return (T)(object)string.Empty;
            return default(T);
        }
        public static string GetName(this IDictionary dictionary)
        {
            var parent = GetParent(dictionary) as IDictionary;
            if (parent != null)
            {
                foreach (string key in parent.Keys)
                {
                    var test_element = parent.Get<IDictionary>(key);
                    if (test_element != null)
                    {
                        if (object.ReferenceEquals(test_element, dictionary)) return key;
                    }
                }
            }
            return string.Empty;
        }
        public static IDictionary ConvertTypes(this IDictionary source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;
            if (types == null) return source;
            var copy = Activator.CreateInstance(source.GetType()) as IDictionary;
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
            var typename = source.Get<string>(typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (targetType == null) throw new Exception($"types['{typename}'] was null");
                if (source.GetType() != targetType)
                {
                    copy = Activator.CreateInstance(targetType) as IDictionary;
                    if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
                }
            }
            foreach (string key in source.Keys)
            {
                var value = source[key];// source.Get(key);
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    copy[key] = ConvertTypes(childDictionary, types, typeKey);
                    //copy.Set(key, ConvertTypes(childDictionary, types, typeKey));
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        copy[key] = IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey);
                        //copy.Set(key, IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey));
                    }
                    else
                    {
                        copy[key] = value;
                        //if (copy.Contains(key)) copy.Set(key, value);
                        //else copy.Set(key, value);
                    }
                }
            }
            return copy;
        }
    }
}
