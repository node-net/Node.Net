using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace Node.Net.Model3D
{
    [TestFixture,Category("Node.Net.Model3D.PrimitivesRenderer")]
    class PrimitivesRenderer_Test
    {
        public static object GetSunlight()
        {
            Dictionary<string, dynamic> sunlight = new Dictionary<string, dynamic>();
            sunlight["Type"] = "Sunlight";
            return sunlight;
        }

        public object GetCube()
        {
            Dictionary<string, dynamic> cube = new Dictionary<string, dynamic>();
            cube["Type"] = "Cube";
            return cube;
        }
        public object GetCubeScene()
        {
            Dictionary<string, dynamic> cubeScene = new Dictionary<string, dynamic>();
            cubeScene["Sunlight"] = GetSunlight();
            cubeScene["Cube"] = GetCube();
            return cubeScene;
        }
        public object GetCubeScene2()
        {
            Dictionary<string, dynamic> cubeScene = new Dictionary<string, dynamic>();
            cubeScene["Sunlight"] = GetSunlight();
            cubeScene["Cube"] = MeshGeometry3D.CreateUnitCube();
            return cubeScene;
        }
        public object GetCubeScene3()
        {
            Dictionary<string, dynamic> cubeScene = new Dictionary<string, dynamic>();
            cubeScene["Sunlight"] = GetSunlight();
            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D = new System.Windows.Media.Media3D.GeometryModel3D() 
            { Geometry=MeshGeometry3D.CreateUnitCube(), Material=Material.GetDiffuse(System.Windows.Media.Colors.Yellow) };
            cubeScene["Cube"] = geometryModel3D;
            return cubeScene;
        }
        public object GetCubeScene4()
        {
            Dictionary<string, dynamic> cubeScene = new Dictionary<string, dynamic>();
            cubeScene["Sunlight"] = GetSunlight();
            System.Windows.Media.Media3D.GeometryModel3D geometryModel3D = new System.Windows.Media.Media3D.GeometryModel3D() { Geometry = MeshGeometry3D.CreateUnitCube(), Material = Material.GetDiffuse(System.Windows.Media.Colors.Tomato) };
            System.Windows.Media.Media3D.Model3DGroup model3DGroup = new System.Windows.Media.Media3D.Model3DGroup();
            model3DGroup.Children.Add(geometryModel3D);
            cubeScene["Cube"] = model3DGroup;
            return cubeScene;
        }
        public object GetCubeScene5()
        {
            Dictionary<string, dynamic> cubeScene = new Dictionary<string, dynamic>();
            //Node.Net.Json.Hash cubeScene = new Json.Hash();
            cubeScene["Sunlight"] = GetSunlight();
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


        [TestCase]
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
        [TestCase, Apartment(ApartmentState.STA), Explicit]
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