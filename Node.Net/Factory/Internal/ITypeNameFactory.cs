using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory.Internal
{
    class ConcreteTypeName : ITypeName
    {
        public string TypeName { get; set; }
    }
    class ITypeNameFactory  :IFactory
    {
        public object Create(Type type,object source)
        {
            return new ConcreteTypeName { TypeName = GetTypeName(source) };
        }

        private static string GetTypeName(object source)
        {
            if (source == null) return string.Empty;
            var dictionary = source as IDictionary;
            if(dictionary != null && dictionary.Contains("Type") && dictionary["Type"] != null)
            {
                return dictionary["Type"].ToString();
            }
            return source.GetType().Name;
        }
    }
}
