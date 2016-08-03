using System;

namespace Node.Net.Deprecated.Collections
{
    public class Copier
    {
        public static void Copy(System.Collections.IEnumerable source, System.Collections.IList destination, IFilter filter = null)
        {
            foreach (object value in source)
            {
                if (object.ReferenceEquals(null, filter) || filter.Include(value))
                {
                    var dictionary = value as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, dictionary))
                    {
                        // Copy by value
                        var hashCopy = System.Activator.CreateInstance(dictionary.GetType()) as System.Collections.IDictionary;
                        Copier.Copy(dictionary, hashCopy, filter);
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
                            Copier.Copy(enumerable, arrayCopy, filter);
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
        public static void Copy(System.Collections.IDictionary source, System.Collections.IDictionary destination, IFilter filter = null)
        {
            foreach (object key in source.Keys)
            {
                var value = source[key];

                var kvp
                    = new System.Collections.Generic.KeyValuePair<object, object>(key, value);
                if (object.ReferenceEquals(null, filter) || filter.Include(kvp))//(key, value))
                {
                    var dictionary = value as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, dictionary))
                    {
                        // Copy by value
                        try {
                            var hashCopy = System.Activator.CreateInstance(dictionary.GetType()) as System.Collections.IDictionary;
                            Copy(dictionary, hashCopy, filter);
                            destination[key] = hashCopy;
                        }
                        catch(Exception e)
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
                            try {
                                arrayCopy = System.Activator.CreateInstance(enumerable.GetType()) as System.Collections.IList;
                            }
                            catch
                            {
                                arrayCopy = new System.Collections.Generic.List<dynamic>();
                            }
                            Copier.Copy(enumerable, arrayCopy, filter);
                            destination[key] = arrayCopy;
                        }
                        else
                        {
                            destination[key] = value;
                        }
                    }
                }
            }
        }

        public static void Copy(System.Collections.IDictionary source, System.Collections.IDictionary destination, System.Type childDictionaryType,IFilter filter = null)
        {
            foreach (object key in source.Keys)
            {
                var value = source[key];

                var kvp
                    = new System.Collections.Generic.KeyValuePair<object, object>(key, value);
                if (object.ReferenceEquals(null, filter) || filter.Include(kvp))
                {
                    var dictionary = value as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, dictionary))
                    {
                        // Copy by value
                        try
                        {
                            var hashCopy = System.Activator.CreateInstance(childDictionaryType) as System.Collections.IDictionary;
                            Copy(dictionary, hashCopy,childDictionaryType, filter);
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
                        if (!object.ReferenceEquals(null, value) && value.GetType() != typeof(string)
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
                            Copier.Copy(enumerable, arrayCopy, filter);
                            destination[key] = arrayCopy;
                        }
                        else
                        {
                            destination[key] = value;
                        }
                    }
                }
            }
        }
    }
}
