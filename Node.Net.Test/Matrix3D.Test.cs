using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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

            var m4 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            Assert.AreEqual(m3, m4, "m3 is not equal to m4");
            var m4s = m4.ToString();
            Assert.AreEqual(192, m4s.Length);
        }

        [Test]
        public void GetValues()
        {
            var m1 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            var m2 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));

            var m1values = m1.GetValues(4);
            var m2values = m2.GetValues(4);
            Assert.AreEqual(m1values, m2values);
        }

        [Test]
        public void AlmostEqual()
        {
            var m1 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            var m2 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));

            Assert.True(m1.AlmostEqual(m2));

            var m3 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 6));
            Assert.False(m1.AlmostEqual(m3));
        }

        [Test]
        public void Tilt()
        {
            var m0 = new Matrix3D().RotateOTS(new Vector3D(0, 0,0));
            var mz0 = m0.Transform(new Vector3D(0, 0, -1));
            Assert.AreEqual(-1, Round(mz0.Z, 4), "mz0.Z");

            var m30 = new Matrix3D().RotateOTS(new Vector3D(0, 30, 0));
            var mz30 = m30.Transform(new Vector3D(0, 0, -1));
            Assert.AreEqual(0, Round(mz30.X, 3), "mz30.X");
            Assert.AreEqual(0.5, Round(mz30.Y, 3), "mz30.Y");
            Assert.AreEqual(-0.866, Round(mz30.Z, 3), "mz30.Z");
            var z30 = m30.Transform(new Vector3D(0, 0, 1));
            Assert.AreEqual(0, Round(z30.X, 3), "z30.X");
            Assert.AreEqual(-0.5, Round(z30.Y, 3), "z30.Y");
            Assert.AreEqual(0.866, Round(z30.Z, 3), "z30.Z");
            //Assert.AreEqual(0, Round(z30.GetTheta(),3), "z30 theta");
            //Assert.AreEqual(30, Round(z30.GetPhi(), 3), "z30 Phi");
        }

        [Test]
        public void Tilt_Ray_Plane_Intersection()
        {
            // 30 degree tilt
            var m30 = new Matrix3D().RotateOTS(new Vector3D(0, 30, 0));
            m30.Translate(new Vector3D(0, 0, 10));

            var mzdir30 = m30.Transform(new Vector3D(0, 0, -1));
            Assert.AreEqual(0, Round(mzdir30.X, 3), "mzdir30.X");
            Assert.AreEqual(0.5, Round(mzdir30.Y, 3), "mzdir30.Y");
            Assert.AreEqual(-0.866, Round(mzdir30.Z, 3), "mzdir30.Z");

            var intersect30 = Vector3DExtension.ComputeRayPlaneIntersection(mzdir30, new Vector3D(0, 0, 10),
                new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.AreEqual(0, Round(intersect30.X, 3), "intersect30.X");
            Assert.AreEqual(5.774, Round(intersect30.Y, 3), "intersect30.Y");
            Assert.AreEqual(0, Round(intersect30.Z, 3), "intersect30.Z");

            // 45 degree tilt
            var m45 = new Matrix3D().RotateOTS(new Vector3D(0, 45, 0));
            m45.Translate(new Vector3D(0, 0, 10));
            var mzdir45 = m45.Transform(new Vector3D(0, 0, -1));
            Assert.AreEqual(0, Round(mzdir45.X, 3), "mzdir45.X");
            Assert.AreEqual(0.707, Round(mzdir45.Y, 3), "mzdir45.Y");
            Assert.AreEqual(-0.707, Round(mzdir45.Z, 3), "mzdir45.Z");

            var intersect45 = Vector3DExtension.ComputeRayPlaneIntersection(mzdir45, new Vector3D(0, 0, 10),
                new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.AreEqual(0, Round(intersect45.X, 3), "intersect45.X");
            Assert.AreEqual(10.0, Round(intersect45.Y, 3), "intersect45.Y");
            Assert.AreEqual(0, Round(intersect45.Z, 3), "intersect45.Z");
        }

        [Test]
        public void AlmostEqual2()
        {
            var m1 = new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            var m2 = new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Assert.True(m1.AlmostEqual(m2));
            var m3 = new Matrix3D().RotateOTS(new Vector3D(15, 12, 0));
            Assert.False(m1.AlmostEqual(m3));
        }
    }
}