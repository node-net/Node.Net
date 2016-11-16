using System;
using System.IO;
using System.Reflection;
using System.Windows.Markup;

namespace Node.Net.Repositories
{
    public class AssemblyRepository
    {
        public Assembly Assembly { get; set; }
        public Func<Stream, object> ReadFunction { get; set; } = XamlReader.Load;
        public object Get(string name)
        {
            if (Assembly == null) return null;
            if (ReadFunction == null) return null;
            foreach (var resourceName in Assembly.GetManifestResourceNames())
            {
                if (resourceName == name) return ReadFunction(Assembly.GetManifestResourceStream(resourceName));
            }
            return null;
        }
    }
}
