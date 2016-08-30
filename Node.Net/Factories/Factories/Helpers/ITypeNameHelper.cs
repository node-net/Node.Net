using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Factories.Helpers
{
    public static class ITypeNameHelper
    {
        class ConcreteTypeName : ITypeName { public string TypeName { get; set; } = string.Empty; }
        public static ITypeName FromIDictionary(IDictionary dictionary)
        {
            if (dictionary != null)
            {
                if (dictionary.Contains("Type") && dictionary["Type"] != null)
                {
                    //concreteTypeName.TypeName = dictionary["Type"].ToString();
                    return new ConcreteTypeName { TypeName = dictionary["Type"].ToString() };
                }
            }
            return null;
        }
    }
}
