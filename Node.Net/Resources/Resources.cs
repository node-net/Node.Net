using System;
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

        public void ImportManifestResources<T>(Type type, string pattern, KeyValuePair<string, string>[] searchReplacePatterns = null)
        {
            var results = Extensions.TypeExtension.CollectManifestResources<T>(type, pattern);
            foreach(string key in results.Keys)
            {
                string name = key;
                if(searchReplacePatterns != null)
                {
                    foreach(var kvp in searchReplacePatterns)
                    {
                        name = name.Replace(kvp.Key, kvp.Value);
                    }
                }
                Add(name, results[key]);
            }
        }
    }
}
