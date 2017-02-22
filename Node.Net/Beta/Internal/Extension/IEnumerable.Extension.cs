using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Beta.Internal
{
    static class IEnumerableExtension
    {
        public static int ComputeHashCode(this IEnumerable enumerable)
        {
            var hashCode = enumerable.GetCount();
            foreach (object value in enumerable)
            {
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
        public static IEnumerable Simplify(this IEnumerable list)
        {
            var count = GetCount(list);
            if (count == 0) return list;
            var hasNull = false;
            var allTypesConvertToDouble = true;
            var types = new List<Type>();
            foreach (var item in list)
            {
                if (item == null) hasNull = true;
                else
                {
                    if (!types.Contains(item.GetType())) types.Add(item.GetType());
                    if (!item.GetType().IsPrimitive) allTypesConvertToDouble = false;
                }
            }
            if (types.Count == 1 && !hasNull)
            {
                if (types[0] == typeof(string))
                {
                    var strings = new List<string>();
                    foreach (string value in list) { strings.Add(value); }
                    return strings.ToArray();
                }
                if (types[0] == typeof(double[]))
                {
                    var length = 0;
                    foreach (double[] dar in list)
                    {
                        if (length == 0) length = dar.Length;
                        if (length != dar.Length) length = -1;
                    }
                    if (length > -1)
                    {
                        var array = new double[count, length];
                        for (int i = 0; i < count; ++i)
                        {
                            for (int j = 0; j < length; ++j)
                            {
                                array[i, j] = ((double[])GetAt(list, i))[j];
                            }
                        }
                        return array;
                    }
                }
            }
            if (allTypesConvertToDouble)
            {
                if (types[0] == typeof(double) || types[0] == typeof(int) || types[0] == typeof(float))
                {
                    var doubles = new List<double>();
                    foreach (var value in list) { doubles.Add(Convert.ToDouble(value)); }
                    return doubles.ToArray();
                }

            }

            return list;
        }

        public static object GetAt(this IEnumerable source, int index)
        {
            if (source != null)
            {
                var i = 0;
                foreach (var item in source)
                {
                    if (i == index) return item;
                    ++i;
                }
            }
            return null;
        }
        public static int GetCount(this IEnumerable source)
        {
            if (source != null)
            {
                var result = 0;
                foreach (var item in source) { ++result; }
                return result;
            }
            return 0;
        }

        public static IEnumerable ConvertTypes(this IEnumerable source, Dictionary<string, Type> types, string typeKey = "Type")
        {
            if (source == null) return null;
            IList copy = new List<dynamic>();
            if (typeof(IList).IsAssignableFrom(source.GetType()))
            {
                // TODO: convert arrays of double[], int[], string[], etc when appropriate
                if (source.GetType().GetConstructor(Type.EmptyTypes) != null)
                {
                    copy = Activator.CreateInstance(source.GetType()) as IList;
                }
            }
            foreach (var value in source)
            {
                var childDictionary = value as IDictionary;
                if (childDictionary != null)
                {
                    copy.Add(childDictionary.ConvertTypes(types, typeKey));
                }
                else
                {
                    var childEnumerable = value as IEnumerable;
                    if (childEnumerable != null && childEnumerable.GetType() != typeof(string))
                    {
                        copy.Add(ConvertTypes(childEnumerable, types));
                    }
                    else
                    {
                        copy.Add(value);
                    }
                }
            }
            return Simplify(copy);
        }
    }
}
