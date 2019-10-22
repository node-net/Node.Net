using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net
{
	public static class ActionExtension
	{
		public static void Invoke<T>(this Action<T> action, object[] parameters)
		{
			action((T)parameters[0]);
		}

		public static void Invoke<T1, T2>(this Action<T1, T2> action, object[] parameters)
		{
			action((T1)(parameters[0]), (T2)(parameters[1]));
		}

		/*
		public static T Convert<T>(object value)
		{
			var dictionary = value as IDictionary;
			if(typeof(T) == typeof(IDictionary<string,string>))
			{
				if(dictionary != null)
				{
					var result = new Dictionary<string, string>();
					foreach(string key in dictionary.Keys)
					{
						result.Add(key, dictionary[key].ToString());
					}
					return (T)(object)result;
				}
			}
			return (T)value;
		}*/
	}
}