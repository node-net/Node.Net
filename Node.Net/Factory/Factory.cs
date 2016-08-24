using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Factory
{
    public class Factory : Dictionary<string,IFactory>, ICompositeFactory
    {
        private Factories.TypeFactories.StreamFactory streamFactory = null;
        public Factory() { }
        public Factory(Func<Stream,object> read_function,Assembly resource_assembly)
        {
            streamFactory = new Factories.TypeFactories.StreamFactory(read_function, null, resource_assembly);
        }
        public IFactory Parent { get; set; }

        public object Create(Type targetType, object value)
        {
            //streamSourceFactory.ReadFunction = ReadFunction;
            //var item = streamSourceFactory.Create(targetType, value);
            //if (item != null && targetType.IsAssignableFrom(item.GetType())) return item;

            foreach(var key in Keys)
            {
                var result = this[key].Create(targetType, value);
                if(result != null)
                {
                    if (targetType.IsAssignableFrom(result.GetType())) return result;
                }
            }

            return null;
        }
    }
}
