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
        public string Key { get; set; } = string.Empty;
        public bool ExactMatch { get; set; } = false;
        public List<string> Values { get; set; } = new List<string>();
        public bool Include(object source)
        {
            var dictionary = source as IDictionary;
            if (dictionary != null)
            {
                if (dictionary.Contains(Key))
                {
                    var tvalue = dictionary[Key].ToString();

                    if (tvalue.Length > 0)
                    {
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
