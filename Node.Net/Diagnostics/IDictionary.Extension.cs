using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Diagnostics
{
    public static class IDictionaryExtension
    {
        public static T Get<T>(IDictionary dictionary,string name, T defaultValue)
        {
            if (dictionary.Contains(name))
            {
                if (typeof(T) == typeof(DateTime))
                {
                    return (T)((object)DateTime.Parse(dictionary[name].ToString()));
                }
                return (T)dictionary[name];
            }
            return defaultValue;
        }
        public static void Set(IDictionary dictionary,string name,object value)
        {
            dictionary[name] = value;
        }
    }
}
