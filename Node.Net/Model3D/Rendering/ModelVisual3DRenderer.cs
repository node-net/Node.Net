namespace Node.Net.Model3D.Rendering
{
    class ModelVisual3DRenderer : Base
    {
        public ModelVisual3DRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer)
        {
        }
        private Model3DRenderer model3DRenderer = null;
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

        public System.Windows.Media.Media3D.ModelVisual3D GetModelVisual3D(object value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            if (object.ReferenceEquals(null, value)) return null;
            if (typeof(System.Windows.Media.Media3D.ModelVisual3D).IsAssignableFrom(value.GetType()))
                return value as System.Windows.Media.Media3D.ModelVisual3D;


            System.Windows.Media.Media3D.ModelVisual3D model = new System.Windows.Media.Media3D.ModelVisual3D();
            model.Content = model3DRenderer.GetModel3D(value, units);
            return model;
        }
    }
}
