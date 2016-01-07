namespace Node.Net.Model3D
{
    public class Renderer : IRenderer
    {
        private System.Windows.ResourceDictionary resources = new System.Windows.ResourceDictionary();
        public System.Windows.ResourceDictionary Resources => resources;

        private Rendering.Viewport3DRenderer viewport3DRenderer = null;
        private Rendering.Viewport3DRenderer Viewport3DRenderer
        {
            get
            {
                if(object.ReferenceEquals(null,viewport3DRenderer))
                {
                    viewport3DRenderer = new Rendering.Viewport3DRenderer(this);
                    viewport3DRenderer.Visual3DRenderer.Model3DRenderer.Model3DRequested += Model3DRenderer_Model3DRequested;
                }
                return viewport3DRenderer;
            }
        }

        public virtual System.Windows.Controls.Viewport3D GetViewport3D(object value) => Viewport3DRenderer.GetViewport3D(value);

        public virtual System.Windows.Media.Media3D.Visual3D GetVisual3D(object value) => Viewport3DRenderer.Visual3DRenderer.GetVisual3D(value);
        //public virtual System.Windows.Media.Media3D.ModelVisual3D GetModelVisual3D(object value) => Viewport3DRenderer.Visual3DRenderer.GetVisual3D(value);
        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(object value) => Viewport3DRenderer.Visual3DRenderer.Model3DRenderer.GetModel3D(value);

        public event Model3DRequestedEventHandler Model3DRequested;
        System.Windows.Media.Media3D.Model3D Model3DRenderer_Model3DRequested(object value, Measurement.LengthUnit units)
        {
            if(!object.ReferenceEquals(null,Model3DRequested))
            {
                System.Windows.Media.Media3D.Model3D model = Model3DRequested(value, units);
                if (!object.ReferenceEquals(null, model)) return model;
            }
            return null;
        }

        public System.Windows.Media.Media3D.MeshGeometry3D GetMeshGeometry3D(string name) => Viewport3DRenderer.Visual3DRenderer.Model3DRenderer.GeometryModel3DRenderer.MeshGeometry3DRenderer.GetMeshGeometry3D(name);
        public System.Windows.Media.Media3D.MatrixTransform3D GetMatrixTransform3D_NoScale(System.Collections.IDictionary value,
                                     Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter) => Viewport3DRenderer.Visual3DRenderer.Model3DRenderer.MatrixTransform3DRenderer.GetMatrixTransform3D_NoScale(value, units);
        public System.Windows.Media.Media3D.Material GetMaterial(string name) => Viewport3DRenderer.Visual3DRenderer.Model3DRenderer.GeometryModel3DRenderer.MaterialRenderer.GetMaterial(name);
    }
}
