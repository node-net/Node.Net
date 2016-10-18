namespace Node.Net.Factories
{
    public sealed class StandardFactory : Factory
    {
        public StandardFactory()
        {
            Add("Resources", ResourceFactory);
            Add("Color", new ColorFactory());
            Add("Brush", new BrushFactory());
            Add("Material", new MaterialFactory());
            Add("MeshGeometry3D", new MeshGeometry3DFactory());
            Add("Model3D", Model3DFactory);
            Add("Visual3D", new Visual3DFactory());
            Add("GeometryModel3D", new GeometryModel3DFactory());
            Add("Transform3D", new Transform3DFactory());
        }

        public ResourceFactory ResourceFactory { get; } = new ResourceFactory();
        public Model3DFactory Model3DFactory { get; } = new Model3DFactory();
    }
}
