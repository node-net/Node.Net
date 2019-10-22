using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Extension
{
    [TestFixture]
    class Visual3DExtensionTest
    {
        [Test]
        public void HitTest()
        {
            var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
            meshBuilder.AddCubeFace(new Point3D(0, 0, 0), new Vector3D(0, 0, 1), new Vector3D(0, 1, 0), 0, 100, 100);
            var visual3D = new ModelVisual3D
            {
                Content = new GeometryModel3D
                {
                    Geometry = meshBuilder.ToMesh(true),
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue),
                    BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Yellow)
                }
            };
            Assert.IsNull(visual3D.HitTest(new Point3D(0, 0, 10), new Vector3D(0, 0, 1)));
            Assert.True(visual3D.HitTest(new Point3D(0, 0, 10), new Vector3D(0, 0, -1)).HasValue);
        }
    }
}
