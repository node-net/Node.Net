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
        }

        [Test]
        public void Determinant()
        {
            Assert.AreEqual(1, new Matrix3D().Determinant, "Identity Matrix Determinant");
        }
    }
}