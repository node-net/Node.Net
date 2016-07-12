using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data.Readers
{
    public class DictionaryTypeConverter : IDictionaryTypeConverter
    {
        public Dictionary<string, Type> Types { get; set; } = new Dictionary<string, Type>();
        public IDictionary Convert(IDictionary source)
        {
            if (source.Contains(nameof(Type)))
            {
                var type = source[nameof(Type)].ToString();
                if(Types != null && Types.ContainsKey(type))
                {
                    var dictionary = Activator.CreateInstance(Types[type]) as IDictionary;
                    Copy(source, dictionary);
                    return dictionary;
                }
            }
            return source;
        }

        public static void Copy(IDictionary source, IDictionary destination)
        {
            foreach (object key in source.Keys)
            {
                var value = source[key];

                var kvp
                    = new System.Collections.Generic.KeyValuePair<object, object>(key, value);
                var dictionary = value as System.Collections.IDictionary;
                if (!object.ReferenceEquals(null, dictionary))
                {
                    // Copy by value
                    try
                    {
                        var hashCopy = System.Activator.CreateInstance(dictionary.GetType()) as System.Collections.IDictionary;
                        Copy(dictionary, hashCopy);
                        destination[key] = hashCopy;
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"unable to create instance of type {dictionary.GetType().FullName}", e);
                    }
                }
                else
                {
                    var enumerable = value as System.Collections.IEnumerable;
                    if (!object.ReferenceEquals(null, value)
                        && value.GetType() != typeof(string)
                        && value.GetType() != typeof(byte[])
                        && !object.ReferenceEquals(null, enumerable))
                    {
                        System.Collections.IList arrayCopy = new System.Collections.Generic.List<dynamic>();
                        // Copy by value
                        try
                        {
                            arrayCopy = System.Activator.CreateInstance(enumerable.GetType()) as System.Collections.IList;
                        }
                        catch
                        {
                            arrayCopy = new System.Collections.Generic.List<dynamic>();
                        }
                        Copy(enumerable, arrayCopy);
                        destination[key] = arrayCopy;
                    }
                    else
                    {
                        destination[key] = value;
                    }
                }
            }
        }

        public static void Copy(IEnumerable source, IList destination)
        {
            foreach (object value in source)
            {
                var dictionary = value as System.Collections.IDictionary;
                if (!object.ReferenceEquals(null, dictionary))
                {
                    // Copy by value
                    var hashCopy = System.Activator.CreateInstance(dictionary.GetType()) as System.Collections.IDictionary;
                    Copy(dictionary, hashCopy);
                    destination.Add(hashCopy);
                }
                else
                {
                    var enumerable = value as System.Collections.IEnumerable;
                    if (!object.ReferenceEquals(null, value) && value.GetType() != typeof(string)
                        && !object.ReferenceEquals(null, enumerable))
                    {
                        // Copy by value
                        var arrayCopy = System.Activator.CreateInstance(enumerable.GetType()) as System.Collections.IList;
                        Copy(enumerable, arrayCopy);
                        destination.Add(arrayCopy);
                    }
                    else
                    {
                        destination.Add(value);
                    }
                }
            }
        }
    }
}
