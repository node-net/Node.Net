namespace Node.Net.Model3D.Rendering
{
    class Viewport3DRenderer : Base
    {
        public Viewport3DRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer)
        {
            Renderer = renderer;
            //SetResourceDictionary(new Node.Net.Model3D.Resources());
        }
        private ModelVisual3DRenderer modelVisual3DRenderer = null;//new ModelVisual3DRenderer();
        public ModelVisual3DRenderer ModelVisual3DRenderer
        {
            get 
            {
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

        public virtual System.Windows.Controls.Viewport3D GetViewport3D(object value)
        {
            if (object.ReferenceEquals(null, value)) return null;

            if (typeof(System.Windows.Controls.Viewport3D).IsAssignableFrom(value.GetType())) return value as System.Windows.Controls.Viewport3D;

            System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, dictionary)) return GetViewport3D(dictionary);

            System.Windows.Media.Media3D.MeshGeometry3D mesh = value as System.Windows.Media.Media3D.MeshGeometry3D;
            if (!object.ReferenceEquals(null, mesh)) return GetViewport3D(mesh);
            return null;
        }

        public virtual System.Windows.Controls.Viewport3D GetViewport3D(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            System.Windows.Controls.Viewport3D viewport = new System.Windows.Controls.Viewport3D();
            System.Windows.Media.Media3D.ModelVisual3D modelVisual3D = modelVisual3DRenderer.GetModelVisual3D(value, units);
            //viewport.Camera = GetCamera(modelVisual3D);
            viewport.Children.Add(modelVisual3D);
            //viewport.Chil
            return viewport;
        }

        private System.Windows.Controls.Viewport3D GetViewport3D(System.Windows.Media.Media3D.MeshGeometry3D mesh)
        {
            System.Windows.Controls.Viewport3D viewport = new System.Windows.Controls.Viewport3D();
            System.Windows.Media.Media3D.ModelVisual3D modelVisual3D = modelVisual3DRenderer.GetModelVisual3D(mesh);
            viewport.Children.Add(modelVisual3D);
            return viewport;
        }
    }
}
