using System;

namespace Node.Net.Json
{
    public class Copier
    {
        public static void Copy(System.Collections.IEnumerable source, System.Collections.IList destination, IFilter filter = null)
        {
            foreach (object value in source)
            {
                if (object.ReferenceEquals(null, filter) || filter.Include(value))//(null, value))
                {
                    System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, dictionary))
                    {
                        // Copy by value
                        System.Collections.IDictionary hashCopy = System.Activator.CreateInstance(dictionary.GetType()) as System.Collections.IDictionary;
                        Copier.Copy(dictionary, hashCopy, filter);
                        destination.Add(hashCopy);
                    }
                    else
                    {
                        System.Collections.IEnumerable enumerable = value as System.Collections.IEnumerable;
                        if (!object.ReferenceEquals(null, value) && value.GetType() != typeof(string)
                            && !object.ReferenceEquals(null, enumerable))
                        {
                            // Copy by value
                            System.Collections.IList arrayCopy = System.Activator.CreateInstance(enumerable.GetType()) as System.Collections.IList;
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
                object value = source[key];

                System.Collections.Generic.KeyValuePair<object, object> kvp
                    = new System.Collections.Generic.KeyValuePair<object, object>(key, value);
                if (object.ReferenceEquals(null, filter) || filter.Include(kvp))//(key, value))
                {
                    System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
                    if (!object.ReferenceEquals(null, dictionary))
                    {
                        // Copy by value
                        try {
                            System.Collections.IDictionary hashCopy = System.Activator.CreateInstance(dictionary.GetType()) as System.Collections.IDictionary;
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
                        System.Collections.IEnumerable enumerable = value as System.Collections.IEnumerable;
                        if (!object.ReferenceEquals(null, value) && value.GetType() != typeof(string)
                            && !object.ReferenceEquals(null, enumerable))
                        {
                            // Copy by value
                            try {
                                System.Collections.IList arrayCopy = System.Activator.CreateInstance(enumerable.GetType()) as System.Collections.IList;
                                Copier.Copy(enumerable, arrayCopy, filter);
                                destination[key] = arrayCopy;
                            }
                            catch (Exception e)
                            {
                                throw new Exception($"unable to create instance of type {enumerable.GetType().FullName}", e);
                            }
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
