using System;
using System.Threading.Tasks;
using static System.Math;
using Node.Net;

namespace Node.Net.Test
{
    internal class Matrix3DTests
    {
        private const double Tolerance = 1e-9;

        [Test]
        public async Task Constructor_Default_CreatesIdentityMatrix()
        {
            Matrix3D matrix = new Matrix3D();
            
            await Assert.That(matrix.IsIdentity).IsTrue();
            await Assert.That(matrix.M11).IsEqualTo(1.0);
            await Assert.That(matrix.M22).IsEqualTo(1.0);
            await Assert.That(matrix.M33).IsEqualTo(1.0);
            await Assert.That(matrix.M44).IsEqualTo(1.0);
            await Assert.That(matrix.OffsetX).IsEqualTo(0.0);
            await Assert.That(matrix.OffsetY).IsEqualTo(0.0);
            await Assert.That(matrix.OffsetZ).IsEqualTo(0.0);
        }

        [Test]
        public async Task Constructor_WithAllValues_SetsAllProperties()
        {
            Matrix3D matrix = new Matrix3D(
                1.0, 2.0, 3.0, 4.0,
                5.0, 6.0, 7.0, 8.0,
                9.0, 10.0, 11.0, 12.0,
                13.0, 14.0, 15.0, 16.0
            );
            
            await Assert.That(matrix.M11).IsEqualTo(1.0);
            await Assert.That(matrix.M12).IsEqualTo(2.0);
            await Assert.That(matrix.M13).IsEqualTo(3.0);
            await Assert.That(matrix.M14).IsEqualTo(4.0);
            await Assert.That(matrix.M21).IsEqualTo(5.0);
            await Assert.That(matrix.M22).IsEqualTo(6.0);
            await Assert.That(matrix.M23).IsEqualTo(7.0);
            await Assert.That(matrix.M24).IsEqualTo(8.0);
            await Assert.That(matrix.M31).IsEqualTo(9.0);
            await Assert.That(matrix.M32).IsEqualTo(10.0);
            await Assert.That(matrix.M33).IsEqualTo(11.0);
            await Assert.That(matrix.M34).IsEqualTo(12.0);
            await Assert.That(matrix.OffsetX).IsEqualTo(13.0);
            await Assert.That(matrix.OffsetY).IsEqualTo(14.0);
            await Assert.That(matrix.OffsetZ).IsEqualTo(15.0);
            await Assert.That(matrix.M44).IsEqualTo(16.0);
        }

        [Test]
        public async Task Properties_CanBeSet()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.M11 = 2.0;
            matrix.M12 = 3.0;
            matrix.OffsetX = 10.0;
            
            await Assert.That(matrix.M11).IsEqualTo(2.0);
            await Assert.That(matrix.M12).IsEqualTo(3.0);
            await Assert.That(matrix.OffsetX).IsEqualTo(10.0);
        }

        [Test]
        public async Task IsIdentity_IdentityMatrix_ReturnsTrue()
        {
            Matrix3D matrix = new Matrix3D();
            await Assert.That(matrix.IsIdentity).IsTrue();
        }

