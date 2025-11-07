#if IS_WINDOWS
using NUnit.Framework;

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class Point3DExtensionTest
    {
        [Test]
        public void ParsePoints()
        {
            System.Windows.Media.Media3D.Point3D[] points = Point3DExtension.ParsePoints("0,0,0 0,0,1");
            Assert.That(points.Length, Is.EqualTo(2));
            System.Windows.Point[] points2D = points.Get2DPoints();
            Assert.That(points2D.Length, Is.EqualTo(2));
            System.Windows.Media.Media3D.Point3D[] tpoints = points.Transform(new System.Windows.Media.Media3D.Matrix3D());
            Assert.That(tpoints.Length, Is.EqualTo(2));
        }
    }
}
#endif