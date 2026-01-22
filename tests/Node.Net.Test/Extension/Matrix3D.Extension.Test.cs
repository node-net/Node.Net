using NUnit.Framework;
using static System.Math;
using Node.Net; // This brings the extension methods into scope (they're in Node.Net namespace, not Node.Net.Extension)

namespace Node.Net.Extension
{
    [TestFixture]
    internal static class Matrix3DExtensionTest
    {
        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(45, 30, 0)]
        [TestCase(45, 30, 0)]
        public static void RotateOTS(double orientation, double tilt, double spin)
        {
            System.Windows.Media.Media3D.Matrix3D matrix = new System.Windows.Media.Media3D.Matrix3D();
            matrix = matrix.RotateOTS(new System.Windows.Media.Media3D.Vector3D(orientation, tilt, spin)); // Extension method from Node.Net.Extension
            System.Windows.Media.Media3D.Vector3D ots = matrix.GetRotationsOTS();
            Assert.That(Round(ots.X, 2),Is.EqualTo(orientation), "orientation");
            Assert.That(Round(ots.Y, 2),Is.EqualTo(tilt), "tilt");
            Assert.That(Round(ots.Z, 2), Is.EqualTo(spin), "spin");
        }
    }
}