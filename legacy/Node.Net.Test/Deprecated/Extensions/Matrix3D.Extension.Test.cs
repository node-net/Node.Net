using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Deprecated.Extensions
{
    [TestFixture, Category(nameof(Extensions))]
    class Matrix3DTest
    {
        [Test]
        public void Matrix3DExtension_ZAxis_Rotation()
        {
            var matrix = new Matrix3D();
            var parent_point = matrix.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(1, parent_point.X, "parent_point.X, 0");

            var matrix_45 = matrix.RotateLocal(new Vector3D(0, 0, 1), 45);
            parent_point = matrix_45.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(0.7071, Round(parent_point.X, 4), "parent_point.X, 45");
            Assert.AreEqual(0, Round(parent_point.Z, 4), "parent_point.Z, 45");

            var matrix_180 = matrix.RotateLocal(new Vector3D(0, 0, 1), 180);
            parent_point = matrix_180.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(-1, Round(parent_point.X, 4), "parent_point.X, 180");
        }

        [Test]
        public void Matrix3DExtension_ZY_Axis_Rotation()
        {
            var matrix = new Matrix3D();
            var matrix_45 = matrix.RotateLocal(new Vector3D(0, 0, 1), 45);
            var matrix_45_45 = matrix_45.RotateLocal(new Vector3D(0, 1, 0), 45);
            var parent_point = matrix_45_45.Transform(new Point3D(1, 0, 0));
            Assert.AreEqual(-0.5, Round(parent_point.Z, 4), "parent_point.Z, 45_45");
        }

        [Test]
        [TestCase(1, 0,  0,      1,      0)]
        [TestCase(1, 0, 45, 0.7071, 0.7071)]
        [TestCase(1, 0, 90,      0,     90)]
        public void Matrix3DExtension_ZAxis_Rotation(double x,double y,double rotation,double rotated_x,double rotated_y)
        {
            var matrix = new Matrix3D().RotateLocal(new Vector3D(0, 0, 1), rotation);
            var rotated = matrix.Transform(new Point3D(x, y, 0));
            Assert.AreEqual(rotated_x, Round(rotated.X, 4), "rotated_x");
        }
    }
}
