namespace Node.Net.Model3D.Rendering
{
    class Visual3DCollectionRenderer : Base
    {
        public Visual3DCollectionRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer) { }
        private ModelVisual3DRenderer modelVisual3DRenderer = null;//new ModelVisual3DRenderer();
        public ModelVisual3DRenderer ModelVisual3DRenderer
        {
            get {
                if(object.ReferenceEquals(null,modelVisual3DRenderer))
                {
                    modelVisual3DRenderer = new ModelVisual3DRenderer(Renderer);
                }
                return modelVisual3DRenderer; 
            }
            set { modelVisual3DRenderer = value; }
        }
        /*
        protected override void SetResourceDictionary(System.Windows.ResourceDictionary resources)
        {
            base.SetResourceDictionary(resources);
            modelVisual3DRenderer.ResourceDictionary = resources;
        }*/

        public System.Windows.Media.Media3D.Visual3D[] GetVisual3DCollection(object value,
        Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter) => GetVisual3DCollection(value as System.Collections.IDictionary, units);
        protected virtual System.Windows.Media.Media3D.Visual3D[] GetVisual3DCollection(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            System.Collections.Generic.List<System.Windows.Media.Media3D.Visual3D> results
                = new System.Collections.Generic.List<System.Windows.Media.Media3D.Visual3D>();
            foreach(object key in value)
            {
                System.Windows.Media.Media3D.Visual3D visual3D = modelVisual3DRenderer.GetModelVisual3D(value[key], units);
                if(!object.ReferenceEquals(null,visual3D))
                {
                    results.Add(visual3D);
                }
            }
            return results.ToArray();
        }
    }
}
