using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Markup;

namespace Node.Net.Model3D
{
    [TestFixture,Category("Node.Net.Model3D.Renderer")]
    class RendererTest
    {
        public System.Windows.Media.Media3D.Model3D GetTableSceneModel()
        {
            Collections.Dictionary scene = new Collections.Dictionary(IO.StreamExtension.GetStream("Dictionary.Test.Table.Scene.json"));
            IRenderer renderer = new Renderer()
            {
                Resources = XamlReader.Load(IO.StreamExtension.GetStream("Dictionary.Test.Table.Scene.Resources.xaml")) as ResourceDictionary
            };
            return renderer.GetModel3D(scene);
        }
        [TestCase, Apartment(ApartmentState.STA)]
        public void Renderer_Table_Scene()
        {
            System.Windows.Media.Media3D.Model3D model = GetTableSceneModel();
        }

        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Renderer_Table_Scene_Explicit()
        {
            HelixToolkit.Wpf.HelixViewport3D viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D()
            {
                Content = GetTableSceneModel()
            });
            Window window = new Window()
            {
                Content = viewport
            };
            window.ShowDialog();
        }
        private Dictionary<string,dynamic> GetGeometry(string geometry_name,string material,
            string x="0",string y="0",string z="0",
            string scaleX="1",string scaleY="1",string scaleZ="1",
            string rotationZ="0")
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
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Renderer_HelixViewport_Cube()
        {
            HelixToolkit.Wpf.MeshBuilder meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0),new Size3D(1,1,1)));
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
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Renderer_Render_Cube_From_IDictionary()
        {
            Renderer renderer = new Renderer();

            HelixToolkit.Wpf.MeshBuilder meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0), new Size3D(1, 1, 1)));
            renderer.Resources["Cube"] = meshBuilder.ToMesh(true);
            renderer.Resources["Red"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Red);
            renderer.Resources["Yellow"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow);

            IDictionary cube = GetGeometry("Cube", "Red");

            HelixToolkit.Wpf.HelixViewport3D viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D()
            {
                Content = renderer.GetModel3D(cube)
            });

            Window window = new Window()
            {
                Title = "Hydrogen.Model3D.RendererTest.Renderer_Render_Cube_From_IDictionary",
                WindowState = WindowState.Maximized,
                Content = viewport
            };
            window.ShowDialog();
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
            //sunlight["Model3D"] = "Sunlight";
            sunlight["Visual3D"] = "Sunlight";
            scene["Sunlight"] = sunlight;
            scene["Cube1"] = GetGeometry("Cube", "Yellow");
            scene["Cube2"] = GetGeometry("Cube", "Red", "2", "2");
            scene["Cube3"] = GetGeometry("Cube", "Black", "-2", "-2", "0", "0.5", "0.75", "1.0");
            scene["Cube4"] = GetGeometry("Cube", "Blue", "-2", "2", "0", "1", "1", "1","45");

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
