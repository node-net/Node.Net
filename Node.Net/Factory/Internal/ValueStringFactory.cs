using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory.Internal
{
    class ValueStringFactory : IFactory
    {
        public ResourceFactory ResourceFactory = new ResourceFactory();
        public object Create(Type type, object value)
        {
            return Create(type, value.ToString());
        }
        public object Create(Type type, string name)
        {

            return ResourceFactory.Create(type,name);
        }
    }
}
