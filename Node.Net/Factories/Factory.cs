using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Node.Net.Factories.Factories.Generic;
using Node.Net.Factories.Factories.Helpers;
using System.Windows.Media;

namespace Node.Net.Factories
{
    public sealed class Factory : Factories.CompositeFactory
    {
        public Factory() {}
        public Factory(Assembly resource_assembly) { ResourceAssemblies.Add(resource_assembly); }

        public Func<string, object> GetFunction
        {
            get { return objectFromString.GetFunction; }
            set { objectFromString.GetFunction = value; }
        }

        public Func<Stream, object> ReadFunction
        {
            get { return objectFromString.ReadFunction; }
            set { objectFromString.ReadFunction = value; }
        }

        public List<Assembly> ResourceAssemblies
        {
            get { return objectFromString.ResourceAssemblies; }
            set { objectFromString.ResourceAssemblies = value; }
        }

        private readonly Factories.TypeSourceFactories.ObjectFromString objectFromString = new Factories.TypeSourceFactories.ObjectFromString();
        public override object Create(Type targetType, object source)
        {
            var instance = base.Create(targetType, source);
            if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;

            if (source != null && source.GetType() == typeof(string))
            {
                instance = objectFromString.Create(targetType, source.ToString());
                if (instance != null && targetType.IsAssignableFrom(instance.GetType())) return instance;

                // Partial matches
                return objectFromString.CreatePartialMatch(targetType, source.ToString());
            }
            return null;
        }
        private static IFactory _default;
        public static IFactory Default
        {
            get
            {
                if(_default == null)
                {
                    _default = Factories.Helpers.IFactoryHelper.CreateDefaultFactory();
                }
                return _default;
            }
        }
    }
}
