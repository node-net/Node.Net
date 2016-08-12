using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Factory.Internal
{
    class ResourceFactory : IFactory
    {
        public List<Assembly> ResourceAssemblies = new List<Assembly>();
        public object Create(Type type,object source)
        {
            if (source == null) return null;
            var resources = GetResources(source.ToString());
            if (resources.Length == 0) resources = GetResources(source.ToString(), true);
            foreach(var resource in resources)
            {
                if(resource != null)
                {
                    if (type.IsAssignableFrom(resource.GetType())) return resource;
                }
            }
            return null;
        }

        private object[] GetResources(string name, bool partial_match = false)
        {
            var results = new List<object>();
            foreach (var assembly in ResourceAssemblies)
            {
                foreach (var manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (partial_match)
                    {
                        if (manifestResourceName.Contains(name)) results.Add(Load(assembly,manifestResourceName));
                    }
                    else
                    {
                        if (manifestResourceName == name) results.Add(Load(assembly,manifestResourceName));
                    }
                }
            }
            return results.ToArray();
        }

        private object Load(Assembly assembly,string manifestResourceName)
        {
            try
            {
                var result = XamlReader.Load(assembly.GetManifestResourceStream(manifestResourceName));
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
