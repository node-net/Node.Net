using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net
{
    public static class IEnumerableExtension
    {
        public static int ComputeHashCode(this IEnumerable enumerable)
        {
            int hashCode = enumerable.GetCount();
            foreach (object? value in enumerable)
            {
                if (value != null)
                {
                    if (value is bool || value is double || value is float
                        || value is int || value is long || value is string)
                    {
                        hashCode ^= value.GetHashCode();
                    }
                    else
                    {
                        if (value is IDictionary)
                        {
                            hashCode ^= (value as IDictionary).ComputeHashCode();
                        }
                        else
                        {
                            if (value is IEnumerable)
                            {
                                hashCode ^= (value as IEnumerable).ComputeHashCode();
                            }
                        }
                    }
                }
            }
            return hashCode;
        }

        public static IEnumerable Simplify(this IEnumerable list)
        {
            int count = GetCount(list);
            if (count == 0)
            {
                return list;
            }

            bool hasNull = false;
            bool allTypesConvertToDouble = true;
            List<Type>? types = new List<Type>();
            foreach (object? item in list)
            {
                if (item == null)
                {
                    hasNull = true;
                }
                else
                {
                    if (!types.Contains(item.GetType()))
                    {
                        types.Add(item.GetType());
                    }

                    if (!item.GetType().IsPrimitive)
                    {
                        allTypesConvertToDouble = false;
                    }
                }
            }
            if (types.Count == 1 && !hasNull)
            {
                if (types[0] == typeof(string))
                {
                    List<string>? strings = new List<string>();
                    foreach (string? value in list)
                    {
                        if (value != null)
                        {
                            strings.Add(value);
                        }
                    }
                    return strings.ToArray();
                }
                if (types[0] == typeof(double[]))
                {
                    int length = 0;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    foreach (double[] dar in list)
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    {
                        if (length == 0)
                        {
                            length = dar.Length;
                        }

                        if (length != dar.Length)
                        {
                            length = -1;
                        }
                    }
                    if (length > -1)
                    {
                        double[,]? array = new double[count, length];
                        for (int i = 0; i < count; ++i)
                        {
                            for (int j = 0; j < length; ++j)
                            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                array[i, j] = ((double[])GetAt(list, i))[j];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
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
                    List<double>? doubles = new List<double>();
                    foreach (object? value in list) { doubles.Add(Convert.ToDouble(value)); }
                    return doubles.ToArray();
                }
            }

            return list;
        }

        public static object? GetAt(this IEnumerable source, int index)
        {
            if (source != null)
            {
                int i = 0;
                foreach (object? item in source)
                {
                    if (i == index)
                    {
                        return item;
                    }

                    ++i;
                }
            }
            return null;
        }

        public static int GetCount(this IEnumerable source)
        {
            if (source != null)
            {
                int result = 0;
                foreach (object? item in source) { ++result; }
                return result;
            }
            return 0;
        }

        public static IEnumerable? ConvertTypes(this IEnumerable source, Dictionary<string, Type> types, Type defaultType, string typeKey = "Type")
        {
            if (source == null)
            {
                return null;
            }

            IList copy = new List<dynamic>();
            if (source is IList)
            {
                // TODO: convert arrays of double[], int[], string[], etc when appropriate
                if (source.GetType().GetConstructor(Type.EmptyTypes) != null)
                {
                    copy = (Activator.CreateInstance(source.GetType()) as IList)!;
                }
            }
            foreach (object? value in source)
            {
                if (value is IDictionary childDictionary)
                {
                    copy.Add(childDictionary.ConvertTypes(types, defaultType, typeKey));
                }
                else
                {
                    if (value is IEnumerable childEnumerable && !(childEnumerable is string))
                    {
                        copy.Add(ConvertTypes(childEnumerable, types, defaultType));
                    }
                    else
                    {
                        copy.Add(value);
                    }
                }
            }
            return Simplify(copy);
        }

        public static IList<string> GetNames(this IEnumerable source)
        {
            List<string>? names = new List<string>();
            foreach (object? item in source)
            {
                if (item != null)
                {
                    string? name = item.GetName();
                    if (!names.Contains(name))
                    {
                        names.Add(name);
                    }
                }
            }

            return names;
        }

        public static IList<string> GetFullNames(this IEnumerable source)
        {
            List<string>? fullNames = new List<string>();
            foreach (object? item in source)
            {
                if (item != null)
                {
                    string? fullname = item.GetName();
                    if (!fullNames.Contains(fullname))
                    {
                        fullNames.Add(fullname);
                    }
                }
            }
            return fullNames;
        }
    }
}