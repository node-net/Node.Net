using NUnit.Framework;

namespace Node.Net.Model3D.Test
{
    [TestFixture,Category("Node.Net.Model3D.PrimitivesRenderer")]
    class PrimitivesRenderer_Test
    {
        public object GetCubeScene()
        {
            Node.Net.Json.Hash cubeScene = new Json.Hash();
            cubeScene["Sunlight"] = new Json.Hash("{'Type':'Sunlight'}");
            cubeScene["Cube"] = new Json.Hash("{'Type':'Cube'}");
            return cubeScene;
        }
        public object GetCubeScene2()
        {
            Node.Net.Json.Hash cubeScene = new Json.Hash();
            cubeScene["Sunlight"] = new Json.Hash("{'Type':'Sunlight'}");
            cubeScene["Cube"] = MeshGeometry3D.CreateUnitCube();
            return cubeScene;
        }
        public object GetCubeScene3()
        {
            Node.Net.Json.Hash cubeScene = new Json.Hash();
            cubeScene["Sunlight"] = new Json.Hash("{'Type':'Sunlight'}");
            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D = new System.Windows.Media.Media3D.GeometryModel3D() 
            { Geometry=MeshGeometry3D.CreateUnitCube(), Material=Material.GetDiffuse(System.Windows.Media.Colors.Yellow) };
            cubeScene["Cube"] = geometryModel3D;
            return cubeScene;
        }
        public object GetCubeScene4()
        {
            Node.Net.Json.Hash cubeScene = new Json.Hash();
            cubeScene["Sunlight"] = new Json.Hash("{'Type':'Sunlight'}");
            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D = new System.Windows.Media.Media3D.GeometryModel3D() { Geometry = MeshGeometry3D.CreateUnitCube(), Material = Material.GetDiffuse(System.Windows.Media.Colors.Tomato) };
            System.Windows.Media.Media3D.Model3DGroup model3DGroup = new System.Windows.Media.Media3D.Model3DGroup();
            model3DGroup.Children.Add(geometryModel3D);
            cubeScene["Cube"] = model3DGroup;
            return cubeScene;
        }
        public object GetCubeScene5()
        {
            Node.Net.Json.Hash cubeScene = new Json.Hash();
            cubeScene["Sunlight"] = new Json.Hash("{'Type':'Sunlight'}");
            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D 
                = new System.Windows.Media.Media3D.GeometryModel3D() 
                { Geometry = MeshGeometry3D.CreateUnitCube(), 
                    Material = Material.GetDiffuse(System.Windows.Media.Colors.Tomato) };
            System.Windows.Media.Media3D.Model3DGroup model3DGroup
                = new System.Windows.Media.Media3D.Model3DGroup();
            model3DGroup.Children.Add(geometryModel3D);

            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D2
                = new System.Windows.Media.Media3D.GeometryModel3D()
                {
                    Geometry = MeshGeometry3D.CreateUnitCube(),
                    Material = Material.GetDiffuse(System.Windows.Media.Colors.Yellow)
                };
            geometryModel3D.Transform =
                new System.Windows.Media.Media3D.TranslateTransform3D(1, 0, 0);
            model3DGroup.Children.Add(geometryModel3D2);
            cubeScene["Cube"] = model3DGroup;
            return cubeScene;
        }


        [NUnit.Framework.TestCase]
        public void PrimitiveRenderer_GetModel3D()
        {
            PrimitivesRenderer renderer = new PrimitivesRenderer();
            System.Windows.Media.Media3D.Model3D model = renderer.GetModel3D(GetCubeScene4());
            System.Windows.Media.Media3D.Rect3D bounds = model.Bounds;
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeX, "unitCube bounds.SizeX is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeY, "unitCube bounds.SizeY is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeZ, "unitCube bounds.SizeZ is not 1");

            model = renderer.GetModel3D(GetCubeScene5());
            bounds = model.Bounds;
            NUnit.Framework.Assert.AreEqual(2.0, bounds.SizeX, "unitCube bounds.SizeX is not 2");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeY, "unitCube bounds.SizeY is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeZ, "unitCube bounds.SizeZ is not 1");
        }
        [NUnit.Framework.TestCase, NUnit.Framework.RequiresSTA, NUnit.Framework.Explicit]
        public void PrimitivesRenderer_GetViewport3D()
        {
            object[] items ={ MeshGeometry3D.CreateUnitCube(),
                              GetCubeScene(),GetCubeScene2(),
                              GetCubeScene3(),GetCubeScene4(),GetCubeScene5() };
            foreach(object item in items)
            {
                PrimitivesRenderer renderer = new PrimitivesRenderer();
                System.Windows.Window window = new System.Windows.Window() { WindowState = System.Windows.WindowState.Maximized };
                System.Windows.Controls.Viewport3D viewport = renderer.GetViewport3D(item);
                viewport.Camera = new System.Windows.Media.Media3D.PerspectiveCamera(
                    new System.Windows.Media.Media3D.Point3D(10, -10, 10),
                    new System.Windows.Media.Media3D.Vector3D(-1, 1, -1),
                    new System.Windows.Media.Media3D.Vector3D(0, 0, 1), 90);
                window.Content = viewport;
                window.ShowDialog();
            }
        }
    }
}