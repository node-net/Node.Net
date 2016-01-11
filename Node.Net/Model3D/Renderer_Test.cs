using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D
{
    [TestFixture,Category("Node.Net.Model3D.Renderer")]
    class Renderer_Test
    {
        private Stream GetStream(Type type,string name)
        {
            foreach(string manifestResourceName in type.Assembly.GetManifestResourceNames())
            {
                if (manifestResourceName.Contains(name)) return type.Assembly.GetManifestResourceStream(manifestResourceName);
            }
            return null;
        }
        [TestCase, NUnit.Framework.Explicit, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void Renderer_GetViewport3D_Mesh()
        {
            System.Windows.Media.Media3D.MeshGeometry3D cube
                = System.Windows.Markup.XamlReader.Load(
                    GetStream(
                      typeof(Node.Net.Model3D.Camera),
                        "UnitCube.Mesh.xaml"))
                as System.Windows.Media.Media3D.MeshGeometry3D;
            RenderToViewport(cube);
        }
        [TestCase, NUnit.Framework.Explicit, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void Renderer_GetViewport3D_Mesh_in_Hash()
        {
            Dictionary<string, dynamic> model = new Dictionary<string, dynamic>();
            model["Sunlight"] = PrimitivesRenderer_Test.GetSunlight();
            //Node.Net.Json.Hash model = new Json.Hash("{'Sunlight':{'Type':'Sunlight'}}");
            Dictionary<string, dynamic> sphere = new Dictionary<string, dynamic>();
            sphere["Type"] = "Sphere";
            sphere["Material"] = "Yellow";
            sphere["X"] = "10 m";
            //Node.Net.Json.Hash sphere = new Json.Hash("{'Type': 'Sphere', 'Material': 'Yellow', 'X': '10 m' }");
            model["Sphere"] = sphere;
            System.Windows.Media.Media3D.MeshGeometry3D cube
                = System.Windows.Markup.XamlReader.Load(
                    GetStream(
                      typeof(Node.Net.Model3D.Camera),
                        "UnitCube.Mesh.xaml"))
                as System.Windows.Media.Media3D.MeshGeometry3D;
            model["Cube"] = cube;
            RenderToViewport(model);
        }
        /*
        [TestCase, NUnit.Framework.Explicit, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void Renderer_GetViewport3D_Scene_1()
        {
            RenderToViewport(new Json.Hash(
                Node.Net.Environment.Resources.GetStream(
                  typeof(Node.Net.Model3D.Renderer_Test),
                  "Node.Net.Test.Resources.Scene.1.json")));
        }

        [TestCase,NUnit.Framework.Explicit,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void Renderer_GetViewport3D_Scene_2()
        {
            RenderToViewport(new Json.Hash(
                Node.Net.Environment.Resources.GetStream(
                  typeof(Node.Net.Model3D.Renderer_Test),
                  "Node.Net.Test.Resources.Scene.2.json")));
        }

        [TestCase,NUnit.Framework.Explicit,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void Renderer_GetViewport3D_Scene_3()
        {
            Render_Resource("Node.Net.Resources.Scene.3.json");
        }

        [TestCase,NUnit.Framework.Explicit,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA)]
        public void Renderer_GetViewport3D_Scene_Table()
        {
            Render_Resource("Node.Net.Resources.Scene.Table.json");
        }
        */
        public void RenderToViewport(object item)
        {
            PrimitivesRenderer renderer = new PrimitivesRenderer();
            System.Windows.Controls.Viewport3D viewport = renderer.GetViewport3D(item);
            //Node.Net.Model3D.Viewport3D viewport = new Node.Net.Model3D.Viewport3D(new PrimitivesRenderer());
            //viewport.DataContext = item;
            System.Windows.Window window = new System.Windows.Window() { Content = viewport, Title = "Renderer_GetViewport3D" };
            window.ShowDialog();
        }
        public void Render_Resource(string name)
        {
            /*
            Node.Net.Json.Hash scene = new Json.Hash(Node.Net.Environment.Resources.GetStream(name));
            Node.Net.Model3D.Viewport3D viewport = new Viewport3D(new PrimitivesRenderer());
            viewport.DataContext = scene;
            System.Windows.Window window = new System.Windows.Window() { Content = viewport, Title = "Renderer_GetViewport3D" };
            window.ShowDialog();

            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                              + @"\Render_Resource_" + name + ".xaml";
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
            {
                System.Windows.Markup.XamlWriter.Save(viewport, fs);
            }*/
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void Renderer_HelixViewport_Cube()
        {
            HelixToolkit.Wpf.MeshBuilder meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0), new Size3D(1, 1, 1)));
            System.Windows.Media.Media3D.MeshGeometry3D cubeMesh = meshBuilder.ToMesh(true);

            HelixToolkit.Wpf.HelixViewport3D viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D()
            {
                Content = new System.Windows.Media.Media3D.GeometryModel3D()
                {
                    Geometry = cubeMesh,
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Red),
                    BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow)
                }
            });

            Window window = new Window()
            {
                Title = "Hydrogen.Model3D.RendererTest.Renderer_HelixViewport_Cube",
                WindowState = WindowState.Maximized,
                Content = viewport
            };
            window.ShowDialog();
        }

        private Dictionary<string, dynamic> GetGeometry(string geometry_name, string material,
            string x = "0", string y = "0", string z = "0",
            string scaleX = "1", string scaleY = "1", string scaleZ = "1",
            string rotationZ = "0")
        {
            Dictionary<string, dynamic> geometry = new Dictionary<string, dynamic>();
            geometry["Geometry"] = geometry_name;
            geometry["Material"] = material;
            geometry["X"] = x;
            geometry["Y"] = y;
            geometry["Z"] = z;
            if (scaleX != "1") geometry["ScaleX"] = scaleX;
            if (scaleY != "1") geometry["ScaleY"] = scaleY;
            if (scaleZ != "1") geometry["ScaleZ"] = scaleZ;
            if (rotationZ != "0") geometry["RotationZ"] = rotationZ;
            return geometry;
        }
        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void Renderer_Render_Cube_Scene_From_IDictionary()
        {
            Renderer renderer = new Renderer();

            HelixToolkit.Wpf.MeshBuilder meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0), new Size3D(1, 1, 1)));
            renderer.Resources["Sunlight"] = new HelixToolkit.Wpf.SunLight();
            renderer.Resources["Cube"] = meshBuilder.ToMesh(true);
            renderer.Resources["Red"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Red);
            renderer.Resources["Yellow"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow);
            renderer.Resources["Black"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Black);
            renderer.Resources["Blue"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue);

            Dictionary<string, dynamic> scene = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> sunlight = new Dictionary<string, dynamic>();
            sunlight["Visual3D"] = "Sunlight";
            scene["Sunlight"] = sunlight;
            scene["Cube1"] = GetGeometry("Cube", "Yellow");
            scene["Cube2"] = GetGeometry("Cube", "Red", "2", "2");
            scene["Cube3"] = GetGeometry("Cube", "Black", "-2", "-2", "0", "0.5", "0.75", "1.0");
            scene["Cube4"] = GetGeometry("Cube", "Blue", "-2", "2", "0", "1", "1", "1", "45");

            HelixToolkit.Wpf.HelixViewport3D viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(renderer.GetVisual3D(scene));

            Window window = new Window()
            {
                Title = "Hydrogen.Model3D.RendererTest.Renderer_Render_Cube_Scene_From_IDictionary",
                WindowState = WindowState.Maximized,
                Content = viewport
            };
            window.ShowDialog();
        }
    }
}
