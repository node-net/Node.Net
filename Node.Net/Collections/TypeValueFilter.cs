using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class TypeValueFilter : KeyValueFilter
    {
        public TypeValueFilter()
        {
            Key = "Type";
        }
        public TypeValueFilter(string value)
        {
            Key = "Type";
            Values.Add(value);
        }
    }
}
