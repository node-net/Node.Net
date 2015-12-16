using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
