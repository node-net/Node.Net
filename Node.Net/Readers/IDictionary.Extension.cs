using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Readers
{
    public static class IDictionaryExtension
    {
        public static IDictionary ConvertTypes(IDictionary source, Dictionary<string, Type> types,string typeKey = "Type")
        {
            if (source == null) return null;
            var copy = Activator.CreateInstance(source.GetType()) as IDictionary;
            var typename = GetTypeName(source,typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (source.GetType() != targetType)
                {
                    copy = Activator.CreateInstance(targetType) as IDictionary;
                }
            }
            foreach (var key in source.Keys)
            {
                var value = source[key];
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    copy[key] = ConvertTypes(childDictionary, types);
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        copy[key] = IEnumerableExtension.ConvertTypes(childEnumerable, types);
                    }
                    else
                    {
                        if (copy.Contains(key)) copy[key] = value;
                        else copy.Add(key, value);
                    }
                }
            }
            return copy;
        }

        private static string GetTypeName(IDictionary source,string typeKey)
        {
            if (source != null)
            {
                //if (source.Contains("Type")) return source["Type"].ToString();
                if (source.Contains(typeKey)) return source[typeKey].ToString();
            }
            return string.Empty;
        }
    }
}
