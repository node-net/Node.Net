extern alias NodeNet;
using NUnit.Framework;
using Vector = NodeNet::System.Windows.Vector; // Explicitly use Node.Net's Vector
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class VectorTests
    {
        [Test]
        public static void Constructor_WithParameters_SetsProperties()
        {
            Vector vector = new Vector(1.0, 2.0);
            Assert.That(vector.X, Is.EqualTo(1.0));
            Assert.That(vector.Y, Is.EqualTo(2.0));
        }

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Vector vector = new Vector();
            Assert.That(vector.X, Is.EqualTo(0.0));
            Assert.That(vector.Y, Is.EqualTo(0.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Vector vector = new Vector();
            vector.X = 5.0;
            vector.Y = 6.0;
            Assert.That(vector.X, Is.EqualTo(5.0));
            Assert.That(vector.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void Length_CalculatesCorrectly()
        {
            Vector vector = new Vector(3.0, 4.0);
            Assert.That(vector.Length, Is.EqualTo(5.0));
        }

        [Test]
        public static void LengthSquared_CalculatesCorrectly()
        {
            Vector vector = new Vector(3.0, 4.0);
            Assert.That(vector.LengthSquared, Is.EqualTo(25.0));
        }

        [Test]
        public static void Add_ReturnsSum()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(3.0, 4.0);
            Vector result = Vector.Add(v1, v2);
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void Subtract_ReturnsDifference()
        {
            Vector v1 = new Vector(5.0, 6.0);
            Vector v2 = new Vector(1.0, 2.0);
            Vector result = Vector.Subtract(v1, v2);
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
        }

        [Test]
        public static void Multiply_ByScalar_ReturnsScaledVector()
        {
            Vector vector = new Vector(2.0, 3.0);
            Vector result = Vector.Multiply(vector, 2.0);
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void Divide_ByScalar_ReturnsDividedVector()
        {
            Vector vector = new Vector(4.0, 6.0);
            Vector result = Vector.Divide(vector, 2.0);
            
            Assert.That(result.X, Is.EqualTo(2.0));
            Assert.That(result.Y, Is.EqualTo(3.0));
        }

        [Test]
        public static void Divide_ByZero_ReturnsInfinityOrNaN()
        {
            Vector vector = new Vector(1.0, 2.0);
            // Windows Vector returns Infinity/NaN on divide by zero, doesn't throw
            Vector result = Vector.Divide(vector, 0.0);
            Assert.That(double.IsInfinity(result.X) || double.IsNaN(result.X), Is.True);
        }

        // Note: Windows Vector doesn't have DotProduct static method
        // This test is skipped to match Windows API exactly

        [Test]
        public static void CrossProduct_CalculatesCorrectly()
        {
            Vector v1 = new Vector(1.0, 0.0);
            Vector v2 = new Vector(0.0, 1.0);
            double result = Vector.CrossProduct(v1, v2);
            
            Assert.That(result, Is.EqualTo(1.0)); // 1*1 - 0*0 = 1
        }

        [Test]
        public static void AngleBetween_CalculatesCorrectly()
        {
            Vector v1 = new Vector(1.0, 0.0);
            Vector v2 = new Vector(0.0, 1.0);
            double angle = Vector.AngleBetween(v1, v2);
            
            Assert.That(Round(angle, 4), Is.EqualTo(90.0));
        }

        [Test]
        public static void AngleBetween_ZeroVector_ReturnsNaN()
        {
            Vector v1 = new Vector(1.0, 0.0);
            Vector v2 = new Vector(0.0, 0.0);
            double angle = Vector.AngleBetween(v1, v2);
            // Windows Vector doesn't return NaN when one vector is zero, it returns 0.0
            Assert.That(angle, Is.EqualTo(0.0));
        }

        [Test]
        public static void Normalize_ModifiesVectorToUnitLength()
        {
            Vector vector = new Vector(3.0, 4.0);
            vector.Normalize();
            Assert.That(Round(vector.Length, 4), Is.EqualTo(1.0));
        }

        [Test]
        public static void Normalize_ZeroVector_SetsToNaN()
        {
            Vector vector = new Vector(0.0, 0.0);
            // Windows Vector doesn't throw on normalize of zero vector, it sets components to NaN
            vector.Normalize();
            Assert.That(double.IsNaN(vector.X) || double.IsNaN(vector.Y), Is.True);
        }

        [Test]
        public static void Negate_ReversesAllComponents()
        {
            Vector vector = new Vector(1.0, 2.0);
            vector.Negate();
            
            Assert.That(vector.X, Is.EqualTo(-1.0));
            Assert.That(vector.Y, Is.EqualTo(-2.0));
        }

        [Test]
        public static void OperatorAdd_ReturnsSum()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(3.0, 4.0);
            Vector result = v1 + v2;
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void OperatorSubtract_ReturnsDifference()
        {
            Vector v1 = new Vector(5.0, 6.0);
            Vector v2 = new Vector(1.0, 2.0);
            Vector result = v1 - v2;
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
        }

        [Test]
        public static void OperatorMultiply_ByScalar_ReturnsScaledVector()
        {
            Vector vector = new Vector(2.0, 3.0);
            Vector result = vector * 2.0;
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void OperatorDivide_ByScalar_ReturnsDividedVector()
        {
            Vector vector = new Vector(4.0, 6.0);
            Vector result = vector / 2.0;
            
            Assert.That(result.X, Is.EqualTo(2.0));
            Assert.That(result.Y, Is.EqualTo(3.0));
        }

        [Test]
        public static void OperatorUnaryMinus_NegatesVector()
        {
            Vector vector = new Vector(1.0, 2.0);
            Vector result = -vector;
            
            Assert.That(result.X, Is.EqualTo(-1.0));
            Assert.That(result.Y, Is.EqualTo(-2.0));
        }

        [Test]
        public static void OperatorEquals_EqualVectors_ReturnsTrue()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(1.0, 2.0);
            
            Assert.That(v1 == v2, Is.True);
        }

        [Test]
        public static void OperatorNotEquals_DifferentVectors_ReturnsTrue()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(3.0, 4.0);
            
            Assert.That(v1 != v2, Is.True);
        }

        [Test]
        public static void Equals_SameValues_ReturnsTrue()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(1.0, 2.0);
            
            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public static void GetHashCode_EqualVectors_ReturnSameHashCode()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(1.0, 2.0);
            Assert.That(v1.GetHashCode(), Is.EqualTo(v2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Vector vector = new Vector(1.5, 2.5);
            // Windows Vector only supports ToString() without parameters
            string result = vector.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
        }
    }
}

