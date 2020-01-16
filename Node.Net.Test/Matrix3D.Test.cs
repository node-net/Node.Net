using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    internal class Matrix3DTest
    {
        [Test]
        public void Identity()
        {
            var matrix = new Matrix3D();
            Assert.True(matrix.IsIdentity);
            Assert.AreEqual(1.0, matrix.M11, "M11");
            Assert.AreEqual(0.0, matrix.M12, "M12");
            Assert.AreEqual(0.0, matrix.M13, "M13");
            Assert.AreEqual(0.0, matrix.M14, "M14");
            Assert.AreEqual(0.0, matrix.M21, "M21");
            Assert.AreEqual(1.0, matrix.M22, "M22");
            Assert.AreEqual(0.0, matrix.M23, "M23");
            Assert.AreEqual(0.0, matrix.M24, "M24");
            Assert.AreEqual(0.0, matrix.M31, "M31");
            Assert.AreEqual(0.0, matrix.M32, "M32");
            Assert.AreEqual(1.0, matrix.M33, "M33");
            Assert.AreEqual(0.0, matrix.M34, "M34");
            Assert.AreEqual(0.0, matrix.OffsetX, "OffsetX");
            Assert.AreEqual(0.0, matrix.OffsetY, "OffsetY");
            Assert.AreEqual(0.0, matrix.OffsetZ, "OffsetZ");
            Assert.AreEqual(1.0, matrix.M44, "M44");
            Assert.AreEqual(1.0, matrix.Determinant, "Determinant");
        }

        [Test]
        public void Determinant()
        {
            Assert.AreEqual(1, new Matrix3D().Determinant, "Identity Matrix Determinant");
        }

        [Test]
        public void Translate()
        {
            var matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10, 20, 30));
            Assert.False(matrix.IsIdentity);
            Assert.AreEqual(1.0, matrix.M11, "M11");
            Assert.AreEqual(0.0, matrix.M12, "M12");
            Assert.AreEqual(0.0, matrix.M13, "M13");
            Assert.AreEqual(0.0, matrix.M14, "M14");
            Assert.AreEqual(0.0, matrix.M21, "M21");
            Assert.AreEqual(1.0, matrix.M22, "M22");
            Assert.AreEqual(0.0, matrix.M23, "M23");
            Assert.AreEqual(0.0, matrix.M24, "M24");
            Assert.AreEqual(0.0, matrix.M31, "M31");
            Assert.AreEqual(0.0, matrix.M32, "M32");
            Assert.AreEqual(1.0, matrix.M33, "M33");
            Assert.AreEqual(0.0, matrix.M34, "M34");
            Assert.AreEqual(10.0, matrix.OffsetX, "OffsetX");
            Assert.AreEqual(20.0, matrix.OffsetY, "OffsetY");
            Assert.AreEqual(30.0, matrix.OffsetZ, "OffsetZ");
            Assert.AreEqual(1.0, matrix.M44, "M44");
            Assert.AreEqual(1.0, matrix.Determinant, "Determinant");

            var position = matrix.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(10, position.X, "position.X");
            Assert.AreEqual(20, position.Y, "position.Y");
            Assert.AreEqual(30, position.Z, "position.Z");
        }
    }
}