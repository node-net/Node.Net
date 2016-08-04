using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Extensions
{
    public class IDictionaryExtension
    {
        public static string[] Find(IDictionary dictionary, IFilter filter)
        {
            var keys = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                if (object.ReferenceEquals(null, filter) || filter.Include(dictionary[key]))
                {
                    keys.Add(key);
                }
                var child_dictionary = dictionary[key] as IDictionary;
                if (!object.ReferenceEquals(null, child_dictionary))
                {
                    var subkeys = Find(child_dictionary, filter);
                    foreach (string subkey in subkeys)
                    {
                        keys.Add($"{key}/{subkey}");
                    }
                }
            }
            return keys.ToArray();
        }

        public static IDictionary Collect(IDictionary dictionary,IFilter filter)
        {

            var results = new Dictionary<string, dynamic>();
            if (!object.ReferenceEquals(null, dictionary))
            {
                var keys = new List<string>(Find(dictionary,filter));
                foreach (string key in keys)
                {
                    results.Add(key, Get(dictionary,key));
                }
            }
            return results;
        }

        public static object Get(IDictionary dictionary, string key)
        {
            if (key.Contains("/"))
            {
                var parts = new List<string>(key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                if (dictionary.Contains(parts[0]))
                {
                    var subDictionary = dictionary[parts[0]] as IDictionary;
                    if (!object.ReferenceEquals(null, subDictionary))
                    {
                        parts.RemoveAt(0);
                        if (parts.Count == 1) return Get(subDictionary, parts[0]);
                        else return Get(subDictionary, String.Join("/", parts));
                    }
                }
            }
            else
            {
                if (dictionary.Contains(key)) return dictionary[key];
            }
            return null;
        }
        public static void Set(IDictionary dictionary, string key, object value)
        {
            if (key.Contains("/"))
            {
                var parts = new List<string>(key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                if (!dictionary.Contains(parts[0]))
                {
                    dictionary.Add(parts[0], new Dictionary<string, dynamic>());
                }

                var subDictionary = dictionary[parts[0]] as IDictionary;
                if (object.ReferenceEquals(null, subDictionary))
                {
                    subDictionary = new Dictionary<string, dynamic>();
                    dictionary[parts[0]] = subDictionary;
                }
                parts.RemoveAt(0);
                if (parts.Count == 1)
                {
                    Set(subDictionary, parts[0], value);
                }
                else
                {
                    Set(subDictionary, String.Join("/", parts), value);
                }
            }
            else
            {
                if (dictionary.Contains(key)) dictionary[key] = value;
                else dictionary.Add(key, value);
            }
        }


        public static void Save(IDictionary dictionary, Stream stream)
        {
            var formatter = new Node.Net.Deprecated.Json.JsonFormatter
            {
                Style = Node.Net.Deprecated.Json.JsonStyle.Indented
            };
            formatter.Serialize(stream, dictionary);
        }

        public static void Save(IDictionary dictionary, string filename)
        {
            using (FileStream stream = File.OpenWrite(filename))
            {
                Save(dictionary, stream);
            }
        }

        public static IDictionary Parse(string[] args)
        {
            var result = new Dictionary<string, dynamic>();
            foreach (string arg in args)
            {
                if (arg.IndexOf('=') > -1)
                {
                    char[] delimiters = { '=' };
                    var words = arg.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    var key = words[0].Trim();
                    var value = words[1].Trim().Replace("\"", "");
                    if (key.Length > 0 && value.Length > 0)
                    {
                        result[key] = value;
                    }
                }
            }
            return result;
        }

        public static bool IsChildKey(IDictionary dictionary, object key)
        {
            if (dictionary == null) return false;
            if (!dictionary.Contains(key)) return false;
            var value = dictionary[key];
            if (value == null) { return false; }
            else if (value.GetType().IsValueType || value.GetType() == typeof(string)) { return false; }
            return true;
        }
    }
}
