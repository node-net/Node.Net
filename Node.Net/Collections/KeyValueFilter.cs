using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class KeyValueFilter : IFilter
    {
        public static Func<object,bool?> GetIncludeFunction(string key,string value,bool exact_match)
        {
            return new KeyValueFilter { Key = key, Values = { value }, ExactMatch = exact_match }.Include;
        }
        public string Key { get; set; } = string.Empty;
        public bool ExactMatch { get; set; } = false;
        public List<string> Values { get; set; } = new List<string>();
        public List<string> ExcludeValues { get; set; } = new List<string>();
        //private TypeFilter typeFilter = new TypeFilter
        public bool? Include(object source)
        {
            var dictionary = source as IDictionary;
            if (dictionary == null)
            {
                return false;
            }
            else
            {
                if (dictionary.Contains(Key))
                {
                    var tvalue = dictionary[Key].ToString();

                    if (tvalue.Length > 0)
                    {
                        foreach(var excludeValue in ExcludeValues)
                        {
                            if(ExactMatch)
                            {
                                if (excludeValue == tvalue) return false;
                            }
                            else
                            {
                                if (tvalue.Contains(excludeValue)) return false;
                            }
                        }
                        foreach (var value in Values)
                        {
                            if (ExactMatch)
                            {
                                if (value == tvalue) return true;
                            }
                            else
                            {
                                if (tvalue.Contains(value)) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
