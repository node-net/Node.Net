using System;
using System.Collections;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class StringFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return JSONWriter.Default.WriteToString(source);
                if (typeof(IEnumerable).IsAssignableFrom(source.GetType())) return JSONWriter.Default.WriteToString(source);
            }
            return null;
        }
    }
}
