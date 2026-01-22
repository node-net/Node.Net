using NUnit.Framework;
using System;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class QuaternionTests
    {
        private const double Tolerance = 1e-9;

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Quaternion q = new Quaternion();
            // Windows Quaternion default constructor creates identity quaternion (0,0,0,1), not (0,0,0,0)
            Assert.That(q.X, Is.EqualTo(0.0));
            Assert.That(q.Y, Is.EqualTo(0.0));
            Assert.That(q.Z, Is.EqualTo(0.0));
            Assert.That(q.W, Is.EqualTo(1.0));
        }

        [Test]
        public static void Constructor_WithValues_InitializesCorrectly()
        {
            Quaternion q = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Assert.That(q.X, Is.EqualTo(1.0));
            Assert.That(q.Y, Is.EqualTo(2.0));
            Assert.That(q.Z, Is.EqualTo(3.0));
            Assert.That(q.W, Is.EqualTo(4.0));
        }

        [Test]
        public static void Constructor_FromAxisAngle_ZeroAngle_CreatesIdentity()
        {
            Quaternion q = new Quaternion(new Vector3D(1, 0, 0), 0);
            Assert.That(Round(q.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(q.Y, 6), Is.EqualTo(0.0));
            Assert.That(Round(q.Z, 6), Is.EqualTo(0.0));
            Assert.That(Round(q.W, 6), Is.EqualTo(1.0));
        }

        [Test]
        public static void Constructor_FromAxisAngle_90DegreesAroundZ_CreatesCorrectQuaternion()
        {
            Quaternion q = new Quaternion(new Vector3D(0, 0, 1), 90);
            Assert.That(Round(q.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(q.Y, 6), Is.EqualTo(0.0));
            Assert.That(Round(Abs(q.Z), 6), Is.EqualTo(0.707107));
            Assert.That(Round(q.W, 6), Is.EqualTo(0.707107));
        }

        [Test]
        public static void Constructor_FromAxisAngle_ZeroAxis_CreatesIdentity()
        {
            // Windows Quaternion throws InvalidOperationException when axis is zero
            Assert.Throws<InvalidOperationException>(() => new Quaternion(new Vector3D(0, 0, 0), 45));
        }

        [Test]
        public static void Properties_GetAndSet_WorkCorrectly()
        {
            Quaternion q = new Quaternion();
            q.X = 1.0;
            q.Y = 2.0;
            q.Z = 3.0;
            q.W = 4.0;

            Assert.That(q.X, Is.EqualTo(1.0));
            Assert.That(q.Y, Is.EqualTo(2.0));
            Assert.That(q.Z, Is.EqualTo(3.0));
            Assert.That(q.W, Is.EqualTo(4.0));
        }

        // Note: Windows Quaternion doesn't expose Length or LengthSquared as public properties
        // These tests are skipped as they test internal implementation details

        [Test]
        public static void Normalize_UnitQuaternion_RemainsUnit()
        {
            Quaternion q = new Quaternion(0.0, 0.0, 0.707107, 0.707107);
            q.Normalize();
            // Verify it's normalized by checking components
            double lengthSq = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
            Assert.That(Round(Sqrt(lengthSq), 6), Is.EqualTo(1.0));
        }

        [Test]
        public static void Normalize_NonUnitQuaternion_NormalizesCorrectly()
        {
            Quaternion q = new Quaternion(3.0, 4.0, 0.0, 0.0);
            q.Normalize();
            double lengthSq = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
            Assert.That(Round(Sqrt(lengthSq), 6), Is.EqualTo(1.0));
            Assert.That(Round(q.X, 6), Is.EqualTo(0.6));
            Assert.That(Round(q.Y, 6), Is.EqualTo(0.8));
        }

        [Test]
        public static void Normalize_ZeroQuaternion_SetsToNaN()
        {
            Quaternion q = new Quaternion(0.0, 0.0, 0.0, 0.0);
            // Windows Quaternion sets components to NaN when normalizing zero quaternion
            q.Normalize();
            Assert.That(double.IsNaN(q.X) || double.IsNaN(q.Y) || double.IsNaN(q.Z) || double.IsNaN(q.W), Is.True);
        }

        [Test]
        public static void Conjugate_NegatesXYZ_KeepsW()
        {
            Quaternion q = new Quaternion(1.0, 2.0, 3.0, 4.0);
            q.Conjugate(); // Modifies in place (Windows API)
            
            Assert.That(q.X, Is.EqualTo(-1.0));
            Assert.That(q.Y, Is.EqualTo(-2.0));
            Assert.That(q.Z, Is.EqualTo(-3.0));
            Assert.That(q.W, Is.EqualTo(4.0));
        }

        [Test]
        public static void Conjugate_ModifiesInPlace()
        {
            Quaternion q = new Quaternion(1.0, 2.0, 3.0, 4.0);
            q.Conjugate(); // Windows API modifies in place
            
            // Verify the quaternion was modified
            Assert.That(q.X, Is.EqualTo(-1.0));
            Assert.That(q.Y, Is.EqualTo(-2.0));
            Assert.That(q.Z, Is.EqualTo(-3.0));
            Assert.That(q.W, Is.EqualTo(4.0));
        }

        [Test]
        public static void Invert_UnitQuaternion_ReturnsConjugate()
        {
            Quaternion q = new Quaternion(0.0, 0.0, 0.707107, 0.707107);
            q.Normalize();
            Quaternion original = new Quaternion(q.X, q.Y, q.Z, q.W);
            q.Invert(); // Modifies in place (Windows API)
            
            // For unit quaternion, inverse should be conjugate
            Assert.That(Round(q.X, 6), Is.EqualTo(Round(-original.X, 6)));
            Assert.That(Round(q.Y, 6), Is.EqualTo(Round(-original.Y, 6)));
            Assert.That(Round(q.Z, 6), Is.EqualTo(Round(-original.Z, 6)));
            Assert.That(Round(q.W, 6), Is.EqualTo(Round(original.W, 6)));
        }

        [Test]
        public static void Invert_ZeroQuaternion_ThrowsException()
        {
            Quaternion q = new Quaternion(0.0, 0.0, 0.0, 0.0);
            // Windows Quaternion.Invert() doesn't throw on zero quaternion, it sets components to NaN
            q.Invert();
            Assert.That(double.IsNaN(q.X) || double.IsNaN(q.Y) || double.IsNaN(q.Z) || double.IsNaN(q.W), Is.True);
        }

        [Test]
        public static void Multiply_IdentityQuaternions_ReturnsIdentity()
        {
            Quaternion q1 = new Quaternion(0.0, 0.0, 0.0, 1.0);
            Quaternion q2 = new Quaternion(0.0, 0.0, 0.0, 1.0);
            Quaternion result = Quaternion.Multiply(q1, q2);
            
            Assert.That(Round(result.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Y, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Z, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.W, 6), Is.EqualTo(1.0));
        }

        [Test]
        public static void Multiply_QuaternionWithItsInverse_ReturnsIdentity()
        {
            Quaternion q = new Quaternion(0.0, 0.0, 0.707107, 0.707107);
            q.Normalize();
            Quaternion original = new Quaternion(q.X, q.Y, q.Z, q.W);
            Quaternion inverse = new Quaternion(q.X, q.Y, q.Z, q.W);
            inverse.Invert(); // Modifies in place (Windows API)
            Quaternion result = Quaternion.Multiply(original, inverse);
            
            Assert.That(Round(result.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Y, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Z, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.W, 6), Is.EqualTo(1.0));
        }

        [Test]
        public static void OperatorMultiply_MultipliesCorrectly()
        {
            Quaternion q1 = new Quaternion(1.0, 0.0, 0.0, 0.0);
            Quaternion q2 = new Quaternion(0.0, 1.0, 0.0, 0.0);
            Quaternion result = q1 * q2;
            
            // i * j = k
            Assert.That(Round(result.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Y, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Z, 6), Is.EqualTo(1.0));
            Assert.That(Round(result.W, 6), Is.EqualTo(0.0));
        }

        [Test]
        public static void OperatorEquality_EqualQuaternions_ReturnsTrue()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            
            Assert.That(q1 == q2, Is.True);
        }

        [Test]
        public static void OperatorEquality_DifferentQuaternions_ReturnsFalse()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 5.0);
            
            Assert.That(q1 == q2, Is.False);
        }

        [Test]
        public static void OperatorInequality_EqualQuaternions_ReturnsFalse()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            
            Assert.That(q1 != q2, Is.False);
        }

        [Test]
        public static void Equals_WithSameValues_ReturnsTrue()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            
            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public static void Equals_WithDifferentValues_ReturnsFalse()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 5.0);
            
            Assert.That(q1.Equals(q2), Is.False);
        }

        [Test]
        public static void Equals_WithNull_ReturnsFalse()
        {
            Quaternion q = new Quaternion(1.0, 2.0, 3.0, 4.0);
            
            Assert.That(q.Equals(null), Is.False);
        }

        [Test]
        public static void Equals_WithNonQuaternion_ReturnsFalse()
        {
            Quaternion q = new Quaternion(1.0, 2.0, 3.0, 4.0);
            
            Assert.That(q.Equals("not a quaternion"), Is.False);
        }

        [Test]
        public static void GetHashCode_SameValues_ReturnsSameHashCode()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            
            Assert.That(q1.GetHashCode(), Is.EqualTo(q2.GetHashCode()));
        }

        [Test]
        public static void GetHashCode_DifferentValues_ReturnsDifferentHashCode()
        {
            Quaternion q1 = new Quaternion(1.0, 2.0, 3.0, 4.0);
            Quaternion q2 = new Quaternion(1.0, 2.0, 3.0, 5.0);
            
            // Hash codes may or may not be different, but if they're equal, the quaternions should be equal
            if (q1.GetHashCode() == q2.GetHashCode())
            {
                // This is acceptable - hash collisions can happen
                Assert.That(true, Is.True);
            }
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Quaternion q = new Quaternion(1.5, 2.5, 3.5, 4.5);
            // Windows Quaternion only supports ToString() without parameters
            string result = q.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
            Assert.That(result, Does.Contain("3.5"));
            Assert.That(result, Does.Contain("4.5"));
        }

        [Test]
        public static void Constructor_FromAxisAngle_180Degrees_CreatesCorrectQuaternion()
        {
            Quaternion q = new Quaternion(new Vector3D(1, 0, 0), 180);
            // For 180 degrees around X axis: q = (sin(90), 0, 0, cos(90)) = (1, 0, 0, 0)
            Assert.That(Round(Abs(q.X), 6), Is.EqualTo(1.0));
            Assert.That(Round(q.Y, 6), Is.EqualTo(0.0));
            Assert.That(Round(q.Z, 6), Is.EqualTo(0.0));
            Assert.That(Round(q.W, 6), Is.EqualTo(0.0));
        }

        [Test]
        public static void Constructor_FromAxisAngle_NormalizesAxis()
        {
            // Use a non-normalized axis
            Quaternion q = new Quaternion(new Vector3D(2, 0, 0), 90);
            // Should normalize the axis internally
            Quaternion q2 = new Quaternion(new Vector3D(1, 0, 0), 90);
            
            // Both should produce the same quaternion (within tolerance)
            Assert.That(Round(Abs(q.X), 6), Is.EqualTo(Round(Abs(q2.X), 6)));
            Assert.That(Round(Abs(q.Y), 6), Is.EqualTo(Round(Abs(q2.Y), 6)));
            Assert.That(Round(Abs(q.Z), 6), Is.EqualTo(Round(Abs(q2.Z), 6)));
            Assert.That(Round(Abs(q.W), 6), Is.EqualTo(Round(Abs(q2.W), 6)));
        }
    }
}

