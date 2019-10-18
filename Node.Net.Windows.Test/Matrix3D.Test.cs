using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using NUnit.Framework;

namespace Node.Net.Windows.Test
{
    [TestFixture]
    internal class Matrix3DTest
    {
        [Test]
        public void Identity()
        {
            var matrix = new Matrix3D();
            Assert.True(matrix.IsIdentity, "matrix.IsIdentity");
            Assert.AreEqual(1, matrix.M11, "matrix.M11");
            Assert.AreEqual(0, matrix.M12, "matrix.M12");
            Assert.AreEqual(0, matrix.M13, "matrix.M13");
            Assert.AreEqual(0, matrix.M14, "matrix.M14");
            Assert.AreEqual(0, matrix.M21, "matrix.M21");
            Assert.AreEqual(1, matrix.M22, "matrix.M22");
            Assert.AreEqual(0, matrix.M23, "matrix.M23");
            Assert.AreEqual(0, matrix.M24, "matrix.M24");
            Assert.AreEqual(0, matrix.M31, "matrix.M31");
            Assert.AreEqual(0, matrix.M32, "matrix.M32");
            Assert.AreEqual(1, matrix.M33, "matrix.M33");
            Assert.AreEqual(0, matrix.M34, "matrix.M34");
            Assert.AreEqual(1, matrix.M44, "matrix.M44");
            Assert.AreEqual(0, matrix.OffsetX, "matrix.OffsetX");
            Assert.AreEqual(0, matrix.OffsetY, "matrix.OffsetY");
            Assert.AreEqual(0, matrix.OffsetZ, "matrix.OffsetZ");

            var point = matrix.Transform(new Point3D());
            Assert.AreEqual(0, point.X, "point.X");
            Assert.AreEqual(0, point.Y, "point.Y");
            Assert.AreEqual(0, point.Z, "point.Z");
        }

        [Test]
        public void Translate()
        {
            var matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10, 20, 30));
            Assert.False(matrix.IsIdentity, "matrix.IsIdentity");
            Assert.AreEqual(1, matrix.M11, "matrix.M11");
            Assert.AreEqual(0, matrix.M12, "matrix.M12");
            Assert.AreEqual(0, matrix.M13, "matrix.M13");
            Assert.AreEqual(0, matrix.M14, "matrix.M14");
            Assert.AreEqual(0, matrix.M21, "matrix.M21");
            Assert.AreEqual(1, matrix.M22, "matrix.M22");
            Assert.AreEqual(0, matrix.M23, "matrix.M23");
            Assert.AreEqual(0, matrix.M24, "matrix.M24");
            Assert.AreEqual(0, matrix.M31, "matrix.M31");
            Assert.AreEqual(0, matrix.M32, "matrix.M32");
            Assert.AreEqual(1, matrix.M33, "matrix.M33");
            Assert.AreEqual(0, matrix.M34, "matrix.M34");
            Assert.AreEqual(1, matrix.M44, "matrix.M44");
            Assert.AreEqual(10, matrix.OffsetX, "matrix.OffsetX");
            Assert.AreEqual(20, matrix.OffsetY, "matrix.OffsetY");
            Assert.AreEqual(30, matrix.OffsetZ, "matrix.OffsetZ");
        }
    }
}
