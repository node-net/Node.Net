using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Filters
{
    public class TypeFilter : IFilter
    {
        public TypeFilter() { }
        public TypeFilter(string type)
        {
            TypeValues.Add(type);
        }
        public List<string> TypeValues = new List<string>();
        public List<string> TypePatterns = new List<string>();
        public bool Include(object value)
        {
            var dictionary = value as IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                if (dictionary.Contains("Type"))
                {
                    var stype = dictionary["Type"].ToString();
                    foreach (string v in TypeValues)
                    {
                        if (stype == v) return true;
                    }
                    foreach (string pattern in TypePatterns)
                    {
                        if (stype.Contains(pattern)) return true;
                    }
                }
            }
            return false;
        }
    }
}
