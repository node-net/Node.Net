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

namespace Node.Net
{
    class FactoryCreateViewport3DTest
    {
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
        public void Viewport3D_Bitmap_ShowDialog()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            var m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d));

            var viewport = factory.Create<Viewport3D>(scene);
            Assert.NotNull(viewport, nameof(viewport));

            var bitmap = viewport.CreateBitmapSource(800, 600);
            new Window
            {
                Title = "Factory Viewport3D as Bitmap",
                WindowState = WindowState.Maximized,
                Content = new Image { Source=bitmap}
            }.ShowDialog();
        }
    }
}
