using System;
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
            Model3DFactory.ParentFactory = this;
            CollectionsFactory.StreamFactory = StreamFactory;
            AbstractFactory.ParentFactory = this;
            FactoryFunctions = new Dictionary<Type, Func<Type, object, object>>
            {
                {typeof(Stream), StreamFactory.Create },
                {typeof(IStreamSignature),StreamFactory.Create },
                {typeof(String),new StringFactory().Create },
                {typeof(double),new DoubleFactory().Create },
                {typeof(Color),new ColorFactory().Create },
                {typeof(Brush),new BrushFactory {ParentFactory=this }.Create },
                {typeof(Matrix3D),new Matrix3DFactory().Create },
                {typeof(Transform3D), new Transform3DFactory {ParentFactory = this }.Create },
                {typeof(MeshGeometry3D),new MeshGeometry3DFactory {ParentFactory=this }.Create },
                {typeof(GeometryModel3D),new GeometryModel3DFactory {ParentFactory=this }.Create },
                {typeof(Model3D),Model3DFactory.Create },
                {typeof(Visual3D), new Visual3DFactory {ParentFactory = this }.Create },
                {typeof(IReadOnlyDocument), CollectionsFactory.Create },
                {typeof(object), AbstractFactory.Create }
                
            };
        }
        public ResourceDictionary Resources
        {
            get { return AbstractFactory.Resources; }
            set { AbstractFactory.Resources = value; }
        }
        public Dictionary<Type, Func<Type, object, object>> FactoryFunctions { get; private set; }
        
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
        public Dictionary<string,Type> IDictionaryTypes
        {
            get { return AbstractFactory.IDictionaryTypes; }
            set { AbstractFactory.IDictionaryTypes = value; }
        }
        public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction
        {
            get { return Model3DFactory.PrimaryModel3DHelperFunction; }
            set { Model3DFactory.PrimaryModel3DHelperFunction = value; }
        }
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

        private Model3DFactory Model3DFactory { get; } = new Model3DFactory();
        private StreamFactory StreamFactory { get; } = new StreamFactory();
        private AbstractFactory AbstractFactory { get; } = new AbstractFactory();
    }
}
