using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class StandardFactoryTest
    {
        [Test]
        public void StandardFactory_Usage()
        {
            var factory = new StandardFactory();
            factory.ResourceFactory.ImportManifestResources(typeof(StandardFactoryTest).Assembly);

            Assert.AreEqual(Colors.Green, factory.Create(typeof(Color), "Green"));
            Assert.AreEqual(Colors.Red, factory.Create(typeof(Color), "255,0,0"));
            Assert.AreEqual(Brushes.Blue, factory.Create(typeof(Brush), "Blue"));

            var mesh = factory.Create(typeof(MeshGeometry3D), "Cube") as MeshGeometry3D;
            Assert.NotNull(mesh, nameof(mesh));
            //Assert.IsNotNull(factory.Create(typeof(Material), "Green"),"create Material 'Green'");
            Assert.IsNotNull(factory.Create(typeof(MeshGeometry3D), "MeshGeometry3D.Cube.xaml"));

            var scene = GlobalFixture.Read("Scene.Cube.json");
            Assert.NotNull(factory.Create(typeof(MeshGeometry3D), "Cube"), "create MeshGeometry3D 'Cube'");
            Assert.NotNull(factory.Create(typeof(Model3D), scene), "create Model3D from scene");
            //Assert.NotNull(factory.Create(typeof(Visual3D), scene), "create Visual3D from scene");
        }

        [Test, Explicit, Apartment(ApartmentState.STA)]
        [TestCase("Scene.Cube.json")]
        [TestCase("Scene.Cubes.json")]
        public void StandardFactory_Rendering(string name)
        {
            var model = GlobalFixture.Read(name);
            var factory = new StandardFactory();
            factory.ResourceFactory.ImportManifestResources(typeof(StandardFactoryTest).Assembly);
            //factory.ManifestResourceFactory.Assemblies.Add(typeof(StandardFactoryTest).Assembly);
            //factory.ManifestResourceFactory.ManifestResourceNameIgnorePatterns.Add(".json");

            var v3d = factory.Create(typeof(Visual3D), model) as Visual3D;

            var view = new HelixToolkit.Wpf.HelixViewport3D
            {
                ShowCameraInfo = true,
                ShowCameraTarget = true,
                ShowCoordinateSystem = true,
                ShowFieldOfView = true,
                ShowFrameRate = true,
                ShowTriangleCountInfo = true,
                ShowViewCube = true,
                ZoomExtentsWhenLoaded = true
            };

            view.Children.Add(new HelixToolkit.Wpf.SunLight());
            if (v3d != null) view.Children.Add(v3d);
            var window = new Window
            {
                Title = $"StandardFactory_Rendering {name}",
                Content = view,
                WindowState = WindowState.Maximized
            };

            window.ShowDialog();
        }
        /*
        [Test, Explicit, Apartment(ApartmentState.STA)]
        [TestCase("Scene.Cube.json")]
        [TestCase("Scene.Cubes.json")]
        public void StandardFactory_Rendering_Element(string name)
        {
            var model = GlobalFixture.Read(name);
            var factory = new StandardFactory();
            factory.ResourceFactory.ImportManifestResources(typeof(StandardFactoryTest).Assembly);

            var dictionary = model as IDictionary;
            var element = new Element(dictionary);
            //factory.ManifestResourceFactory.Assemblies.Add(typeof(StandardFactoryTest).Assembly);
            //factory.ManifestResourceFactory.ManifestResourceNameIgnorePatterns.Add(".json");

            var v3d = factory.Create(typeof(Visual3D), element) as Visual3D;

            var view = new HelixToolkit.Wpf.HelixViewport3D
            {
                ShowCameraInfo = true,
                ShowCameraTarget = true,
                ShowCoordinateSystem = true,
                ShowFieldOfView = true,
                ShowFrameRate = true,
                ShowTriangleCountInfo = true,
                ShowViewCube = true,
                ZoomExtentsWhenLoaded = true
            };

            view.Children.Add(new HelixToolkit.Wpf.SunLight());
            if (v3d != null) view.Children.Add(v3d);
            var window = new Window
            {
                Title = $"StandardFactory_Rendering {name}",
                Content = view,
                WindowState = WindowState.Maximized
            };

            window.ShowDialog();
        }*/


        [Test, Explicit]
        public void StandardFactory_Create_Visual3D_Stress_Test()
        {
            var model = GlobalFixture.Read("Scene.Cubes.json");
            var factory = new StandardFactory { Cache = true };
            factory.ResourceFactory.ImportManifestResources(typeof(StandardFactoryTest).Assembly);

            //Assert.NotNull(factory.Create(typeof(Visual3D), model) as Visual3D,"Visual3D");

            for (int i = 0; i < 10000; ++i)
            {
                var v3d = factory.Create(typeof(Visual3D), model) as Visual3D;
                Assert.NotNull(v3d);
            }
        }

        [Test]
        public void StandardFactory_CreateMaterial_From_String()
        {
            var factory = new StandardFactory();
            Assert.NotNull(factory.Create(typeof(Material), "Blue"));
        }

        [TestCase(typeof(MeshGeometry3D), "Mesh.Cube.xaml")]
        public void StandardFactory_CreateFromManifestResource(Type targetType, string name)
        {
            var factory = new StandardFactory();
            factory.ResourceFactory.ImportManifestResources(typeof(StandardFactoryTest).Assembly);
            var item = factory.Create(targetType, name);
            Assert.NotNull(item);
        }

        [Test]
        public void Factory_CreateFromManifestResource()
        {
            var factory = new StandardFactory();
            factory.ReadFunction = new Node.Net.Readers.JsonReader().Read;
            factory.AbstractFactory.Add(typeof(IDictionary), typeof(Dictionary<string, dynamic>));
            factory.ResourceAssemblies.Add(typeof(StandardFactoryTest).Assembly);

            var dictionary = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(dictionary, nameof(dictionary));
        }
    }
}
