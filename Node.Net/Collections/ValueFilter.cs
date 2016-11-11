using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class ValueFilter : IFilter
    {
        public bool ExactMatch { get; set; } = false;
        public List<string> Values { get; set; } = new List<string>();
        public List<string> ExcludeValues { get; set; } = new List<string>();
        public bool? Include(object item)
        {
            var dictionary = item as IDictionary;
            if (dictionary != null)
            {
                foreach (var exclude_value in ExcludeValues)
                {
                    foreach (var key in dictionary.Keys)
                    {
                        var value = dictionary[key];
                        if (value != null)
                        {
                            if (ExactMatch)
                            {
                                if (exclude_value == value.ToString()) return false;
                            }
                            else
                            {
                                if (value.ToString().Contains(exclude_value)) return false;
                            }
                        }
                    }
                }
                foreach(var include_value in Values)
                {
                    foreach(var key in dictionary.Keys)
                    {
                        var value = dictionary[key];
                        if(value != null)
                        {
                            if (ExactMatch)
                            {
                                if (include_value == value.ToString()) return true;
                            }
                            else
                            {
                                if (value.ToString().Contains(include_value)) return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
