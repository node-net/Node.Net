using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Node.Net.Beta
{
    public sealed class Factory : IFactory
    {
        public T Create<T>() => Internal.IFactoryExtension.Create<T>(this);
        public T Create<T>(object source) => Internal.IFactoryExtension.Create<T>(this, source);
        public object Create(Type targetType, object source) => factory.Create(targetType, source);
        public ResourceDictionary Resources
        {
            get { return factory.Resources; }
            set { factory.Resources = value; }
        }
        public Dictionary<Type, Func<Type, object, object>> FactoryFunctions
        {
            get { return factory.FactoryFunctions; }
        }
        public List<Assembly> ManifestResourceAssemblies
        {
            get { return factory.ManifestResourceAssemblies; }
            set { factory.ManifestResourceAssemblies = value; }
        }
        public Dictionary<string, Type> IDictionaryTypes { get { return factory.IDictionaryTypes; } }
        public Dictionary<Type, Type> AbstractTypes
        {
            get { return factory.AbstractTypes; }
            set
            {
                factory.AbstractTypes.Clear();
                foreach (var type in value.Keys)
                {
                    factory.AbstractTypes[type] = value[type];
                }
            }
        }
        public Func<Stream, object> ReadFunction
        {
            get { return factory.ReadFunction; }
            set { factory.ReadFunction = value; }
        }
        private Internal.Factories.Factory factory = new Internal.Factories.Factory();
    }
}
