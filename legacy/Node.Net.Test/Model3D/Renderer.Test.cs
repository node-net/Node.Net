using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Markup;

namespace Node.Net._Model3D
{
    [TestFixture,Category("Model3D.Renderer")]
    class RendererTest
    {
        public static System.Windows.Media.Media3D.Model3D GetTableSceneModel()
        {
            var rd = XamlReader.Load(_Extensions.StreamExtension.GetStream("Dictionary.Test.Table.Scene.Resources.xaml")) as ResourceDictionary;
            var scene = Node.Net.Reader.Default.Read(_Extensions.StreamExtension.GetStream("Dictionary.Test.Table.Scene.json"));
            IRenderer renderer = new Renderer
            {
                Resources = new Resources.Resources(rd)
            };
            return renderer.GetModel3D(scene);
        }
        public static System.Windows.Media.Media3D.Model3D GetPositionalSceneModel()
        {
            var rd = XamlReader.Load(_Extensions.StreamExtension.GetStream("Dictionary.Test.Positional.Scene.Resources.xaml")) as ResourceDictionary;
            var scene = Node.Net.Reader.Default.Read(_Extensions.StreamExtension.GetStream("Dictionary.Test.Positional.Scene.json"));
            IRenderer renderer = new Renderer
            {
                Resources = new Resources.Resources(rd)
            };
            return renderer.GetModel3D(scene);
        }

        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Renderer_Table_Scene_Explicit()
        {
            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D
            {
                Content = GetTableSceneModel()
            });
            var window = new Window
            {
                Content = viewport
            };
            window.ShowDialog();
        }
        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void Renderer_Positional_Scene_Explicit()
        {
            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D
            {
                Content = GetPositionalSceneModel()
            });
            var window = new Window
            {
                Content = viewport
            };
            window.ShowDialog();
        }
        private static Dictionary<string,dynamic> GetGeometry(string geometry_name,string material,
            string x="0",string y="0",string z="0",
            string scaleX="1",string scaleY="1",string scaleZ="1",
            string rotationZ="0")
        {
            var geometry = new Dictionary<string, dynamic>();
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
            var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0),new Size3D(1,1,1)));
            var cubeMesh = meshBuilder.ToMesh(true);

            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D
            {
                Content = new System.Windows.Media.Media3D.GeometryModel3D
                {
                    Geometry = cubeMesh,
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Red),
                    BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow)
                }
            });

            var window = new Window
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
            var renderer = new Renderer();
            var resources = new Resources.Resources();

            var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0), new Size3D(1, 1, 1)));
            resources["Cube"] = meshBuilder.ToMesh(true);
            resources["Red"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Red);
            resources["Yellow"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow);
            renderer.Resources = resources;

            IDictionary cube = GetGeometry("Cube", "Red");

            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            viewport.Children.Add(new System.Windows.Media.Media3D.ModelVisual3D
            {
                Content = renderer.GetModel3D(cube)
            });

            var window = new Window
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
            var renderer = new Renderer();
            var resources = new Resources.Resources();

            var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddBox(new Rect3D(new Point3D(0, 0, 0), new Size3D(1, 1, 1)));
            resources["Sunlight"] = new HelixToolkit.Wpf.SunLight();
            resources["Cube"] = meshBuilder.ToMesh(true);
            resources["Red"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Red);
            resources["Yellow"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow);
            resources["Black"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Black);
            resources["Blue"] = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue);
            renderer.Resources = resources;

            var scene = new Dictionary<string, dynamic>();
            var sunlight = new Dictionary<string, dynamic>();
            //sunlight["Model3D"] = "Sunlight";
            sunlight["Visual3D"] = "Sunlight";
            scene["Sunlight"] = sunlight;
            scene["Cube1"] = GetGeometry("Cube", "Yellow");
            scene["Cube2"] = GetGeometry("Cube", "Red", "2", "2");
            scene["Cube3"] = GetGeometry("Cube", "Black", "-2", "-2", "0", "0.5", "0.75", "1.0");
            scene["Cube4"] = GetGeometry("Cube", "Blue", "-2", "2", "0", "1", "1", "1","45");

            var viewport = new HelixToolkit.Wpf.HelixViewport3D();
            viewport.Children.Add(renderer.GetVisual3D(scene));

            var window = new Window
            {
                Title = "Hydrogen.Model3D.RendererTest.Renderer_Render_Cube_Scene_From_IDictionary",
                WindowState = WindowState.Maximized,
                Content = viewport
            };
            window.ShowDialog();
        }
        /*
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void Renderer_Teapot_OTS()
        {
            var window = new Window
            {
                Title = "Renderer_Teapot_OTS",
                WindowState = WindowState.Maximized,
                Content = new TeapotOTSView()
            };
            window.ShowDialog();
        }*/

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void Renderer_BillboardText()
        {
            var view3D = new HelixToolkit.Wpf.HelixViewport3D();
            var visual = new ModelVisual3D();
            visual.Children.Add(new HelixToolkit.Wpf.BillboardTextVisual3D { Text = "0,0",FontSize = 14, Foreground=Brushes.Black, HorizontalAlignment= HorizontalAlignment.Left,VerticalAlignment= VerticalAlignment.Bottom });
            visual.Children.Add(new HelixToolkit.Wpf.BillboardTextVisual3D { Text = "(center)", FontSize = 14, Foreground = Brushes.Black, HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top});
            visual.Children.Add(new HelixToolkit.Wpf.BillboardTextVisual3D { Text = "10,0", Transform = new TranslateTransform3D(10,0,0), FontSize = 14, Foreground = Brushes.Black });
            view3D.Children.Add(visual);
            var window = new Window
            {
                Title = "Renderer_BillboardText",
                WindowState = WindowState.Maximized,
                Content = view3D
            };
            window.ShowDialog();
        }
    }
}
