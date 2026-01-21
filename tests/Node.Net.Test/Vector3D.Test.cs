using NUnit.Framework;
using static System.Math;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Node.Net; // Extension methods are in Node.Net namespace, not Node.Net.Extension

namespace Node.Net.Test
{
    [TestFixture]
    internal static class Vector3DExtensionTest
    {
        [Test]
        public static void ComputeRayPlaneIntersection()
        {
            Vector3D intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, 0, -1), new Vector3D(0, 0, 10), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.That(Round(intersection.X, 4),Is.EqualTo(0), "intersection.X");
            Assert.That(Round(intersection.Y, 4), Is.EqualTo(0), "intersection.Y");
            Assert.That(Round(intersection.Z, 4), Is.EqualTo(0), "intersection.Z");

            intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, 0, -1), new Vector3D(0, 0, 0), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.That(Round(intersection.X, 4), Is.EqualTo(0), "intersection.X");
            Assert.That(Round(intersection.Y, 4), Is.EqualTo(0), "intersection.Y");
            Assert.That(Round(intersection.Z, 4), Is.EqualTo(0), "intersection.Z");

            intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, 0.5, -1), new Vector3D(0, 0, 10), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.That(Round(intersection.X, 4), Is.EqualTo(0), "intersection.X");
            Assert.That(Round(intersection.Y, 4),Is.EqualTo(5.0), "intersection.Y");
            Assert.That(Round(intersection.Z, 4), Is.EqualTo(0), "intersection.Z");

            // ray facing away from plane
            intersection = Vector3DExtension.ComputeRayPlaneIntersection(new Vector3D(0, -0.5, 1), new Vector3D(0, 0, 10), new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.That(Round(intersection.X, 4), Is.EqualTo(0), "intersection.X");
            Assert.That(Round(intersection.Y, 4), Is.EqualTo(5.0), "intersection.Y");
            Assert.That(Round(intersection.Z, 4), Is.EqualTo(0), "intersection.Z");
        }

        [Test]
        [TestCase(0, 0, 1, 0, "positive Z axis")]
        [TestCase(0, 0, -1, 0, "negative Z axis")]
        [TestCase(1, 0, 0, 0, "positive X axis")]
        [TestCase(-1, 0, 0, 180, "negative X axis")]
        [TestCase(0, 1, 0, 90, "positive Y axis")]
        [TestCase(0, -1, 0, -90, "negative Y axis")]
        [TestCase(1, 1, 0, 45, "1,1 in XY plane")]
        [TestCase(-1, 1, 0, 135, "-1,1 in XY plane")]
        [TestCase(1, -1, 0, -45.0, "1,-1 in XY plane")]
        [TestCase(-1, -1, 0, -135.0, "-1,-1 in XY plane")]
        public static void GetAzimuthalAngle(double x, double y, double z, double expected_angle, string name)
        {
            Assert.That( Round(new Vector3D(x, y, z).GetAzimuthalAngle(), 3),Is.EqualTo(expected_angle), name);
        }

        [Test]
        [TestCase(0, 0, 1, 0, "positive Z axis")]
        [TestCase(0, 0, -1, 180, "negative Z axis")]
        [TestCase(1, 0, 0, 90, "positive X axis")]
        [TestCase(-1, 0, 0, 90, "negative X axis")]
        [TestCase(0, 1, 0, 90, "positive Y axis")]
        [TestCase(0, -1, 0, 90, "negative Y axis")]
        [TestCase(1, 0, 1, 45.0, "1,1 in the XZ plane")]
        public static void GetPolarAngle(double x, double y, double z, double expected_angle, string name)
        {
            Assert.That(Round(new Vector3D(x, y, z).GetPolarAngle(), 3), Is.EqualTo(expected_angle), name);
        }

        [Test]
        [TestCase(0, 0, -1, 0, "positive Z axis")]
        [TestCase(0, 0, -1, 0, "negative Z axis")]
        [TestCase(1, 0, 0, -90, "positive X axis")]
        [TestCase(-1, 0, 0, 90, "negative X axis")]
        [TestCase(0, 1, 0, 0, "positive Y axis")]
        [TestCase(0, -1, 0, -180, "negative Y axis")]
        [TestCase(1, 1, 0, -45, "1,1 in XY plane")]
        [TestCase(-1, 1, 0, 45, "-1,1 in XY plane")]
        [TestCase(1, -1, 0, -135.0, "1,-1 in XY plane")]
        [TestCase(-1, -1, 0, 135.0, "-1,-1 in XY plane")]
        public static void GetOrientation(double x, double y, double z, double expected_orientation, string name)
        {
            Assert.That(Round(new Vector3D(x, y, z).GetOrientation(), 3), Is.EqualTo(expected_orientation), name);
        }


    }
}
