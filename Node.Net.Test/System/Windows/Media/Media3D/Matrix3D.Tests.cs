using NUnit.Framework;
using System;
using System.Windows.Media.Media3D;
using static System.Math;
using Node.Net;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class Matrix3DTests
    {
        private const double Tolerance = 1e-9;

        [Test]
        public static void Constructor_Default_CreatesIdentityMatrix()
        {
            Matrix3D matrix = new Matrix3D();
            
            Assert.That(matrix.IsIdentity, Is.True);
            Assert.That(matrix.M11, Is.EqualTo(1.0));
            Assert.That(matrix.M22, Is.EqualTo(1.0));
            Assert.That(matrix.M33, Is.EqualTo(1.0));
            Assert.That(matrix.M44, Is.EqualTo(1.0));
            Assert.That(matrix.OffsetX, Is.EqualTo(0.0));
            Assert.That(matrix.OffsetY, Is.EqualTo(0.0));
            Assert.That(matrix.OffsetZ, Is.EqualTo(0.0));
        }

        [Test]
        public static void Constructor_WithAllValues_SetsAllProperties()
        {
            Matrix3D matrix = new Matrix3D(
                1.0, 2.0, 3.0, 4.0,
                5.0, 6.0, 7.0, 8.0,
                9.0, 10.0, 11.0, 12.0,
                13.0, 14.0, 15.0, 16.0
            );
            
            Assert.That(matrix.M11, Is.EqualTo(1.0));
            Assert.That(matrix.M12, Is.EqualTo(2.0));
            Assert.That(matrix.M13, Is.EqualTo(3.0));
            Assert.That(matrix.M14, Is.EqualTo(4.0));
            Assert.That(matrix.M21, Is.EqualTo(5.0));
            Assert.That(matrix.M22, Is.EqualTo(6.0));
            Assert.That(matrix.M23, Is.EqualTo(7.0));
            Assert.That(matrix.M24, Is.EqualTo(8.0));
            Assert.That(matrix.M31, Is.EqualTo(9.0));
            Assert.That(matrix.M32, Is.EqualTo(10.0));
            Assert.That(matrix.M33, Is.EqualTo(11.0));
            Assert.That(matrix.M34, Is.EqualTo(12.0));
            Assert.That(matrix.OffsetX, Is.EqualTo(13.0));
            Assert.That(matrix.OffsetY, Is.EqualTo(14.0));
            Assert.That(matrix.OffsetZ, Is.EqualTo(15.0));
            Assert.That(matrix.M44, Is.EqualTo(16.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.M11 = 2.0;
            matrix.M12 = 3.0;
            matrix.OffsetX = 10.0;
            
            Assert.That(matrix.M11, Is.EqualTo(2.0));
            Assert.That(matrix.M12, Is.EqualTo(3.0));
            Assert.That(matrix.OffsetX, Is.EqualTo(10.0));
        }

        [Test]
        public static void IsIdentity_IdentityMatrix_ReturnsTrue()
        {
            Matrix3D matrix = new Matrix3D();
            Assert.That(matrix.IsIdentity, Is.True);
        }

        [Test]
        public static void IsIdentity_NonIdentityMatrix_ReturnsFalse()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.M11 = 2.0;
            Assert.That(matrix.IsIdentity, Is.False);
        }

        [Test]
        public static void Determinant_IdentityMatrix_ReturnsOne()
        {
            Matrix3D matrix = new Matrix3D();
            Assert.That(Round(matrix.Determinant, 6), Is.EqualTo(1.0));
        }

        [Test]
        public static void SetIdentity_SetsToIdentity()
        {
            Matrix3D matrix = new Matrix3D(
                1.0, 2.0, 3.0, 4.0,
                5.0, 6.0, 7.0, 8.0,
                9.0, 10.0, 11.0, 12.0,
                13.0, 14.0, 15.0, 16.0
            );
            matrix.SetIdentity();
            
            Assert.That(matrix.IsIdentity, Is.True);
        }

        [Test]
        public static void Transform_Point3D_WithIdentity_ReturnsSamePoint()
        {
            Matrix3D matrix = new Matrix3D();
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Point3D result = matrix.Transform(point);
            
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Transform_Point3D_WithTranslation_TranslatesPoint()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Point3D result = matrix.Transform(point);
            
            Assert.That(result.X, Is.EqualTo(11.0));
            Assert.That(result.Y, Is.EqualTo(22.0));
            Assert.That(result.Z, Is.EqualTo(33.0));
        }

        [Test]
        public static void Transform_Vector3D_WithIdentity_ReturnsSameVector()
        {
            Matrix3D matrix = new Matrix3D();
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = matrix.Transform(vector);
            
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Transform_Vector3D_WithTranslation_DoesNotTranslate()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = matrix.Transform(vector);
            
            // Vectors should not be affected by translation
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Translate_ModifiesOffset()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            
            Assert.That(matrix.OffsetX, Is.EqualTo(10.0));
            Assert.That(matrix.OffsetY, Is.EqualTo(20.0));
            Assert.That(matrix.OffsetZ, Is.EqualTo(30.0));
        }

        [Test]
        public static void Translate_MultipleTimes_Accumulates()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            matrix.Translate(new Vector3D(5.0, 5.0, 5.0));
            
            Assert.That(matrix.OffsetX, Is.EqualTo(15.0));
            Assert.That(matrix.OffsetY, Is.EqualTo(25.0));
            Assert.That(matrix.OffsetZ, Is.EqualTo(35.0));
        }

        [Test]
        public static void Scale_ScalesMatrix()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Scale(new Vector3D(2.0, 3.0, 4.0));
            
            Assert.That(matrix.M11, Is.EqualTo(2.0));
            Assert.That(matrix.M22, Is.EqualTo(3.0));
            Assert.That(matrix.M33, Is.EqualTo(4.0));
        }

        [Test]
        public static void ScaleAt_ScalesAroundPoint()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 10.0, 10.0));
            matrix.ScaleAt(new Vector3D(2.0, 2.0, 2.0), new Point3D(10.0, 10.0, 10.0));
            
            // After scaling around (10,10,10), the translation should be adjusted
            Point3D point = new Point3D(0.0, 0.0, 0.0);
            Point3D result = matrix.Transform(point);
            
            // Should translate to (-10, -10, -10) then scale, then translate back
            Assert.That(Round(result.X, 6), Is.EqualTo(10.0));
            Assert.That(Round(result.Y, 6), Is.EqualTo(10.0));
            Assert.That(Round(result.Z, 6), Is.EqualTo(10.0));
        }

        [Test]
        public static void Rotate_WithQuaternion_RotatesMatrix()
        {
            Matrix3D matrix = new Matrix3D();
            Quaternion q = new Quaternion(new Vector3D(0, 0, 1), 90);
            matrix.Rotate(q);
            
            // 90 degree rotation around Z axis
            Vector3D vector = matrix.Transform(new Vector3D(1.0, 0.0, 0.0));
            Assert.That(Round(vector.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(vector.Y, 6), Is.EqualTo(1.0));
            Assert.That(Round(vector.Z, 6), Is.EqualTo(0.0));
        }

        [Test]
        public static void Invert_IdentityMatrix_RemainsIdentity()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Invert();
            
            Assert.That(matrix.IsIdentity, Is.True);
        }

        [Test]
        public static void Invert_ThenInvert_ReturnsOriginal()
        {
            Matrix3D original = new Matrix3D();
            original.Translate(new Vector3D(10.0, 20.0, 30.0));
            original.Rotate(new Quaternion(new Vector3D(0, 0, 1), 45));
            
            Matrix3D inverted = original;
            inverted.Invert();
            inverted.Invert();
            
            // Should be back to original (within tolerance)
            Assert.That(Round(inverted.M11, 6), Is.EqualTo(Round(original.M11, 6)));
            Assert.That(Round(inverted.OffsetX, 6), Is.EqualTo(Round(original.OffsetX, 6)));
        }

        [Test]
        public static void Invert_ZeroDeterminant_ThrowsException()
        {
            Matrix3D matrix = new Matrix3D(
                1.0, 2.0, 3.0, 4.0,
                2.0, 4.0, 6.0, 8.0, // Linearly dependent rows
                1.0, 1.0, 1.0, 1.0,
                1.0, 1.0, 1.0, 1.0
            );
            
            Assert.Throws<InvalidOperationException>(() => matrix.Invert());
        }

        [Test]
        public static void Append_MultipliesMatrices()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            m1.Append(m2);
            Point3D point = m1.Transform(new Point3D(0.0, 0.0, 0.0));
            
            // Should apply m2 then m1: translate Y then X
            Assert.That(point.X, Is.EqualTo(10.0));
            Assert.That(point.Y, Is.EqualTo(20.0));
            Assert.That(point.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Prepend_MultipliesMatrices()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            m1.Prepend(m2);
            Point3D point = m1.Transform(new Point3D(0.0, 0.0, 0.0));
            
            // Should apply m1 then m2: translate X then Y
            Assert.That(point.X, Is.EqualTo(10.0));
            Assert.That(point.Y, Is.EqualTo(20.0));
            Assert.That(point.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Multiply_StaticMethod_MultipliesCorrectly()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            Matrix3D result = Matrix3D.Multiply(m1, m2);
            Point3D point = result.Transform(new Point3D(0.0, 0.0, 0.0));
            
            Assert.That(point.X, Is.EqualTo(10.0));
            Assert.That(point.Y, Is.EqualTo(20.0));
        }

        [Test]
        public static void OperatorMultiply_MultipliesMatrices()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            Matrix3D result = m1 * m2;
            Point3D point = result.Transform(new Point3D(0.0, 0.0, 0.0));
            
            Assert.That(point.X, Is.EqualTo(10.0));
            Assert.That(point.Y, Is.EqualTo(20.0));
        }

        [Test]
        public static void OperatorEquality_EqualMatrices_ReturnsTrue()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            Assert.That(m1 == m2, Is.True);
        }

        [Test]
        public static void OperatorEquality_DifferentMatrices_ReturnsFalse()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            m2.M11 = 2.0;
            
            Assert.That(m1 == m2, Is.False);
        }

        [Test]
        public static void OperatorInequality_EqualMatrices_ReturnsFalse()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            Assert.That(m1 != m2, Is.False);
        }

        [Test]
        public static void Equals_WithSameValues_ReturnsTrue()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            Assert.That(m1.Equals(m2), Is.True);
        }

        [Test]
        public static void Equals_WithDifferentValues_ReturnsFalse()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            m2.M11 = 2.0;
            
            Assert.That(m1.Equals(m2), Is.False);
        }

        [Test]
        public static void GetHashCode_SameValues_ReturnsSameHashCode()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            Assert.That(m1.GetHashCode(), Is.EqualTo(m2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Matrix3D matrix = new Matrix3D();
            // Windows Matrix3D only supports ToString() without parameters
            string result = matrix.ToString();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Length, Is.GreaterThan(0));
        }

        [Test]
        public static void Transform_IdentityMatrix_ReturnsSameVector()
        {
            Matrix3D identity = new Matrix3D();
            Vector3D input = new Vector3D(1.0, 0.0, 0.0);
            Vector3D result = identity.Transform(input);
            
            // Identity matrix should transform vector to itself
            Assert.That(Round(result.X, 4), Is.EqualTo(1.0));
            Assert.That(Round(result.Y, 4), Is.EqualTo(0.0));
            Assert.That(Round(result.Z, 4), Is.EqualTo(0.0));
        }

        [Test]
        public static void GetOrientation_IdentityMatrix_ReturnsZero()
        {
            Matrix3D identity = new Matrix3D();
            double orientation = identity.GetOrientation();
            
            // Identity matrix should have zero orientation
            Assert.That(Round(orientation, 4), Is.EqualTo(0.0));
            Assert.That(double.IsNaN(orientation), Is.False);
        }

        [Test]
        public static void GetTilt_IdentityMatrix_ReturnsZero()
        {
            Matrix3D identity = new Matrix3D();
            double tilt = identity.GetTilt();
            
            // Identity matrix should have zero tilt
            Assert.That(Round(tilt, 4), Is.EqualTo(0.0));
            Assert.That(double.IsNaN(tilt), Is.False);
        }

        [Test]
        public static void GetSpin_IdentityMatrix_ReturnsZero()
        {
            Matrix3D identity = new Matrix3D();
            double spin = identity.GetSpin();
            
            // Identity matrix should have zero spin
            Assert.That(Round(spin, 4), Is.EqualTo(0.0));
            Assert.That(double.IsNaN(spin), Is.False);
        }

        [Test]
        public static void RotateXYZ_ZeroRotations_LeavesMatrixAsIdentity()
        {
            Matrix3D identity = new Matrix3D();
            Vector3D zeroRotations = new Vector3D(0.0, 0.0, 0.0);
            Matrix3D result = identity.RotateXYZ(zeroRotations);
            
            // Rotating by zero should leave matrix as identity
            Assert.That(result.IsIdentity, Is.True);
            Assert.That(result.GetOrientation(), Is.EqualTo(0.0));
            Assert.That(result.GetTilt(), Is.EqualTo(0.0));
            Assert.That(result.GetSpin(), Is.EqualTo(0.0));
        }

        [Test]
        public static void Translate_ZeroVector_LeavesMatrixAsIdentity()
        {
            Matrix3D identity = new Matrix3D();
            Vector3D zeroTranslation = new Vector3D(0.0, 0.0, 0.0);
            identity.Translate(zeroTranslation);
            
            // Translating by zero should leave matrix as identity
            Assert.That(identity.IsIdentity, Is.True);
        }
    }
}

