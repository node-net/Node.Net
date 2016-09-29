using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public sealed class Factory : IFactory
    {
        static Factory()
        {
            Node.Net.Factories.MetaDataMap.GetMetaDataFunction = Node.Net.Collections.MetaDataMap.GetMetaDataFunction;
        }
        public Factory() { }
        public Factory(Assembly assembly)
        {
            factory.ResourceAssemblies.Add(assembly);
        }

        public Func<string, object> GetFunction
        {
            get { return factory.GetFunction; }
            set { factory.GetFunction = value; }
        }

        public Func<Stream, object> ReadFunction
        {
            get { return factory.ReadFunction; }
            set { factory.ReadFunction = value; }
        }

        public List<Assembly> ResourceAssemblies
        {
            get { return factory.ResourceAssemblies; }
            set { factory.ResourceAssemblies = value; }
        }

        class ConcreteLocalToParent : ILocalToParent { public Matrix3D LocalToParent { get; set; } = new Matrix3D(); }
        class ConcreteLocalToWorld : ILocalToWorld { public Matrix3D LocalToWorld { get; set; } = new Matrix3D(); }
        public object Create(Type targetType, object source)
        {
            if(targetType == typeof(ILocalToParent))
            {
                var localToParent = Node.Net.Factories.Factory.Default.Create(typeof(Node.Net.Factories.ILocalToParent), source) as Node.Net.Factories.ILocalToParent;
                return new ConcreteLocalToParent { LocalToParent = localToParent.LocalToParent };
            }
            if (targetType == typeof(ILocalToWorld))
            {
                var localToWorld = Node.Net.Factories.Factory.Default.Create(typeof(Node.Net.Factories.ILocalToWorld), source) as Node.Net.Factories.ILocalToWorld;
                return new ConcreteLocalToWorld { LocalToWorld = localToWorld.LocalToWorld };
            }
            return factory.Create(targetType, source);
        }

        private readonly Node.Net.Reader reader = new Reader();
        private Node.Net.Factories.Factory _factory;
        private Node.Net.Factories.Factory factory
        {
            get
            {
                if(_factory == null)
                {
                    _factory = new Factories.Factory { ReadFunction = reader.Read };
                }
                return _factory;
            }
        }
    }
}
