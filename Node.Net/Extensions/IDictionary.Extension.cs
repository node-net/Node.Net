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
            List<string> keys = new List<string>();
            foreach (string key in dictionary.Keys)
            {
                if (object.ReferenceEquals(null, filter) || filter.Include(dictionary[key]))
                {
                    keys.Add(key);
                }
                IDictionary child_dictionary = dictionary[key] as IDictionary;
                if (!object.ReferenceEquals(null, child_dictionary))
                {
                    string[] subkeys = Find(child_dictionary, filter);
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
        
            Dictionary<string, dynamic> results = new Dictionary<string, dynamic>();
            if (!object.ReferenceEquals(null, dictionary))
            {
                List<string> keys = new List<string>(Find(dictionary,filter));
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
                List<string> parts = new List<string>(key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                if (dictionary.Contains(parts[0]))
                {
                    IDictionary subDictionary = dictionary[parts[0]] as IDictionary;
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
                List<string> parts = new List<string>(key.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                if (!dictionary.Contains(parts[0]))
                {
                    dictionary.Add(parts[0], new Dictionary<string, dynamic>());
                }

                IDictionary subDictionary = dictionary[parts[0]] as IDictionary;
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
            Node.Net.Json.JsonFormatter formatter = new Node.Net.Json.JsonFormatter()
            {
                Style = Node.Net.Json.JsonStyle.Indented
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
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            foreach (string arg in args)
            {
                if (arg.IndexOf('=') > -1)
                {
                    char[] delimiters = { '=' };
                    string[] words = arg.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string key = words[0].Trim();
                    string value = words[1].Trim().Replace("\"", "");
                    if (key.Length > 0 && value.Length > 0)
                    {
                        result[key] = value;
                    }
                }
            }
            return result;
        }
    }
}
