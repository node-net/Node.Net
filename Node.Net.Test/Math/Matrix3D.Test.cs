using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Math
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
        }

        [Test]
		public void Usage()
		{
			var identity = new Matrix3D();
		}
	}
}
