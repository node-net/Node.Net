using System;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    public class TypeFilter : IFilter
    {
        public List<Type> Types { get; set; } = new List<Type>();
        public bool? Include(object value)
        {
            if (value == null) return false;
            if (Types != null)
            {
                foreach (var type in Types)
                {
                    if (!type.IsAssignableFrom(value.GetType())) return false;
                }
            }
            return null;
        }
    }
}
