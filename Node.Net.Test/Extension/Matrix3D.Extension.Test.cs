#if IS_WINDOWS
using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

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
            Matrix3D matrix = new Matrix3D();
            matrix = matrix.RotateOTS(new Vector3D(orientation, tilt, spin));
            Vector3D ots = matrix.GetRotationsOTS();
            Assert.That(Round(ots.X, 2),Is.EqualTo(orientation), "orientation");
            Assert.That(Round(ots.Y, 2),Is.EqualTo(tilt), "tilt");
            Assert.That(Round(ots.Z, 2), Is.EqualTo(spin), "spin");
        }
    }
}
#endif