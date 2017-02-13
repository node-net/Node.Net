using System;
using System.IO;

namespace Node.Net.Factories
{
    public sealed class StandardFactory : Factory
    {
        public StandardFactory()
        {
            Add("Resources", ResourceFactory);
            Add("ManifestResources", ManifestResourceFactory);
            Add("Abstract", AbstractFactory);
            Add("Color", new ColorFactory());
            Add("Brush", new BrushFactory());
            Add("Material", new MaterialFactory());
            Add("MeshGeometry3D", new MeshGeometry3DFactory());
            Add("Model3D", Model3DFactory);
            Add("Visual3D", Visual3DFactory);
            Add("GeometryModel3D", new GeometryModel3DFactory());
            Add("Transform3D", new Transform3DFactory());
        }

        public bool Cache
        {
            get { return Visual3DFactory.Cache; }
            set { Visual3DFactory.Cache = value; }
        }
        public Func<Stream, object> ReadFunction
        {
            get { return ResourceFactory.ReadFunction; }
            set
            {
                ResourceFactory.ReadFunction = value;
                ManifestResourceFactory.ReadFunction = value;
            }
        }
        public ResourceFactory ResourceFactory { get; } = new ResourceFactory();
        public ManifestResourceFactory ManifestResourceFactory
        {
            get
            {
                if(manifestResourceFactory == null)
                {
                    manifestResourceFactory = new ManifestResourceFactory { ReadFunction = ReadFunction };
                }
                return manifestResourceFactory;
            }
        }
        private ManifestResourceFactory manifestResourceFactory;
        public Model3DFactory Model3DFactory { get; } = new Model3DFactory();
        public Visual3DFactory Visual3DFactory { get; } = new Visual3DFactory();
        public AbstractFactory AbstractFactory { get; } = new AbstractFactory();
    }
}
