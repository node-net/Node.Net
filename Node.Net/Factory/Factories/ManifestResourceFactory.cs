using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Markup;

namespace Node.Net.Factory.Factories
{
    public class ManifestResourceFactory : IFactory
    {
        public List<Assembly> Assemblies = new List<Assembly>();
        public Func<Stream, object> ReadFunction;

        public object Create(Type targetType, object source)
        {
            if (source == null) return null;
            foreach (var assembly in Assemblies)
            {
                var resources = GetResources(source.ToString());
                foreach (var resourceName in resources.Keys)
                {
                    var resource = resources[resourceName];
                    if (resource != null && targetType.IsAssignableFrom(resource.GetType()))
                    {
                        return resource;
                    }
                }
            }
            return null;
        }

        private IDictionary GetResources(string name)
        {
            var dictionary = new Dictionary<string, dynamic>();
            foreach (var assembly in Assemblies)
            {
                foreach (var resourceName in assembly.GetManifestResourceNames())
                {
                    if (!dictionary.ContainsKey(resourceName) && resourceName == name)
                    {
                        dictionary.Add(resourceName, LoadResource(resourceName));
                    }
                }
            }
            if (dictionary.Count == 0)
            {
                foreach (var resourceName in GetPartialMatchResourceNames(name))
                {
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        dictionary.Add(resourceName, LoadResource(resourceName));
                    }
                }
            }

            return dictionary;
        }

        private string[] GetPartialMatchResourceNames(string name)
        {
            var results = new List<string>();
            if (name.Length == 0) return results.ToArray();
            foreach (var assembly in Assemblies)
            {
                foreach (var resourceName in assembly.GetManifestResourceNames())
                {
                    if (resourceName.Contains(name) && !results.Contains(resourceName))
                    {
                        results.Add(resourceName);
                    }
                }
            }
            return results.ToArray();
        }

        private object LoadResource(string resourceName)
        {
            foreach (var assembly in Assemblies)
            {
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    if (ReadFunction != null) return ReadFunction(stream);
                    return XamlReader.Load(stream);
                }
            }
            return null;
        }
    }
}
