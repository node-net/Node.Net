using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture]
    class VisualExtensionTest
    {
        /*
        [Test,Apartment(ApartmentState.STA),Explicit]
        public void Visual_CreateBitmapSource()
        {
            var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new System.Windows.Media.Media3D.Point3D(0, 0, 0), 1, 1, 1);
            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            viewport.Children.Add(new ModelVisual3D
            {
                Content = new GeometryModel3D
                {
                    Geometry = meshBuilder.ToMesh(true),
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue)
                }
            });


            new Window
            {
                Title = "Visual_CreateBitmapSource",
                Content = new Node.Net.Tests.Extension.VisualCreateBitmapTestControl { UIElement = viewport },
                WindowState = WindowState.Maximized
            }.ShowDialog();
        }*/
    }
}
