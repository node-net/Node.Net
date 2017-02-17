using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Beta
{
    public static class IDictionaryExtension
    {
        public static IList Collect(this IDictionary dictionary, Type type) => Internal.IDictionaryExtension.Collect(dictionary, type);
        public static object GetParent(this IDictionary dictionary) => Internal.IDictionaryExtension.GetParent(dictionary);
        public static string GetName(this IDictionary dictionary) => Internal.IDictionaryExtension.GetName(dictionary);
    }
}
