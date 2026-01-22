using System.Threading.Tasks;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test.Extension
{
    internal class Point3DExtensionTest
    {
        [Test]
        public async Task ParsePoints()
        {
            Point3D[] points = Point3DExtension.ParsePoints("0,0,0 0,0,1");
            await Assert.That(points.Length).IsEqualTo(2);
            Point[] points2D = points.Get2DPoints();
            await Assert.That(points2D.Length).IsEqualTo(2);
            Point3D[] tpoints = points.Transform(new Matrix3D());
            await Assert.That(tpoints.Length).IsEqualTo(2);
        }
    }
}