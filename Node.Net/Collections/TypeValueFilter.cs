using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections
{
    public class TypeValueFilter : IFilter
    {
        private string _typeValue = "";
        public TypeValueFilter(string typeValue)
        {
            _typeValue = typeValue;
        }
        public bool Include(object value)
        {
            var dictionary = value as IDictionary;
            if(dictionary != null)
            {
                if(dictionary.Contains("Type"))
                {
                    var tvalue = dictionary["Type"];
                    if (tvalue != null)
                    {
                        if (tvalue.ToString() == _typeValue) return true;
                    }
                }
            }
            return false;
        }
    }
}
