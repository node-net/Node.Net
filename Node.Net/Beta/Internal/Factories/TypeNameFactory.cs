using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class TypeNameFactory : Dictionary<string, Type>, IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (source != null && typeof(IDictionary).IsAssignableFrom(source.GetType()))
            {

            }
            return null;
        }
    }
}
