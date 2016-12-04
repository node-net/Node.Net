//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections;
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
            Node.Net.Factories.MetaDataMap.GetMetaDataFunction = Node.Net.Collections.GlobalFunctions.GetMetaDataFunction;
            Node.Net.Collections.GlobalFunctions.GetLocalToParentFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToParent;
            Node.Net.Collections.GlobalFunctions.GetLocalToWorldFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToWorld;

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
                var localToParent = Node.Net.Factories.Deprecated.Factory.Default.Create(typeof(Node.Net.Factories.Deprecated.ILocalToParent), source,null) as Node.Net.Factories.Deprecated.ILocalToParent;
                return new ConcreteLocalToParent { LocalToParent = localToParent.LocalToParent };
            }
            if (targetType == typeof(ILocalToWorld))
            {
                var localToWorld = Node.Net.Factories.Deprecated.Factory.Default.Create(typeof(Node.Net.Factories.Deprecated.ILocalToWorld), source,null) as Node.Net.Factories.Deprecated.ILocalToWorld;
                return new ConcreteLocalToWorld { LocalToWorld = localToWorld.LocalToWorld };
            }
            var result = factory.Create(targetType, source,null);
            var idictionary = result as IDictionary;
            if (idictionary != null)
            {
                idictionary.DeepUpdateParents();
                //idictionary.DeepCollect<IDictionary>();
            }
            return result;
        }

        public T Create<T>()
        {
            var instance = Activator.CreateInstance<T>();
            var idictionary = instance as IDictionary;
            if(idictionary != null)
            {
                if(!idictionary.Contains("Type"))
                {
                    idictionary["Type"] = instance.GetType().Name;
                }
            }
            return (T)instance;
        }
        private readonly Node.Net.Reader reader = new Reader();
        private Node.Net.Factories.Deprecated.Factory _factory;
        private Node.Net.Factories.Deprecated.Factory factory
        {
            get
            {
                if(_factory == null)
                {
                    _factory = new Factories.Deprecated.Factory { ReadFunction = reader.Read };
                }
                return _factory;
            }
        }
    }
}
