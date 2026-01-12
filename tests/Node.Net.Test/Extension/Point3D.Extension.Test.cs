extern alias NodeNet;
using NUnit.Framework;
using NodeNet::System.Windows.Media.Media3D;
using NodeNet::System.Windows; // For Point type
using NodeNet::Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class Point3DExtensionTest
    {
        [Test]
        public void ParsePoints()
        {
            Point3D[] points = NodeNet::Node.Net.Point3DExtension.ParsePoints("0,0,0 0,0,1");
            Assert.That(points.Length, Is.EqualTo(2));
            Point[] points2D = points.Get2DPoints();
            Assert.That(points2D.Length, Is.EqualTo(2));
            Point3D[] tpoints = points.Transform(new Matrix3D());
            Assert.That(tpoints.Length, Is.EqualTo(2));
        }
    }
}