        [Test]
        public async Task IsIdentity_NonIdentityMatrix_ReturnsFalse()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.M11 = 2.0;
            await Assert.That(matrix.IsIdentity).IsFalse();
        }

        [Test]
        public async Task Determinant_IdentityMatrix_ReturnsOne()
        {
            Matrix3D matrix = new Matrix3D();
            await Assert.That(Round(matrix.Determinant, 6)).IsEqualTo(1.0);
        }

        [Test]
        public async Task SetIdentity_SetsToIdentity()
        {
            Matrix3D matrix = new Matrix3D(
                1.0, 2.0, 3.0, 4.0,
                5.0, 6.0, 7.0, 8.0,
                9.0, 10.0, 11.0, 12.0,
                13.0, 14.0, 15.0, 16.0
            );
            matrix.SetIdentity();
            
            await Assert.That(matrix.IsIdentity).IsTrue();
        }

        [Test]
        public async Task Transform_Point3D_WithIdentity_ReturnsSamePoint()
        {
            Matrix3D matrix = new Matrix3D();
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Point3D result = matrix.Transform(point);
            
            await Assert.That(result.X).IsEqualTo(1.0);
            await Assert.That(result.Y).IsEqualTo(2.0);
            await Assert.That(result.Z).IsEqualTo(3.0);
        }

        [Test]
        public async Task Transform_Point3D_WithTranslation_TranslatesPoint()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Point3D result = matrix.Transform(point);
            
            await Assert.That(result.X).IsEqualTo(11.0);
            await Assert.That(result.Y).IsEqualTo(22.0);
            await Assert.That(result.Z).IsEqualTo(33.0);
        }

        [Test]
        public async Task Transform_Vector3D_WithIdentity_ReturnsSameVector()
        {
            Matrix3D matrix = new Matrix3D();
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = matrix.Transform(vector);
            
            await Assert.That(result.X).IsEqualTo(1.0);
            await Assert.That(result.Y).IsEqualTo(2.0);
            await Assert.That(result.Z).IsEqualTo(3.0);
        }

        [Test]
        public async Task Transform_Vector3D_WithTranslation_DoesNotTranslate()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = matrix.Transform(vector);
            
            // Vectors should not be affected by translation
            await Assert.That(result.X).IsEqualTo(1.0);
            await Assert.That(result.Y).IsEqualTo(2.0);
            await Assert.That(result.Z).IsEqualTo(3.0);
        }

        [Test]
        public async Task Translate_ModifiesOffset()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            
            await Assert.That(matrix.OffsetX).IsEqualTo(10.0);
            await Assert.That(matrix.OffsetY).IsEqualTo(20.0);
            await Assert.That(matrix.OffsetZ).IsEqualTo(30.0);
        }

        [Test]
        public async Task Translate_MultipleTimes_Accumulates()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            matrix.Translate(new Vector3D(5.0, 5.0, 5.0));
            
            await Assert.That(matrix.OffsetX).IsEqualTo(15.0);
            await Assert.That(matrix.OffsetY).IsEqualTo(25.0);
            await Assert.That(matrix.OffsetZ).IsEqualTo(35.0);
        }

        [Test]
        public async Task Scale_ScalesMatrix()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Scale(new Vector3D(2.0, 3.0, 4.0));
            
            await Assert.That(matrix.M11).IsEqualTo(2.0);
            await Assert.That(matrix.M22).IsEqualTo(3.0);
            await Assert.That(matrix.M33).IsEqualTo(4.0);
        }

        [Test]
        public async Task ScaleAt_ScalesAroundPoint()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 10.0, 10.0));
            matrix.ScaleAt(new Vector3D(2.0, 2.0, 2.0), new Point3D(10.0, 10.0, 10.0));
            
            // After scaling around (10,10,10), the translation should be adjusted
            Point3D point = new Point3D(0.0, 0.0, 0.0);
            Point3D result = matrix.Transform(point);
            
            // Should translate to (-10, -10, -10) then scale, then translate back
            await Assert.That(Round(result.X, 6)).IsEqualTo(10.0);
            await Assert.That(Round(result.Y, 6)).IsEqualTo(10.0);
            await Assert.That(Round(result.Z, 6)).IsEqualTo(10.0);
        }

        [Test]
        public async Task Rotate_WithQuaternion_RotatesMatrix()
        {
            Matrix3D matrix = new Matrix3D();
            Quaternion q = new Quaternion(new Vector3D(0, 0, 1), 90);
            matrix.Rotate(q);
            
            // 90 degree rotation around Z axis
            Vector3D vector = matrix.Transform(new Vector3D(1.0, 0.0, 0.0));
            await Assert.That(Round(vector.X, 6)).IsEqualTo(0.0);
            await Assert.That(Round(vector.Y, 6)).IsEqualTo(1.0);
            await Assert.That(Round(vector.Z, 6)).IsEqualTo(0.0);
        }

        [Test]
        public async Task Invert_IdentityMatrix_RemainsIdentity()
        {
            Matrix3D matrix = new Matrix3D();
            matrix.Invert();
            
            await Assert.That(matrix.IsIdentity).IsTrue();
        }

        [Test]
        public async Task Invert_ThenInvert_ReturnsOriginal()
        {
            Matrix3D original = new Matrix3D();
            original.Translate(new Vector3D(10.0, 20.0, 30.0));
            original.Rotate(new Quaternion(new Vector3D(0, 0, 1), 45));
            
            Matrix3D inverted = original;
            inverted.Invert();
            inverted.Invert();
            
            // Should be back to original (within tolerance)
            await Assert.That(Round(inverted.M11, 6)).IsEqualTo(Round(original.M11, 6));
            await Assert.That(Round(inverted.OffsetX, 6)).IsEqualTo(Round(original.OffsetX, 6));
        }

        [Test]
        public async Task Invert_ZeroDeterminant_ThrowsException()
        {
            Matrix3D matrix = new Matrix3D(
                1.0, 2.0, 3.0, 4.0,
                2.0, 4.0, 6.0, 8.0, // Linearly dependent rows
                1.0, 1.0, 1.0, 1.0,
                1.0, 1.0, 1.0, 1.0
            );
            
            await Assert.That(() => matrix.Invert()).Throws<InvalidOperationException>();
        }

        [Test]
        public async Task Append_MultipliesMatrices()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            m1.Append(m2);
            Point3D point = m1.Transform(new Point3D(0.0, 0.0, 0.0));
            
            // Should apply m2 then m1: translate Y then X
            await Assert.That(point.X).IsEqualTo(10.0);
            await Assert.That(point.Y).IsEqualTo(20.0);
            await Assert.That(point.Z).IsEqualTo(0.0);
        }

        [Test]
        public async Task Prepend_MultipliesMatrices()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            m1.Prepend(m2);
            Point3D point = m1.Transform(new Point3D(0.0, 0.0, 0.0));
            
            // Should apply m1 then m2: translate X then Y
            await Assert.That(point.X).IsEqualTo(10.0);
            await Assert.That(point.Y).IsEqualTo(20.0);
            await Assert.That(point.Z).IsEqualTo(0.0);
        }

        [Test]
        public async Task Multiply_StaticMethod_MultipliesCorrectly()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            Matrix3D result = Matrix3D.Multiply(m1, m2);
            Point3D point = result.Transform(new Point3D(0.0, 0.0, 0.0));
            
            await Assert.That(point.X).IsEqualTo(10.0);
            await Assert.That(point.Y).IsEqualTo(20.0);
        }

        [Test]
        public async Task OperatorMultiply_MultipliesMatrices()
        {
            Matrix3D m1 = new Matrix3D();
            m1.Translate(new Vector3D(10.0, 0.0, 0.0));
            
            Matrix3D m2 = new Matrix3D();
            m2.Translate(new Vector3D(0.0, 20.0, 0.0));
            
            Matrix3D result = m1 * m2;
            Point3D point = result.Transform(new Point3D(0.0, 0.0, 0.0));
            
            await Assert.That(point.X).IsEqualTo(10.0);
            await Assert.That(point.Y).IsEqualTo(20.0);
        }

        [Test]
        public async Task OperatorEquality_EqualMatrices_ReturnsTrue()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            await Assert.That(m1 == m2).IsTrue();
        }

        [Test]
        public async Task OperatorEquality_DifferentMatrices_ReturnsFalse()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            m2.M11 = 2.0;
            
            await Assert.That(m1 == m2).IsFalse();
        }

        [Test]
        public async Task OperatorInequality_EqualMatrices_ReturnsFalse()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            await Assert.That(m1 != m2).IsFalse();
        }

        [Test]
        public async Task Equals_WithSameValues_ReturnsTrue()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            await Assert.That(m1.Equals(m2)).IsTrue();
        }

        [Test]
        public async Task Equals_WithDifferentValues_ReturnsFalse()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            m2.M11 = 2.0;
            
            await Assert.That(m1.Equals(m2)).IsFalse();
        }

        [Test]
        public async Task GetHashCode_SameValues_ReturnsSameHashCode()
        {
            Matrix3D m1 = new Matrix3D();
            Matrix3D m2 = new Matrix3D();
            
            await Assert.That(m1.GetHashCode()).IsEqualTo(m2.GetHashCode());
        }

        [Test]
        public async Task ToString_ReturnsFormattedString()
        {
            Matrix3D matrix = new Matrix3D();
            // Windows Matrix3D only supports ToString() without parameters
            string result = matrix.ToString();
            await Assert.That(result).IsNotNull();
            await Assert.That(result.Length).IsGreaterThan(0);
        }

        [Test]
        public async Task Transform_IdentityMatrix_ReturnsSameVector()
        {
            Matrix3D identity = new Matrix3D();
            Vector3D input = new Vector3D(1.0, 0.0, 0.0);
            Vector3D result = identity.Transform(input);
            
            // Identity matrix should transform vector to itself
            await Assert.That(Round(result.X, 4)).IsEqualTo(1.0);
            await Assert.That(Round(result.Y, 4)).IsEqualTo(0.0);
            await Assert.That(Round(result.Z, 4)).IsEqualTo(0.0);
        }

        [Test]
        public async Task GetOrientation_IdentityMatrix_ReturnsZero()
        {
            Matrix3D identity = new Matrix3D();
            double orientation = identity.GetOrientation();
            
            // Identity matrix should have zero orientation
            await Assert.That(Round(orientation, 4)).IsEqualTo(0.0);
            await Assert.That(double.IsNaN(orientation)).IsFalse();
        }

        [Test]
        public async Task GetTilt_IdentityMatrix_ReturnsZero()
        {
            Matrix3D identity = new Matrix3D();
            double tilt = identity.GetTilt();
            
            // Identity matrix should have zero tilt
            await Assert.That(Round(tilt, 4)).IsEqualTo(0.0);
            await Assert.That(double.IsNaN(tilt)).IsFalse();
        }

        [Test]
        public async Task GetSpin_IdentityMatrix_ReturnsZero()
        {
            Matrix3D identity = new Matrix3D();
            double spin = identity.GetSpin();
            
            // Identity matrix should have zero spin
            await Assert.That(Round(spin, 4)).IsEqualTo(0.0);
            await Assert.That(double.IsNaN(spin)).IsFalse();
        }

        [Test]
        public async Task RotateXYZ_ZeroRotations_LeavesMatrixAsIdentity()
        {
            Matrix3D identity = new Matrix3D();
            Vector3D zeroRotations = new Vector3D(0.0, 0.0, 0.0);
            Matrix3D result = identity.RotateXYZ(zeroRotations);
            
            // Rotating by zero should leave matrix as identity
            await Assert.That(result.IsIdentity).IsTrue();
            await Assert.That(result.GetOrientation()).IsEqualTo(0.0);
            await Assert.That(result.GetTilt()).IsEqualTo(0.0);
            await Assert.That(result.GetSpin()).IsEqualTo(0.0);
        }

        [Test]
        public async Task Translate_ZeroVector_LeavesMatrixAsIdentity()
        {
            Matrix3D identity = new Matrix3D();
            Vector3D zeroTranslation = new Vector3D(0.0, 0.0, 0.0);
            identity.Translate(zeroTranslation);
            
            // Translating by zero should leave matrix as identity
            await Assert.That(identity.IsIdentity).IsTrue();
        }
    }
}

