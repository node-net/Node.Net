using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Node.Net.Factories
{
    public sealed class ResourceFactory : ResourceDictionary, IFactory
    {
        public Func<Stream, object> ReadFunction { get; set; } = DefaultRead;
        public bool RequireExactMatch { get; set; } = false;
        public object Create(Type targetType, object source)
        {
            if(source != null && source.GetType() == typeof(string))
            {
                return CreateFromString(targetType,source.ToString());
            }
            return null;
        }

        private object CreateFromString(Type targetType,string name)
        {
            object instance = null;
            if(Contains(name))
            {
                instance = this[name];
                if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;
                instance = null;
            }
            if (!RequireExactMatch)
            {
                foreach (string key in Keys)
                {
                    if (key.Contains(name))
                    {
                        instance = this[key];
                        if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;
                    }
                }
            }
            return null;
        }

        public void ImportResources(ResourceDictionary resourceDictionary)
        {
            foreach(string key in resourceDictionary.Keys)
            {
                if (!Contains(key)) Add(key, resourceDictionary[key]);
            }
        }
        public void ImportManifestResources(Assembly assembly)
        {
            foreach(var manifestResourceName in assembly.GetManifestResourceNames())
            {
                try
                {
                    var instance = ReadFunction(assembly.GetManifestResourceStream(manifestResourceName));
                    if(instance != null)
                    {
                        Add(manifestResourceName, instance);
                    }
                }
                catch { }
            }
        }

        public static object DefaultRead(Stream stream)
        {
            return XamlReader.Load(stream);
        }
    }
}
