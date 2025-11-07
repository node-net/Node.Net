using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if IS_WINDOWS
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
#endif

namespace Node.Net.Internal
{
    internal class Factory : IFactory
    {
        public static Factory Default { get; } = new Factory();

        public Factory()
        {
#if IS_WINDOWS
            Model3DFactory.ParentFactory = this;
            AbstractFactory.ParentFactory = this;
#endif
            CollectionsFactory.StreamFactory = StreamFactory;
            FactoryFunctions = new Dictionary<Type, Func<Type, object, object>>
            {
                {typeof(Stream), StreamFactory.Create },
                {typeof(IStreamSignature),StreamFactory.Create },
#if IS_WINDOWS
                {typeof(Color),new ColorFactory().Create },
                {typeof(Brush),new BrushFactory {ParentFactory=this}.Create },
                {typeof(Material),new MaterialFactory {ParentFactory=this}.Create },
                {typeof(Matrix3D),new Matrix3DFactory().Create },
                {typeof(Transform3D), new Transform3DFactory {ParentFactory = this }.Create },
                {typeof(MeshGeometry3D),new MeshGeometry3DFactory {ParentFactory=this }.Create },
                {typeof(GeometryModel3D),new GeometryModel3DFactory {ParentFactory=this }.Create },
                {typeof(Model3D),Model3DFactory.Create },
                {typeof(Visual3D), new Visual3DFactory {ParentFactory = this }.Create },
                {typeof(object), AbstractFactory.Create }
#else
                {typeof(object), CollectionsFactory.Create }
#endif
            };
#if IS_WINDOWS
            Resources = new ResourceDictionary();
#endif
        }

#if IS_WINDOWS
        public ResourceDictionary Resources
        {
            get { return AbstractFactory.Resources; }
            set { AbstractFactory.Resources = value; }
        }

        public Dictionary<object, Model3D> Model3DCache
        {
            get { return Model3DFactory.Model3DCache; }
            set { Model3DFactory.Model3DCache = value; }
        }

        public List<Type> Model3DIgnoreTypes
        {
            get { return Model3DFactory.IgnoreTypes; }
        }
#endif

        public Dictionary<Type, Func<Type, object, object>> FactoryFunctions { get; }

        public List<Assembly> ManifestResourceAssemblies
        {
            get { return StreamFactory.ResourceAssemblies; }
            set { StreamFactory.ResourceAssemblies = value; }
        }

        public CollectionsFactory CollectionsFactory { get; } = new CollectionsFactory();

#if IS_WINDOWS
        public Func<Stream, object> ReadFunction
        {
            get { return AbstractFactory.ReadFunction; }
            set { AbstractFactory.ReadFunction = value; }
        }

        public Dictionary<Type, Type> AbstractTypes { get { return AbstractFactory; } }

        public Dictionary<string, Type> IDictionaryTypes
        {
            get { return AbstractFactory.IDictionaryTypes; }
            set { AbstractFactory.IDictionaryTypes = value; }
        }

        public Type DefaultObjectType
        {
            get { return AbstractFactory.DefaultObjectType; }
            set { AbstractFactory.DefaultObjectType = value; }
        }
#else
        public Func<Stream, object> ReadFunction { get; set; } = new Internal.JsonReader().Read;

        public Dictionary<Type, Type> AbstractTypes { get; } = new Dictionary<Type, Type>();

        public Dictionary<string, Type> IDictionaryTypes { get; set; } = new Dictionary<string, Type>();

        public Type DefaultObjectType { get; set; } = typeof(Dictionary<string, dynamic>);
#endif

#if IS_WINDOWS
        public Func<IDictionary, Model3D> PrimaryModel3DHelperFunction
        {
            get { return Model3DFactory.PrimaryModel3DHelperFunction; }
            set { Model3DFactory.PrimaryModel3DHelperFunction = value; }
        }

        public bool ScalePrimaryModel3D
        {
            get { return Model3DFactory.ScalePrimaryModel; }
            set { Model3DFactory.ScalePrimaryModel = value; }
        }
#endif

        public Dictionary<Type, int> InstanceCounts { get; } = new Dictionary<Type, int>();

        public object Create(Type targetType, object source)
        {
            StreamFactory.Refresh();
#if IS_WINDOWS
            if (source != null && Resources.Contains(source))
            {
                object? instance = Resources[source];
                if (targetType.IsInstanceOfType(instance))
                {
                    if (!InstanceCounts.ContainsKey(targetType))
                    {
                        InstanceCounts.Add(targetType, 1);
                    }
                    else
                    {
                        InstanceCounts[targetType]++;
                    }

                    return instance;
                }
            }
#endif

            foreach (Type? type in FactoryFunctions.Keys)
            {
                if (type.IsAssignableFrom(targetType))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    object? instance = FactoryFunctions[type](targetType, source);
#pragma warning restore CS8604 // Possible null reference argument.
                    if (instance != null)
                    {
                        if (!InstanceCounts.ContainsKey(targetType))
                        {
                            InstanceCounts.Add(targetType, 1);
                        }
                        else
                        {
                            InstanceCounts[targetType]++;
                        }

#if IS_WINDOWS
                        if (source != null && (source is string) && IsResourceType(targetType) && !Resources.Contains(source.ToString()))
                        {
                            Resources.Add(source.ToString(), instance);
                        }
#endif
                        return instance;
                    }
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

#if IS_WINDOWS
        public bool IsResourceType(Type type)
        {
            if (typeof(Model3D).IsAssignableFrom(type))
            {
                return true;
            }

            if (typeof(MeshGeometry3D).IsAssignableFrom(type))
            {
                return true;
            }

            return false;
        }

        public bool Cache
        {
            get { return Model3DFactory.Cache; }
            set { Model3DFactory.Cache = value; }
        }

        public void ClearCache()
        {
            Model3DFactory.ClearCache();
        }

        public void ClearCache(object model)
        {
            Model3DFactory.ClearCache(model);
        }
#endif

        public bool Logging { get; set; } = false;
        public Action<string> LogFunction { get; set; }

        private StreamFactory StreamFactory { get; } = new StreamFactory();
#if IS_WINDOWS
        private Model3DFactory Model3DFactory { get; } = new Model3DFactory();

        private AbstractFactory AbstractFactory { get; } = new AbstractFactory
        {
            {typeof(IDictionary),typeof(Dictionary<string,dynamic>) },
            {typeof(IList),typeof(List<dynamic>) }
        };
#endif
    }
}