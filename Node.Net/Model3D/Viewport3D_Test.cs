

namespace Node.Net.Model3D.Test
{
	/*
    public class Viewport3D : System.Windows.Controls.UserControl
    {
        private Node.Net.Model3D.IRenderer renderer = null;
        private HelixToolkit.Wpf.HelixViewport3D viewport = null;
        public Viewport3D()
        {
            DataContextChanged += onDataContextChanged;
            Update();
        }

        public Viewport3D(Node.Net.Model3D.IRenderer value)
        {
            renderer = value;
            DataContextChanged += onDataContextChanged;
            Update();
        }

        public Node.Net.Model3D.IRenderer Renderer
        {
            get { return renderer; }
            set
            {
                if (!object.ReferenceEquals(renderer, value))
                {
                    renderer = value;
                    Update();
                }
            }
        }
        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);
            viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.ZoomExtentsWhenLoaded = true;
            Background = System.Windows.Media.Brushes.Gray;
            Content = viewport;

            if (object.ReferenceEquals(null, renderer))
            {
                renderer = new Node.Net.Model3D.Renderer();
                renderer.Resources["Sunlight"] = Node.Net.Model3D.Model3D.GetSunlight();
                renderer.Resources["Cube"] = Node.Net.Model3D.MeshGeometry3D.CreateUnitCube();
                renderer.Resources["Gray"] = Node.Net.Model3D.Material.GetDiffuse(System.Windows.Media.Colors.Gray);
                renderer.Resources["Green"] = Node.Net.Model3D.Material.GetDiffuse(System.Windows.Media.Colors.Green);
                renderer.Resources["Red"] = Node.Net.Model3D.Material.GetDiffuse(System.Windows.Media.Colors.Red);

            }
            Update();
        }

        void onDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            if (object.ReferenceEquals(null, viewport)) return;

            viewport.Children.Clear();

            object model = Node.Net.Json.KeyValuePair.GetValue(DataContext);
            if (object.ReferenceEquals(null, model)) return;
            System.Windows.Controls.Viewport3D vp = renderer.GetViewport3D(model);
            if (!object.ReferenceEquals(null, vp))
            {
                System.Collections.Generic.List<System.Windows.Media.Media3D.Visual3D> children
                    = Node.Net.Model3D.Viewport3D.GetVisual3DChildren(vp);
                vp.Children.Clear();

                foreach (System.Windows.Media.Media3D.Visual3D v3d in children)
                {
                    viewport.Children.Add(v3d);
                }
            }
        }
    }
    [NUnit.Framework.TestFixture,NUnit.Framework.RequiresSTA]
    class Viewport3D_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.Explicit]
        public void Viewport3D_Usage()
        {
            System.Windows.Controls.Viewport3D viewport = new System.Windows.Controls.Viewport3D();
            viewport.DataContext = "Cube";
            Node.Net.Model3D.Viewport3D.Update(viewport);
            System.Windows.Window window = new System.Windows.Window() { Content = viewport, Title = "Viewport3D_Test" };
            window.ShowDialog();
        }
    }*/
}
