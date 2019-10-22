namespace Node.Net.Model3D.Test
{
    [NUnit.Framework.TestFixture,NUnit.Framework.Category("Model3D")]
    class Model3D_Test
    {
        [NUnit.Framework.TestCase]
        public void Model3D_UnitCube()
        {
            System.Windows.Media.Media3D.Model3D unitCube
                = Model3D.GetCube();
            System.Windows.Media.Media3D.Rect3D bounds = unitCube.Bounds;
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeX, "unitCube bounds.SizeX is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeY, "unitCube bounds.SizeY is not 1");
            NUnit.Framework.Assert.AreEqual(1.0, bounds.SizeZ, "unitCube bounds.SizeZ is not 1");
        }
    }
}