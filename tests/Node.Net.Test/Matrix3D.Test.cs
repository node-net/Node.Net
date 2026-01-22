using NUnit.Framework;
using System.Collections.Generic;
using static System.Math;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net
{
    [TestFixture]
    internal class Matrix3DTest
    {
        [Test]
        public void Identity()
        {
            Matrix3D matrix = new Matrix3D();
            Assert.That(matrix.IsIdentity,Is.True);
            Assert.That(matrix.M11,Is.EqualTo(1.0), "M11");
            Assert.That(matrix.M12,Is.EqualTo(0.0), "M12");
            Assert.That(matrix.M13, Is.EqualTo(0.0), "M13");
            Assert.That(matrix.M14, Is.EqualTo(0.0), "M14");
            Assert.That(matrix.M21, Is.EqualTo(0.0), "M21");
            Assert.That(matrix.M22, Is.EqualTo(1.0), "M22");
            Assert.That(matrix.M23, Is.EqualTo(0.0), "M23");
            Assert.That(matrix.M24, Is.EqualTo(0.0), "M24");
            Assert.That(matrix.M31, Is.EqualTo(0.0), "M31");
            Assert.That(matrix.M32, Is.EqualTo(0.0), "M32");
            Assert.That(matrix.M33, Is.EqualTo(1.0), "M33");
            Assert.That(matrix.M34, Is.EqualTo(0.0), "M34");
            Assert.That(matrix.OffsetX, Is.EqualTo(0.0), "OffsetX");
            Assert.That(matrix.OffsetY, Is.EqualTo(0.0), "OffsetY");
            Assert.That(matrix.OffsetZ, Is.EqualTo(0.0), "OffsetZ");
            Assert.That(matrix.M44, Is.EqualTo(1.0), "M44");
            Assert.That(matrix.Determinant, Is.EqualTo(1.0), "Determinant");
        }

        [Test]
        public void Determinant()
        {
            Assert.That(new Matrix3D().Determinant,Is.EqualTo(1), "Identity Matrix Determinant");
        }

        [Test]
        public void Translate()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10, 20, 30));
            Assert.That(matrix.IsIdentity,Is.False);
            Assert.That(matrix.M11, Is.EqualTo(1.0), "M11");
            Assert.That(matrix.M12, Is.EqualTo(0.0), "M12");
            Assert.That(matrix.M13, Is.EqualTo(0.0), "M13");
            Assert.That(matrix.M14, Is.EqualTo(0.0), "M14");
            Assert.That(matrix.M21, Is.EqualTo(0.0), "M21");
            Assert.That(matrix.M22, Is.EqualTo(1.0), "M22");
            Assert.That(matrix.M23, Is.EqualTo(0.0), "M23");
            Assert.That(matrix.M24, Is.EqualTo(0.0), "M24");
            Assert.That(matrix.M31, Is.EqualTo(0.0), "M31");
            Assert.That(matrix.M32, Is.EqualTo(0.0), "M32");
            Assert.That(matrix.M33, Is.EqualTo(1.0), "M33");
            Assert.That(matrix.M34, Is.EqualTo(0.0), "M34");
            Assert.That(matrix.OffsetX, Is.EqualTo(10.0), "OffsetX");
            Assert.That(matrix.OffsetY, Is.EqualTo(20.0), "OffsetY");
            Assert.That(matrix.OffsetZ, Is.EqualTo(30.0), "OffsetZ");
            Assert.That(matrix.M44, Is.EqualTo(1.0), "M44");
            Assert.That(matrix.Determinant, Is.EqualTo(1.0), "Determinant");

            Point3D position = matrix.Transform(new Point3D(0, 0, 0));
            Assert.That(position.X, Is.EqualTo(10), "position.X");
            Assert.That(position.Y, Is.EqualTo(20), "position.Y");
            Assert.That(position.Z, Is.EqualTo(30), "position.Z");
        }

        [Test]
        public void Rotate()
        {
            Matrix3D m1 = new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Vector3D xvec1 = m1.Transform(new Vector3D(1, 0, 0));
            Assert.That(Round(xvec1.X, 3), Is.EqualTo(0.966), "xvec1.X");
            Assert.That(Round(xvec1.Y, 3), Is.EqualTo(0.259), "xvec1.Y");
            Assert.That(Round(xvec1.Z, 3), Is.EqualTo(0.000), "xvec1.Z");
            Vector3D ots1 = m1.GetRotationsOTS();
            Assert.That(Round(ots1.X, 3), Is.EqualTo(15.0), "ots1.X");
            Assert.That(Round(ots1.Y, 3), Is.EqualTo(0.0), "ots1.Y");
            Assert.That(Round(ots1.Z, 3), Is.EqualTo(0.0), "ots1.Z");

            Matrix3D m2 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 0));
            Vector3D zvec2 = m2.Transform(new Vector3D(0, 0, 1));
            Assert.That(Round(zvec2.X, 3), Is.EqualTo(-0.224), "zvec2.X");
            Assert.That(Round(zvec2.Y, 3), Is.EqualTo(0.837), "zvec2.Y");
            Assert.That(Round(zvec2.Z, 3), Is.EqualTo(0.500), "zvec2.Z");
            Vector3D yvec2 = m2.Transform(new Vector3D(0, 1, 0));
            Assert.That(Round(yvec2.X, 3), Is.EqualTo(-0.129), "yvec2.X");
            Assert.That(Round(yvec2.Y, 3), Is.EqualTo(0.483), "yvec2.Y");
            Assert.That(Round(yvec2.Z, 3), Is.EqualTo(-0.866), "yvec2.Z");
            Vector3D ots2 = m2.GetRotationsOTS();
            Assert.That(Round(ots2.X, 3), Is.EqualTo(15.0), "ots2.X");
            Assert.That(Round(ots2.Y, 3), Is.EqualTo(-60.0), "ots2.Y");
            Assert.That(Round(ots2.Z, 3), Is.EqualTo(0.0), "ots2.Z");

            Matrix3D m3 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            Vector3D zvec3 = m3.Transform(new Vector3D(0, 0, 1));
            Assert.That(Round(zvec3.X, 3), Is.EqualTo(-0.224), "zvec3.X");
            Assert.That(Round(zvec3.Y, 3), Is.EqualTo(0.837), "zvec3.Y");
            Assert.That(Round(zvec3.Z, 3), Is.EqualTo(0.500), "zvec3.Z");
            Vector3D yvec3 = m3.Transform(new Vector3D(0, 1, 0));
            Assert.That(Round(yvec3.X, 3), Is.EqualTo(-0.213), "yvec3.X");
            Assert.That(Round(yvec3.Y, 3), Is.EqualTo(0.459), "yvec3.Y");
            Assert.That(Round(yvec3.Z, 3), Is.EqualTo(-0.863), "yvec3.Z");
            Vector3D ots3 = m3.GetRotationsOTS();
            Assert.That(Round(ots3.X, 3), Is.EqualTo(18.016), "ots3.X");
            Assert.That(Round(ots3.Y, 3), Is.EqualTo(-59.868), "ots3.Y");
            Assert.That(Round(ots3.Z, 3), Is.EqualTo(-4.359), "ots3.Z");

            Matrix3D m4 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            Assert.That(m4, Is.EqualTo(m3), "m3 is not equal to m4");
            string m4s = m4.ToString();
            Assert.That(m4s.Length,Is.EqualTo(192));
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(45, 0, 0)]
        [TestCase(135, 0, 0)]
        [TestCase(135, 55, 0)]
        [TestCase(45, 15, 0)]
        [TestCase(45, -15, 0)]
        [TestCase(-45, 15, 0)]
        public void RotateOTS(double orientation, double tilt, double spin)
        {
            Matrix3D mA = new Matrix3D().RotateOTS(new Vector3D(orientation, tilt, spin));
            double o_check = mA.GetOrientation();
            double t_check = mA.GetTilt();
            double s_check = mA.GetSpin();

            Assert.That(Round(o_check, 4), Is.EqualTo(orientation), "orientation");
            Assert.That(Round(t_check, 4), Is.EqualTo(tilt), "tilt");
            Assert.That(Round(s_check, 4), Is.EqualTo(spin), "spin");
        }

        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(45, 0, 0)]
        [TestCase(45, 30, 0)]
        [TestCase(45, -30, 0)]
        public void RotateOTS_With_Offset(double orientation, double tilt, double spin)
        {
            Matrix3D mA = new Matrix3D();
            mA.Translate(new Vector3D(10, 20, 30));
            mA = mA.RotateOTS(new Vector3D(orientation, tilt, spin));

            double o_check = mA.GetOrientation();
            double t_check = mA.GetTilt();
            double s_check = mA.GetSpin();

            Assert.That(Round(o_check, 4), Is.EqualTo(orientation), "orientation");
            Assert.That(Round(t_check, 4), Is.EqualTo(tilt), "tilt");
            Assert.That(Round(s_check, 4), Is.EqualTo(spin), "spin");
        }

        [Test]
        public void GetValues()
        {
            Matrix3D m1 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            Matrix3D m2 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));

            List<double> m1values = m1.GetValues(4);
            List<double> m2values = m2.GetValues(4);
            Assert.That(m1values, Is.EqualTo(m2values));
        }

        [Test]
        public void AlmostEqual()
        {
            Matrix3D m1 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));
            Matrix3D m2 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 5));

            Assert.That(m1.AlmostEqual(m2),Is.True);

            Matrix3D m3 = new Matrix3D().RotateOTS(new Vector3D(15, -60, 6));
            Assert.That(m1.AlmostEqual(m3), Is.False);
        }

        [Test]
        public void Tilt()
        {
            Matrix3D m0 = new Matrix3D().RotateOTS(new Vector3D(0, 0, 0));
            Vector3D mz0 = m0.Transform(new Vector3D(0, 0, -1));
            Assert.That(Round(mz0.Z, 4), Is.EqualTo(-1), "mz0.Z");

            Matrix3D m30 = new Matrix3D().RotateOTS(new Vector3D(0, 30, 0));
            Vector3D mz30 = m30.Transform(new Vector3D(0, 0, -1));
            Assert.That(Round(mz30.X, 3), Is.EqualTo(0), "mz30.X");
            Assert.That(Round(mz30.Y, 3), Is.EqualTo(0.5), "mz30.Y");
            Assert.That(Round(mz30.Z, 3), Is.EqualTo(-0.866), "mz30.Z");
            Vector3D z30 = m30.Transform(new Vector3D(0, 0, 1));
            Assert.That(Round(z30.X, 3), Is.EqualTo(0), "z30.X");
            Assert.That(Round(z30.Y, 3), Is.EqualTo(-0.5), "z30.Y");
            Assert.That(Round(z30.Z, 3), Is.EqualTo(0.866), "z30.Z");
            //Assert.AreEqual(0, Round(z30.GetTheta(),3), "z30 theta");
            //Assert.AreEqual(30, Round(z30.GetPhi(), 3), "z30 Phi");
        }

        [Test]
        public void Tilt_Ray_Plane_Intersection()
        {
            // 30 degree tilt
            Matrix3D m30 = new Matrix3D().RotateOTS(new Vector3D(0, 30, 0));
            m30.Translate(new Vector3D(0, 0, 10));

            Vector3D mzdir30 = m30.Transform(new Vector3D(0, 0, -1));
            Assert.That(Round(mzdir30.X, 3), Is.EqualTo(0), "mzdir30.X");
            Assert.That(Round(mzdir30.Y, 3), Is.EqualTo(0.5), "mzdir30.Y");
            Assert.That(Round(mzdir30.Z, 3), Is.EqualTo(-0.866), "mzdir30.Z");

            Vector3D intersect30 = Vector3DExtension.ComputeRayPlaneIntersection(mzdir30, new Vector3D(0, 0, 10),
                new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.That(Round(intersect30.X, 3), Is.EqualTo(0), "intersect30.X");
            Assert.That(Round(intersect30.Y, 3), Is.EqualTo(5.774), "intersect30.Y");
            Assert.That(Round(intersect30.Z, 3), Is.EqualTo(0), "intersect30.Z");

            // 45 degree tilt
            Matrix3D m45 = new Matrix3D().RotateOTS(new Vector3D(0, 45, 0));
            m45.Translate(new Vector3D(0, 0, 10));
            Vector3D mzdir45 = m45.Transform(new Vector3D(0, 0, -1));
            Assert.That(Round(mzdir45.X, 3), Is.EqualTo(0), "mzdir45.X");
            Assert.That(Round(mzdir45.Y, 3), Is.EqualTo(0.707), "mzdir45.Y");
            Assert.That(Round(mzdir45.Z, 3), Is.EqualTo(-0.707), "mzdir45.Z");

            Vector3D intersect45 = Vector3DExtension.ComputeRayPlaneIntersection(mzdir45, new Vector3D(0, 0, 10),
                new Vector3D(0, 0, 1), new Vector3D(0, 0, 0));
            Assert.That(Round(intersect45.X, 3), Is.EqualTo(0), "intersect45.X");
            Assert.That(Round(intersect45.Y, 3), Is.EqualTo(10.0), "intersect45.Y");
            Assert.That(Round(intersect45.Z, 3), Is.EqualTo(0), "intersect45.Z");

            Vector3D zDirectionVector = m45.GetZDirectionVector();
            Assert.That(Round(zDirectionVector.X, 3), Is.EqualTo(0), "zDirectionVector.X");
            Assert.That(Round(zDirectionVector.Y, 3), Is.EqualTo(-0.707), "zDirectionVector.Y");
            Assert.That(Round(zDirectionVector.Z, 3), Is.EqualTo(0.707), "zDirectionVector.Z");

            Vector3D yDirectionVector = m45.GetYDirectionVector();
            Assert.That(Round(yDirectionVector.X, 3), Is.EqualTo(0), "yDirectionVector.X");
            Assert.That(Round(yDirectionVector.Y, 3), Is.EqualTo(0.707), "yDirectionVector.Y");
            Assert.That(Round(yDirectionVector.Z, 3), Is.EqualTo(0.707), "yDirectionVector.Z");

            Vector3D xDirectionVector = m45.GetXDirectionVector();
            Assert.That(Round(xDirectionVector.X, 3), Is.EqualTo(1.0), "xDirectionVector.X");
            Assert.That(Round(xDirectionVector.Y, 3), Is.EqualTo(0.0), "xDirectionVector.Y");
            Assert.That(Round(xDirectionVector.Z, 3), Is.EqualTo(0.0), "xDirectionVector.Z");
        }

        /*
        [Test]
        public void Align_ZAxis()
        {
            // 30 degree tilt
            var m30 = new Matrix3D().RotateOTS(new Vector3D(0, 30, 0));
            m30.Translate(new Vector3D(0, 0, 10));

            var zDirectionVector = new Vector3D(0, -0.7069999, 0.7069999);
            Assert.AreEqual(90.0, Round(zDirectionVector.GetOrientation(), 3), "z direction orientation");
            var m45 = m30.AlignZDirectionVector(zDirectionVector);

            Assert.AreEqual(45.0, Round(m45.GetTilt(), 3), "tilt");

            zDirectionVector = new Vector3D(-0.7069999, 0.7069999, 0.7069999);
            var mTest = m45.AlignZDirectionVector(zDirectionVector);
            Assert.AreEqual(135.0, Round(mTest.GetOrientation(), 3), "Orientation");
            Assert.AreEqual(11, Round(mTest.GetTilt(), 3), "Tilt");
        }
        */

        [Test]
        public void AlmostEqual2()
        {
            Matrix3D m1 = new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Matrix3D m2 = new Matrix3D().RotateOTS(new Vector3D(15, 0, 0));
            Assert.That(m1.AlmostEqual(m2),Is.True);
            Matrix3D m3 = new Matrix3D().RotateOTS(new Vector3D(15, 12, 0));
            Assert.That(m1.AlmostEqual(m3), Is.False);
        }


    }
}