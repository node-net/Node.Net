//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
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
            try
            {
                object resource = null;
                foreach (var child in Children)
                {
                    resource = child.GetResource(name);
                    if (resource != null) return resource;
                }
                return resource;
            }
            catch(Exception exception)
            {
                throw new Exception($"{GetType().FullName}.GetDynamicResource('{name}')", exception);
            }
        }

        public void ImportManifestResources<T>(Type type, string pattern, KeyValuePair<string, string>[] searchReplacePatterns = null)
        {
            var results = CollectManifestResources<T>(type, pattern);
            foreach(string key in results.Keys)
            {
                var name = key;
                if (searchReplacePatterns != null)
                {
                    foreach(var kvp in searchReplacePatterns)
                    {
                        name = name.Replace(kvp.Key, kvp.Value);
                    }
                }
                Add(name, results[key]);
            }
        }

        public static Dictionary<string, T> CollectManifestResources<T>(Type type, string pattern)
        {
            var results = new Dictionary<string, T>();
            foreach (var manifest_resource_name in type.Assembly.GetManifestResourceNames())
            {
                if (manifest_resource_name.Contains(pattern))
                {
                    var item = (T)Reader.Default.Read(type.GetStream(manifest_resource_name));
                    if (item != null)
                    {
                        results.Add(manifest_resource_name, item);
                    }
                }
            }
            return results;
        }
    }
}
