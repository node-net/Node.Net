﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    class Factory : IFactory
    {
        public static Factory Default { get; } = new Factory();
        public Factory()
        {
            CollectionsFactory.StreamFactory = StreamFactory;
            AbstractFactory.ParentFactory = this;
            FactoryFunctions = new Dictionary<Type, Func<Type, object, object>>
            {
                
                {typeof(String),new StringFactory().Create },
                {typeof(Color),new ColorFactory().Create },
                {typeof(Brush),new BrushFactory {ParentFactory=this }.Create },
                {typeof(Matrix3D),new Matrix3DFactory().Create },
                {typeof(Transform3D), new Transform3DFactory {ParentFactory = this }.Create },
                {typeof(MeshGeometry3D),new MeshGeometry3DFactory {ParentFactory=this }.Create },
                {typeof(Visual3D), new Visual3DFactory {ParentFactory = this }.Create },
                {typeof(IReadOnlyDocument), CollectionsFactory.Create },
                {typeof(object), AbstractFactory.Create },
                {typeof(Stream), StreamFactory.Create }
            };
        }
        public ResourceDictionary Resources
        {
            get { return AbstractFactory.Resources; }
            set { AbstractFactory.Resources = value; }
        }
        //private ResourceFactory ResourceFactory = new ResourceFactory();
        public Dictionary<Type, Func<Type, object, object>> FactoryFunctions { get; private set; }
        private StreamFactory StreamFactory { get; } = new StreamFactory();
        public List<Assembly> ManifestResourceAssemblies
        {
            get { return StreamFactory.ResourceAssemblies; }
            set { StreamFactory.ResourceAssemblies = value; }
        }
        public CollectionsFactory CollectionsFactory { get; } = new CollectionsFactory();
        public Func<Stream,object> ReadFunction
        {
            get { return AbstractFactory.ReadFunction; }
            set { AbstractFactory.ReadFunction = value; }
        }
        public Dictionary<Type,Type> AbstractTypes { get { return AbstractFactory; } }
        private AbstractFactory AbstractFactory { get; } = new AbstractFactory();
        public Dictionary<string,Type> IDictionaryTypes { get { return AbstractFactory.IDictionaryTypes; } }
        public object Create(Type target_type, object source)
        {
            foreach (var type in FactoryFunctions.Keys)
            {
                if (type.IsAssignableFrom(target_type))
                {
                    var instance = FactoryFunctions[type](target_type, source);
                    if (instance != null) return instance;
                }
            }
            if (source != null && Resources.Contains(source)) return Resources[source];
            return null;
        }
    }
}
