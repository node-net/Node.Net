using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    public static class IElementExtension
    {
        public static Node.Net.Readers.IElement ConvertTypes(this Node.Net.Readers.IElement source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;
            if (types == null) return source;
            var copy = Activator.CreateInstance(source.GetType()) as Node.Net.Readers.IElement;
            if (copy == null) throw new Exception($"failed to create instance of type {source.GetType().FullName}");
            var typename = GetTypeName(source, typeKey);
            if (typename.Length > 0 && types.ContainsKey(typename))
            {
                var targetType = types[typename];
                if (targetType == null) throw new Exception($"types['{typename}'] was null");
                if (source.GetType() != targetType)
                {
                    copy = Activator.CreateInstance(targetType) as Node.Net.Readers.IElement;
                    if (copy == null) throw new Exception($"failed to create instance of type {targetType.FullName}");
                }
            }
            foreach (var key in source.Keys)
            {
                var value = source.Get(key);
                var childDictionary = value as Node.Net.Readers.IElement;
                if (childDictionary != null)
                {
                    copy.Set(key, ConvertTypes(childDictionary, types, typeKey));
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        copy.Set(key, IEnumerableExtension.ConvertTypes(childEnumerable, types, typeKey));
                    }
                    else
                    {
                        if (copy.Contains(key)) copy.Set(key, value);
                        else copy.Set(key, value);
                    }
                }
            }
            return copy;
        }

        private static string GetTypeName(this Node.Net.Readers.IElement source, string typeKey)
        {
            if (source != null)
            {
                if (source.Contains(typeKey)) return source.Get<string>(typeKey);
            }
            return string.Empty;
        }
    }
}
