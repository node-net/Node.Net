namespace Node.Net.Model3D.Rendering
{
    class Viewport3DRenderer : Base
    {
        public Viewport3DRenderer(Node.Net.Model3D.IRenderer renderer) : base(renderer)
        {
            Renderer = renderer;
        }
        private Visual3DRenderer visual3DRenderer = null;
        public Visual3DRenderer Visual3DRenderer
        {
            get 
            {
                if(object.ReferenceEquals(null,visual3DRenderer))
                {
                    visual3DRenderer = new Visual3DRenderer(Renderer);
                }
                return visual3DRenderer; 
            }
            set { visual3DRenderer = value; }
        }

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
            System.Windows.Media.Media3D.Visual3D modelVisual3D = visual3DRenderer.GetVisual3D(value, units);
            viewport.Children.Add(modelVisual3D);
            return viewport;
        }

        private System.Windows.Controls.Viewport3D GetViewport3D(System.Windows.Media.Media3D.MeshGeometry3D mesh)
        {
            System.Windows.Controls.Viewport3D viewport = new System.Windows.Controls.Viewport3D();
            System.Windows.Media.Media3D.Visual3D modelVisual3D = visual3DRenderer.GetVisual3D(mesh);
            viewport.Children.Add(modelVisual3D);
            return viewport;
        }
    }
}
