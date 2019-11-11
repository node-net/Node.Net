﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net
{
	public static class IEnumerableExtension
	{
		public static int ComputeHashCode(this IEnumerable enumerable)
		{
			var hashCode = enumerable.GetCount();
			foreach (object value in enumerable)
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
			var count = GetCount(list);
			if (count == 0)
			{
				return list;
			}

			var hasNull = false;
			var allTypesConvertToDouble = true;
			var types = new List<Type>();
			foreach (var item in list)
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
					var strings = new List<string>();
					foreach (string value in list) { strings.Add(value); }
					return strings.ToArray();
				}
				if (types[0] == typeof(double[]))
				{
					var length = 0;
					foreach (double[] dar in list)
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
				var result = 0;
				foreach (var item in source) { ++result; }
				return result;
			}
			return 0;
		}

		public static IEnumerable ConvertTypes(this IEnumerable source, Dictionary<string, Type> types, Type defaultType, string typeKey = "Type")
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
					copy = Activator.CreateInstance(source.GetType()) as IList;
				}
			}
			foreach (var value in source)
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
			var names = new List<string>();
			foreach (var item in source)
			{
				if (item != null)
				{
					var name = item.GetName();
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
			var fullNames = new List<string>();
			foreach (var item in source)
			{
				if (item != null)
				{
					var fullname = item.GetName();
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