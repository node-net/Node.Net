using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Node.Net.Extensions.Test
{
    /// <summary>
    /// Interaction logic for Viewport3DTester.xaml
    /// </summary>
    public partial class Viewport3DTester : UserControl
    {
        public Viewport3DTester()
        {
            InitializeComponent();
            viewport.Children.Add(GetVisual3D());
            helixViewport.Children.Add(GetVisual3D());
            helixViewport.CameraChanged += HelixViewport_CameraChanged;
            propertyGrid.SelectedObject = helixViewport.Camera;
        }

        private void HelixViewport_CameraChanged(object sender, RoutedEventArgs e)
        {
            viewport.Camera = helixViewport.Camera;
        }

        private Visual3D GetVisual3D()
        {
            var v3d = new ModelVisual3D();
            v3d.Children.Add(new HelixToolkit.Wpf.SunLight());
            v3d.Children.Add(new HelixToolkit.Wpf.Teapot());
            return v3d;
        }

        private void ZoomExtents_Click(object sender, RoutedEventArgs e)
        {
            // For given Camera FieldOfView,LookDirection and UpDirection
            //       and BoundingVolume of Viewport3D.Children,
            // compute Position that would bring all of BoundingVolume into view
        }
    }
}
