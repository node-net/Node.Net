using System;
using System.Windows;
using System.Windows.Markup;

namespace Node.Net.View
{
    public class HelixView3D : Grid
    {
        public HelixView3D()
        {
            this.DataContextChanged += View3D_DataContextChanged;
            var rd = (ResourceDictionary)XamlReader.Load(Node.Net.Extensions.StreamExtension.GetStream("HelixView3D.Resources.xaml"));
            Resources = new Resources.Resources(rd);

        }

        public IResources Resources
        {
            get { return renderer.Resources; }
            set { renderer.Resources = value; }
        }
        private readonly HelixToolkit.Wpf.HelixViewport3D viewport = new HelixToolkit.Wpf.HelixViewport3D();
        private readonly Node.Net._Model3D.Renderer renderer = new Node.Net._Model3D.Renderer();
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Children.Add(viewport);
        }
        private void View3D_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            update();
        }
        private void update()
        {
            viewport.Children.Clear();
            viewport.Children.Add(renderer.GetVisual3D(DataContext));
        }
    }
}
