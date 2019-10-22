using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Collections.Internal
{
    public class TypeFilter : IFilter
    {
        public string Type { get; set; } = "";
        public bool Exact { get; set; } = true;
        public bool? Include(object value)
        {
            if (value == null) return false;
            var idictionary = value as IDictionary;
            if(idictionary != null)
            {
                if(idictionary.Contains("Type") && 
                    idictionary["Type"] != null && 
                    idictionary["Type"].GetType() == typeof(string))
                {
                    if(Exact) return (Type != (string)idictionary["Type"]) ? false : true;
                    else
                    {
                        if (((string)idictionary["Type"]).Contains(Type)) return true;
                    }
                }
            }
           
            return false;
        }
    }
}
