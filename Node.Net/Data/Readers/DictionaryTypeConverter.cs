using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Node.Net.Data.Readers
{
    public class DictionaryTypeConverter : IDictionaryTypeConverter
    {
        public DictionaryTypeConverter() { }
        public DictionaryTypeConverter(Assembly assembly)
        {
            AddTypes(assembly);
        }
        public Dictionary<string, Type> Types { get; set; } = new Dictionary<string, Type>();

        public IDictionary Convert(IDictionary source)
        {
            if (source != null && source.Contains(nameof(Type)))
            {
                var type = source[nameof(Type)].ToString();
                if (Types != null && Types.ContainsKey(type))
                {
                    try
                    {
                        var _type = Types[type];
                        if (_type == null) throw new Exception($"Types['{type}'] was null");
                        var dictionary = Activator.CreateInstance(_type) as IDictionary;
                        Copy(source, dictionary);
                        return dictionary;
                    }
                    catch(Exception ex)
                    {
                        throw new Exception($"Exception while converting type '{type}', fullname {Types[type].FullName}", ex);
                    }
                }
            }
            return source;
        }

        private IDictionary Create(IDictionary source)
        {
            IDictionary result = null;

            if (source.Contains(nameof(Type)))
            {
                var type = source[nameof(Type)].ToString();
                if (Types != null && Types.ContainsKey(type))
                {
                    result = Activator.CreateInstance(Types[type]) as IDictionary;
                }
            }
            if(result == null) result = System.Activator.CreateInstance(source.GetType()) as IDictionary;
            return result;
        }


        public void AddType(Type type)
        {
            var defaultConstructorInfo = type.GetConstructor(Type.EmptyTypes);
            if (defaultConstructorInfo != null)
            {
                Types[type.Name] = type;
            }
        }

        public void AddTypes(Assembly assembly)
        {
            foreach(var type in assembly.GetTypes())
            {
                var defaultConstructorInfo = type.GetConstructor(Type.EmptyTypes);
                if (defaultConstructorInfo != null)
                {
                    Types[type.Name] = type;
                }
            }
        }

        public void Copy(IDictionary source, IDictionary destination)
        {
            if (source == null) return;
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
                        var child = Create(dictionary);
                        Copy(dictionary, child);
                        destination[key] = child;
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

        public void Copy(IEnumerable source, IList destination)
        {
            foreach (object value in source)
            {
                var dictionary = value as System.Collections.IDictionary;
                if (!object.ReferenceEquals(null, dictionary))
                {
                    // Copy by value
                    var child = Create(dictionary);
                    Copy(dictionary, child);
                    destination.Add(child);
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
