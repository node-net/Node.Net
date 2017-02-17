using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Beta.Internal.Factories
{
    sealed class StringFactory : IFactory
    {
        public object Create(Type target_type, object source)
        {
            if (source != null)
            {
                if (typeof(IDictionary).IsAssignableFrom(source.GetType())) return Internal.Writers.JSONWriter.Default.WriteToString(source);
                if (typeof(IEnumerable).IsAssignableFrom(source.GetType())) return Internal.Writers.JSONWriter.Default.WriteToString(source);
            }
            return null;
        }
    }
}
