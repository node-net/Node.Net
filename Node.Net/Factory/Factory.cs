using System;
using System.Collections.Generic;

namespace Node.Net.Factory
{
    public class Factory : Dictionary<string,IFactory>, ICompositeFactory
    {
        public static Factory Default { get; set; } = new Factories.DefaultFactory();
        public Factory() { }
        public IFactory Parent { get; set; }

        public object Create(Type targetType, object value)
        {
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
