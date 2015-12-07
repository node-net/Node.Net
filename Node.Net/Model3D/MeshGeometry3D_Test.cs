using NUnit.Framework;

namespace Node.Net.Model3D.Test
{
    [NUnit.Framework.TestFixture,Category("Node.Net.Model3D.MeshGeometry3D")]
    class MeshGeometry3D_Test
    {
        [NUnit.Framework.TestCase]
        public void MeshGeometry3D_UnitCube()
        {
            System.Windows.Media.Media3D.MeshGeometry3D unitCube
                = MeshGeometry3D.CreateUnitCube();
            System.Windows.Media.Media3D.Rect3D bounds = unitCube.Bounds;
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeX, "unitCube bounds.SizeX is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeY, "unitCube bounds.SizeY is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeZ, "unitCube bounds.SizeZ is not 1");
        }

        [NUnit.Framework.TestCase]
        public void MeshGeometry3D_Write_Unit_Mesh_Methods()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(MeshGeometry3D));
            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                              + @"\MeshGeometry.Methods.cs";
            using(System.IO.StreamWriter sw = new System.IO.StreamWriter(filename))
            {
                string[] names = { "Cube","Pyramid","Cylinder","Cone","Sphere","Hemisphere"};
                foreach(string name in names)
                {
                    System.Windows.Media.Media3D.MeshGeometry3D mesh
                        = (System.Windows.Media.Media3D.MeshGeometry3D)System.Windows.Markup.XamlReader.Load(
                            assembly.GetManifestResourceStream("Node.Net.Node.Net.Resources.Unit" + name + ".MeshGeometry.xaml"));
                    sw.WriteLine(MeshGeometry3D.GetCreateMeshMethod("CreateUnit" + name, mesh));
                    
                }
            }
        }
    }
}
