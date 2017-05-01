using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;

namespace Node.Net
{
    [TestFixture]
    class FactoryCreateVisual3DTest
    {
        [Test]
        public void SceneCubes()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var m3d = factory.Create<Model3D>("Scene.Cubes.json");
            Assert.NotNull(m3d, nameof(m3d), "from 'Scene.Cubes.json'");

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d),"from IDictionary");

            

            var v3d = factory.Create<Visual3D>(scene);
            Assert.NotNull(v3d, nameof(v3d));

            v3d = factory.Create<Visual3D>("Scene.Cubes.json");
            Assert.NotNull(v3d, nameof(v3d));
        }

        [Test]
        public void Scene()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.json");
            Assert.NotNull(scene, nameof(scene));

            var m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d), "from IDictionary");

            m3d = factory.Create<Model3D>("Scene.json");
            Assert.NotNull(m3d, nameof(m3d), "from 'Scene.json'");

            

            

            var v3d = factory.Create<Visual3D>(scene);
            Assert.NotNull(v3d, nameof(v3d));

            v3d = factory.Create<Visual3D>("Scene.json");
            Assert.NotNull(v3d, nameof(v3d));
        }

        [Test,Apartment(ApartmentState.STA),Explicit]
        public void DeepScene()
        {
            var builder = new HelixToolkit.Wpf.MeshBuilder();
            builder.AddSphere(new Point3D(0, 0, 0), 0.25);
            var model3D = new GeometryModel3D
            {
                Geometry = builder.ToMesh(),
                Material = new DiffuseMaterial(Brushes.Orange)
            };
            var sb = new StringBuilder();
            var xmlWriter = XmlWriter.Create(sb);
            XamlWriter.Save(model3D, xmlWriter);
            var xaml = sb.ToString();

            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            var model = CreateDeepSceneModel();
            var v3d = factory.Create<Visual3D>(model);
            var viewport = new HelixToolkit.Wpf.HelixViewport3D
            {
                ShowCoordinateSystem = true,
                ShowTriangleCountInfo = true
            };
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            viewport.Children.Add(v3d);
            new Window
            {
                Content = viewport,
                WindowState = WindowState.Maximized,
                Title = "DeepScene"
            }.ShowDialog();
        }

        private IDictionary CreateDeepSceneModel()
        {
            var model = new Dictionary<string, dynamic>();
            for (double x = 0.0; x < 10.1; x+=1.5)
            {
                for (double y = 0.0; y < 10.1; y+=1.5)
                {
                    var key = $"{x},{y}";
                    var value = new Dictionary<string, dynamic>
                    {
                        {"Type","Cube" },{"X" ,$"{x} m"},{"Y",$"{y} m" },{"child",CreateChildModel()}
                    };
                    model.Add(key, value);
                }
            }
            return model;
        }
        private IDictionary CreateChildModel()
        {
            var model = new Dictionary<string, dynamic>
            {
                {"s0" , new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","-0.5 m" }, {"Y","-0.5 m" },{"Z","1 m"} } },
                {"s1" ,new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","0.5 m" }, { "Y","-0.5 m" },{ "Z","1 m"} } },
                {"s2" , new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","0.5 m" }, {"Y","0.5 m" },{"Z","1 m"} } },
                {"s3" ,new Dictionary<string,dynamic>{{ "Type","Sphere" },{ "X","-0.5 m" }, { "Y","0.5 m" },{ "Z","1 m"} } }
            };
            return model;
        }
    }
}
