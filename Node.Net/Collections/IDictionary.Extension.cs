using System;
using System.Collections;

namespace Node.Net.Collections
{
    public static class IDictionaryExtension
    {
        public static T Get<T>(IDictionary dictionary, string name)
        {
            if (dictionary.Contains(name))
            {
                var value = dictionary[name];
                if (value != null)
                {
                    if (dictionary[name] != null && typeof(T).IsAssignableFrom(value.GetType()))
                    {
                        return (T)dictionary[name];
                    }
                    if (typeof(T) == typeof(DateTime) && value.GetType()== typeof(string))
                    {
                        return (T)((object)DateTime.Parse(value.ToString()));
                    }
                }
            }
            return default(T);
        }

        public static void Set(IDictionary dictionary,string key,object value)
        {
            if (value == null) dictionary[key] = value;
            else
            {
                if (value.GetType() == typeof(DateTime))
                {
                    dictionary[key] = ((DateTime)value).ToString("o");
                }
                else
                {
                    dictionary[key] = value;
                }
            }
        }
    }
}
