using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Node.Net;
using System.Windows.Media;

namespace Node.Net
{
    class FactoryCreateViewport3DTest
    {
        public Viewport3D GetViewport3D_Blue()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            return factory.Create<Viewport3D>("Scene.Cubes.json");
        }
        public Viewport3D GetViewport3D_Red()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            var viewport =  factory.Create<Viewport3D>("Scene.Cubes.Red.xaml");
            viewport.Camera = new PerspectiveCamera
            {
                LookDirection = new Vector3D(1, 1, -1),
                Position = new Point3D(-5, -5, 7),
                UpDirection = new Vector3D(0, 0, 1)
            };
            return viewport;
        }
        [Test, Apartment(ApartmentState.STA), Explicit]
        public void Viewport3D_ShowDialog()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            var m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d));

            var viewport = factory.Create<Viewport3D>(scene);
            Assert.NotNull(viewport, nameof(viewport));

            new Window
            {
                Title = "Factory Viewport3D",
                WindowState = WindowState.Maximized,
                Content = viewport
            }.ShowDialog();
        }

        [Test, Apartment(ApartmentState.STA), Explicit]
        public void Viewport3D_ShowDialog_Red()
        {
            new Window
            {
                Title = "Factory Viewport3D",
                WindowState = WindowState.Maximized,
                Content = GetViewport3D_Red()
            }.ShowDialog();
        }

        [Test, Apartment(ApartmentState.STA), Explicit]
        public void Viewport3D_Bitmap_ShowDialog()
        {
            var bitmap = GetViewport3D_Blue().CreateBitmapSource(1600, 1200, GetViewport3D_Red());
            new Window
            {
                Title = "Factory Viewport3D as Bitmap",
                WindowState = WindowState.Maximized,
                Content = new Image { Source= bitmap }
            }.ShowDialog();
        }

        public static ImageSource GetLayeredImageSource(Visual background,UIElement foreground,double width,double height)
        {
            var background_bitmap = background.CreateBitmapSource(width, height);
            var grid = new Grid();
            grid.Children.Add(new Image { Source = background_bitmap });
            grid.Children.Add(foreground);
            return grid.CreateBitmapSource(width, height);
        }
    }
}
