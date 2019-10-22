using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Deprecated.Readers
{
    public static class IDictionaryExtension
    {
        public static object ConvertTypes(IDictionary source, Dictionary<string, Type> types,string typeKey = "Type")
        {
            if (source == null) return null;
            if (types == null) return source;
            var copy = Activator.CreateInstance(source.GetType());// as IDictionary;
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");


            var typename = GetTypeName(source,typeKey);
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

            var copy_dictionary = copy as IDictionary;
           // var copy_element = copy as Node.Net.Readers.IElement;
            foreach (var key in source.Keys)
            {
                var value = source[key];
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    //if (copy_element != null) copy_element.Set(key.ToString(), ConvertTypes(childDictionary, types, typeKey));
                    //else if (copy_dictionary != null)
                    //{
                        copy_dictionary[key] = ConvertTypes(childDictionary, types, typeKey);
                    //}

                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        //if (copy_element != null) copy_element.Set(key.ToString(), IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey));
                        //else
                        //{
                            if(copy_dictionary != null)
                            {
                                copy_dictionary[key] = IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey);
                            }
                        //}

                    }
                    else
                    {
                        //if (copy_element != null) copy_element.Set(key.ToString(), value);
                        //else
                        //{
                            if (copy_dictionary != null)
                            {
                                if (copy_dictionary.Contains(key)) copy_dictionary[key] = value;
                                else copy_dictionary.Add(key, value);
                            }
                        //}

                    }
                }
            }
            return copy;
        }

        private static string GetTypeName(object source,string typeKey)
        {
            if (source != null)
            {
                var dictionary = source as IDictionary;
                if (dictionary != null)
                {
                    if (dictionary.Contains(typeKey)) return dictionary[typeKey].ToString();
                }
                /*
                var element = source as Node.Net.Readers.IElement;
                if(element != null)
                {
                    if (element.Contains(typeKey)) return element.Get(typeKey).ToString();
                }*/
            }
            return string.Empty;
        }
    }
}
