using NUnit.Framework;

namespace Node.Net.Model3D.Test
{
    [TestFixture,Category("Node.Net.Model3D.Resources")]
    class Resources_Test
    {
        [TestCase]
        public void Resources_Usage()
        {
            Resources resources = new Resources();
            resources["Sunlight"] = Model3D.GetSunlight();
            resources["Cube"] = MeshGeometry3D.CreateUnitCube();
            resources["Sphere"] = MeshGeometry3D.CreateUnitSphere();
            resources["Cube.Top"] = MeshGeometry3D.GetTranslatedMesh(MeshGeometry3D.CreateUnitCube(), 0, 0, -0.5);
            resources["Red"] = Material.GetDiffuse(System.Windows.Media.Colors.Red);

            string filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                              + @"\Resources.xaml";
            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
            {
                resources.Save(fs);
            }

            using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open))
            {
                Resources resources2 = new Resources(fs);
                NUnit.Framework.Assert.True(resources2.Contains("Sunlight"));
                NUnit.Framework.Assert.NotNull(resources2["Sunlight"]);
            }

            filename = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)
                              + @"\Resources.json";
            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Create))
            {
                resources.Save(fs, Resources.Format.Json);
            }
            using(System.IO.FileStream fs = new System.IO.FileStream(filename,System.IO.FileMode.Open))
            {
                Resources resources3 = new Resources(fs);
                NUnit.Framework.Assert.True(resources3.Contains("Sunlight"));
                NUnit.Framework.Assert.NotNull(resources3["Sunlight"]);
            }
        }
    }
}
