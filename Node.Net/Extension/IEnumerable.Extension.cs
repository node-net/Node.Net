﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class IEnumerableExtension
    {
        public static object GetAt(this IEnumerable source, int index) => Beta.Internal.IEnumerableExtension.GetAt(source,index);
        public static IEnumerable ConvertTypes(this IEnumerable source, Dictionary<string, Type> types, string typeKey = "Type") => Beta.Internal.IEnumerableExtension.ConvertTypes(source, types, typeKey);
        public static IDictionary ToDictionary(this IEnumerable source)
        {
            var dictionary = new Dictionary<string, dynamic>();
            foreach(var item in source)
            {
                var name = item.GetName();
                if(name.Length > 0)
                {
                    if(dictionary.ContainsKey(name))
                    {
                        var fullname = item.GetFullName();
                        if(!dictionary.ContainsKey(fullname))
                        {
                            dictionary.Add(fullname, item);
                        }
                    }
                    else
                    {
                        dictionary.Add(name, item);
                    }
                }
            }
            return dictionary;
        }
    }
}
