using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Model3D.Rendering
{
    [TestFixture, Category("Node.Net.Model3D.Rendering.Renderer")]
    class RendererTest
    {
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
        public void Renderer_Cube_Scene()
        {
            Node.Net.Model3D.Rendering.Renderer renderer = new Node.Net.Model3D.Rendering.Renderer();

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
