namespace Node.Net.Model3D
{

    public class CompositeRenderer : IRenderer
    {
        private Renderer primaryRenderer = new Renderer();
        private System.Collections.ObjectModel.ObservableCollection<IRenderer> renderers = null;

        public System.Collections.ObjectModel.ObservableCollection<IRenderer> Renderers
        {
            get { return renderers; }
        }
        public CompositeRenderer()
        {
            renderers = new System.Collections.ObjectModel.ObservableCollection<IRenderer>();
            renderers.CollectionChanged += renderers_CollectionChanged;
        }

        void renderers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            primaryRenderer.Resources.Clear();
            foreach(IRenderer renderer in renderers)
            {
                foreach(object key in renderer.Resources.Keys)
                {
                    if (!primaryRenderer.Resources.Contains(key)) primaryRenderer.Resources.Add(key, renderer.Resources[key]);
                }
                renderer.Model3DRequested -= renderer_Model3DRequested;
                renderer.Model3DRequested += renderer_Model3DRequested;
            }
        }

        System.Windows.Media.Media3D.Model3D renderer_Model3DRequested(object value, Measurement.LengthUnit units)
        {
            return GetModel3D(value);
        }

        public event Model3DRequestedEventHandler Model3DRequested;
        System.Windows.Media.Media3D.Model3D Model3DRenderer_Model3DRequested(object value, Measurement.LengthUnit units)
        {
            if (!object.ReferenceEquals(null, Model3DRequested))
            {
                System.Windows.Media.Media3D.Model3D model = Model3DRequested(value, units);
                if (!object.ReferenceEquals(null, model)) return model;
            }
            return null;
        }

        public System.Windows.Controls.Viewport3D GetViewport3D(object value)
        {
            return primaryRenderer.GetViewport3D(value);
        }
        public System.Windows.ResourceDictionary Resources
        {
            get { return primaryRenderer.Resources; }
        }
        public virtual System.Windows.Media.Media3D.Model3D GetModel3D(object value)
        {
            System.Windows.Media.Media3D.Model3D model = null;
            foreach(IRenderer renderer in renderers)
            {
                model = renderer.GetModel3D(value);
                if (!object.ReferenceEquals(null, model)) return model;
            }
            return primaryRenderer.GetModel3D(value);
        }
    }
}