using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Test.Controls
{
    [TestFixture]
    class View3DTest
    {
        [Test, Explicit, Apartment(ApartmentState.STA)]
        [TestCase("Scene.Office.json", "default")]
        [TestCase("Scene.Cube.json", "")]
        [TestCase("Scene.Cubes.json", "")]
        [TestCase("Scene.Cubes.json", "default")]
        [TestCase("Scene.Cylinder.json", "default")]
        [TestCase("Scene.Cylinder.Box.json", "default")]
        [TestCase("Scene.Cylinder.json","custom_cylinder")]
        public void View3D_Usage(string resourceName, string factoryName)
        {
            var factory = GetFactory(factoryName);
            var window = new Window
            {
                Title = $"View3D {resourceName}, {factoryName}",
                Content = new View3D(GetFactory(factoryName), true) { DataContext = GlobalFixture.Read(resourceName) },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        private static bool lockCustomCylinder = false;
        private static Model3D Model3DFromIDictionary_Custom_Cylinder(IDictionary source,IFactory factory)
        {
            if (lockCustomCylinder) return null;
            try
            {
                lockCustomCylinder = true;
                var customSource = new Dictionary<string, dynamic>();
                foreach(string key in source.Keys)
                {
                    customSource[key] = source[key];
                }
                customSource["Type"] = "Cube";
                return factory.Create<Model3D>(customSource, null);
            }
            finally
            {
                lockCustomCylinder = false;
            }
        }
        private static IFactory GetFactory(string factoryName)
        {
            Factory factory;
            switch(factoryName)
            {
                case "custom_cylinder":
                    {
                        factory = new Factory { GetFunction = GetHelper.GetResource };
                        factory.Add("cylinder", new Node.Net.Factories.Deprecated.Factories.Generic.FunctionAdapter3<Model3D, IDictionary>(Model3DFromIDictionary_Custom_Cylinder));
                        factory.Add("default", Factory.Default.Create<IFactory>(null, null));
                        //return factory;
                        break;
                    }
                default:
                    {
                        factory = new Factory { GetFunction = GetHelper.GetResource };
                        factory.Add("default", Factory.Default.Create<IFactory>(null, null));
                        //return factory;
                        break;
                    }
            }
            return factory;
        }

        public static object GetFunction(string name)
        {
            if (name == "Cube")
            {
                var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
                meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                return new GeometryModel3D
                {
                    Geometry = meshBuilder.ToMesh(),
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue),
                    BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Gray)
                };
            }
            if (name == "Sunlight")
            {
                var sunlight = new Model3DGroup();
                sunlight.Children.Add(new DirectionalLight { Direction = new Vector3D(-0.32139380484327, 0.383022221559489, -0.866025403784439), Color = Color.FromRgb(255, 153, 153) });
                sunlight.Children.Add(new AmbientLight { Color = Color.FromRgb(255, 102, 102) });
                return sunlight;
            }
            return null;
        }
    }
}
