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
            if (resources.Count == 0) resources = GetResources(source.ToString(), true);
            foreach(var resourceName in resources.Keys)
            {
                var resource = resources[resourceName];
                if(resource != null)
                {
                    if (type.IsAssignableFrom(resource.GetType())) return resource;
                }
            }
            return null;
        }

        private IDictionary GetResources(string name, bool partial_match = false)
        {
            var results = new Dictionary<string, dynamic>();
            foreach (var assembly in ResourceAssemblies)
            {
                foreach (var manifestResourceName in assembly.GetManifestResourceNames())
                {
                    if (!results.ContainsKey(manifestResourceName))
                    {
                        if (partial_match && name.Length > 0)
                        {
                            if (manifestResourceName.Contains(name)) results.Add(manifestResourceName, Load(assembly, manifestResourceName));
                        }
                        else
                        {
                            if (manifestResourceName == name) results.Add(manifestResourceName,Load(assembly, manifestResourceName));
                        }
                    }
                }
            }
            return results;
        }

        private static object Load(Assembly assembly,string manifestResourceName)
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
