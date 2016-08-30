using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    // TODO: rename Node.Net.Factory to Node.Net.Factories
    public class Factory : IFactory
    {
        public Factory() { }
        public Factory(Assembly assembly)
        {
            factory.ResourceAssemblies.Add(assembly);
        }

        private Node.Net.Factories.Factory factory = new Factories.Factory { ReadFunction = Node.Net.Data.Readers.Reader.Default.Read };
        public object Create(Type targetType,object source)
        {
            return factory.Create(targetType,source);
        }
    }
}
