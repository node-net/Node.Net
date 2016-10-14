using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Deprecated
{
    public sealed class RenderFactory : IFactory
    {
        public Dictionary<string, ResourceFactory> ResourceFactories = new Dictionary<string, ResourceFactory>();
        public object Create(Type targetType,object source,IFactory helper)
        {
            return null;
        }
    }
}
