using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using NUnit.Framework;
using static System.Math;

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

        [Test]
        public void Rotate()
        {
            var m1 = new Matrix3D().RotateOTS(new Vector3D(15,0,0));
            var xvec1 = m1.Transform(new Vector3D(1, 0, 0));
            Assert.AreEqual(0.966, Round(xvec1.X, 3), "xvec1.X");
            Assert.AreEqual(0.259, Round(xvec1.Y, 3), "xvec1.Y");
            Assert.AreEqual(0.000, Round(xvec1.Z, 3), "xvec1.Z");
            var ots1 = m1.GetRotationsOTS();
            Assert.AreEqual(15.0, Round(ots1.X,3), "ots1.X");
            Assert.AreEqual(0.0, Round(ots1.Y, 3), "ots1.Y");
            Assert.AreEqual(0.0, Round(ots1.Z, 3), "ots1.Z");

            var m2 = new Matrix3D().RotateOTS(new Vector3D(15, -60,0));
            var zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            Assert.AreEqual(-0.224, Round(zvec2.X, 3), "zvec2.X");
            Assert.AreEqual(0.837, Round(zvec2.Y, 3), "zvec2.Y");
            Assert.AreEqual(0.500, Round(zvec2.Z, 3), "zvec2.Z");
            var yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            Assert.AreEqual(-0.129, Round(yvec2.X, 3), "yvec2.X");
            Assert.AreEqual(0.483, Round(yvec2.Y, 3), "yvec2.Y");
            Assert.AreEqual(-0.866, Round(yvec2.Z, 3), "yvec2.Z");
            var ots2 = m2.GetRotationsOTS();
            Assert.AreEqual(15.0, Round(ots2.X, 3), "ots2.X");
            Assert.AreEqual(60.0, Round(ots2.Y, 3), "ots2.Y");
            Assert.AreEqual(0.0, Round(ots2.Z, 3), "ots2.Z");

            var m3 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            var zvec3 = m3.Transform(new Vector3D(0, 0, 1));
            Assert.AreEqual(-0.224, Round(zvec3.X, 3), "zvec3.X");
            Assert.AreEqual(0.837, Round(zvec3.Y, 3), "zvec3.Y");
            Assert.AreEqual(0.500, Round(zvec3.Z, 3), "zvec3.Z");
            var yvec3 = m3.Transform(new Vector3D(0, 1, 0));
            Assert.AreEqual(-0.213, Round(yvec3.X, 3), "yvec3.X");
            Assert.AreEqual(0.459, Round(yvec3.Y, 3), "yvec3.Y");
            Assert.AreEqual(-0.863, Round(yvec3.Z, 3), "yvec3.Z");
            var ots3 = m3.GetRotationsOTS();
            Assert.AreEqual(18.016, Round(ots3.X, 3), "ots3.X");
            Assert.AreEqual(59.868, Round(ots3.Y, 3), "ots3.Y");
            Assert.AreEqual(4.359, Round(ots3.Z, 3), "ots3.Z");
        }
    }
}