using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta.Internal.Factories
{
    class DependencyObjectFactory : IFactory
    {
        public IFactory ParentFactory { get; set; }
        public DependencyObjectFactory()
        {
            FactoryFunctions = new Dictionary<Type, Func<Type, object, object>>
            {
                {typeof(Stream), StreamFactory.Create },
                {typeof(String),new StringFactory().Create },
                {typeof(Color),new ColorFactory().Create },
                {typeof(Brush),new BrushFactory {ParentFactory=this }.Create },
                {typeof(Matrix3D),new Matrix3DFactory().Create },
                {typeof(Transform3D), new Transform3DFactory {ParentFactory = this }.Create },
                {typeof(Visual3D), new Visual3DFactory {ParentFactory = this }.Create },
                {typeof(IReadOnlyDocument), CollectionsFactory.Create },
                {typeof(object), AbstractFactory.Create }
            };
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

        public ResourceDictionary Resources { get; set; } = new ResourceDictionary();
        public Dictionary<Type, Func<Type, object, object>> FactoryFunctions { get; private set; }
        private StreamFactory StreamFactory { get; } = new StreamFactory();
        public List<Assembly> ManifestResourceAssemblies
        {
            get { return StreamFactory.ResourceAssemblies; }
            set { StreamFactory.ResourceAssemblies = value; }
        }
        public CollectionsFactory CollectionsFactory { get; } = new CollectionsFactory();
        public Func<Stream, object> ReadFunction
        {
            get { return AbstractFactory.ReadFunction; }
            set { AbstractFactory.ReadFunction = value; }
        }
        public Dictionary<Type, Type> AbstractTypes { get { return AbstractFactory; } }
        private AbstractFactory AbstractFactory { get; } = new AbstractFactory();
        public Dictionary<string, Type> IDictionaryTypes { get { return AbstractFactory.IDictionaryTypes; } }
    }
}
