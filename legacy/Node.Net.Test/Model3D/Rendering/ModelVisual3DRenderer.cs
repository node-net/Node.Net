namespace Node.Net.Model3D.Rendering
{
    class ModelVisual3DRenderer : Base
    {
        public ModelVisual3DRenderer(IRenderer renderer) : base(renderer)
        {
        }
        private Model3DRenderer model3DRenderer = null;//new Model3DRenderer();
        public Model3DRenderer Model3DRenderer
        {
            get 
            { 
                if(object.ReferenceEquals(null,model3DRenderer))
                {
                    model3DRenderer = new Model3DRenderer(Renderer);
                }
                return model3DRenderer; 
            }
            set { model3DRenderer = value; }
        }
        /*
        protected override void SetResourceDictionary(System.Windows.ResourceDictionary resources)
        {
            base.SetResourceDictionary(resources);
            model3DRenderer.ResourceDictionary = resources;
        }*/

        public System.Windows.Media.Media3D.ModelVisual3D GetModelVisual3D(object value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            if (typeof(System.Windows.Media.Media3D.ModelVisual3D).IsAssignableFrom(value.GetType()))
                return value as System.Windows.Media.Media3D.ModelVisual3D;

            System.Windows.Media.Media3D.ModelVisual3D model = new System.Windows.Media.Media3D.ModelVisual3D();
            model.Content = model3DRenderer.GetModel3D(value, units);
            return model;
            //return GetModelVisual3D(value as System.Collections.IDictionary, units);
        }
        /*
        protected virtual System.Windows.Media.Media3D.ModelVisual3D GetModelVisual3D(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            System.Windows.Media.Media3D.ModelVisual3D model = new System.Windows.Media.Media3D.ModelVisual3D();
            model.Content = model3DRenderer.GetModel3D(value, units);
            return model;
        }

        public System.Windows.Media.Media3D.ModelVisual3D GetModelVisual3D(System.Windows.Media.Media3D.MeshGeometry3D mesh)
        {
            if (object.ReferenceEquals(null, mesh)) return null;
            System.Windows.Media.Media3D.ModelVisual3D model = new System.Windows.Media.Media3D.ModelVisual3D();
            model.Content = model3DRenderer.GetModel3D(mesh);
            return model;
        }
         */
    }
}
