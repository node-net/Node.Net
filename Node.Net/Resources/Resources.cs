using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Node.Net.Resources
{
    public class Resources : ResourceDictionary, IResources
    {
        public Resources()
        {

        }
        public Resources(ResourceDictionary dictionary)
        {
            foreach(var key in dictionary.Keys)
            {
                Add(key, dictionary[key]);
            }
        }
        public readonly List<IResources> Children = new List<IResources>();
        public virtual object GetResource(string name)
        {
            object resource=null;
            if (Contains(name)) resource =this[name];
            if (resource != null) return resource;
            foreach (var child in Children)
            {
                resource = child.GetResource(name);
                if (resource != null) return resource;
            }
            return GetDynamicResource(name);
        }

        protected virtual object GetDynamicResource(string name)
        {
            object resource=null;
            foreach (var child in Children)
            {
                resource = child.GetResource(name);
                if (resource != null) return resource;
            }
            return resource;
        }

        public void ImportManifestResources<T>(Assembly assembly,string pattern)
        {
            foreach(var manifest_resource_name in assembly.GetManifestResourceNames())
            {
                if(manifest_resource_name.Contains(pattern))
                {
                    T item = (T)XamlReader.Load(assembly.GetManifestResourceStream(manifest_resource_name));
                    if(item != null)
                    {
                        Add(manifest_resource_name, item);
                    }
                }
            }
        }
    }
}
