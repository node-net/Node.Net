using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta
{
    [TestFixture,Category(nameof(Beta))]
    class View3DTest
    {/*
        [Test,Apartment(ApartmentState.STA),Explicit]

        public void View3D_ShowDialog()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(View3DTest).Assembly);
            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            var v3d = factory.Create<Visual3D>(scene);
            Assert.NotNull(v3d, nameof(v3d));

            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(v3d);

            new Window
            {
                Title = "View3D Test",
                Content = viewport
            }.ShowDialog();
        }*/
    }
}
