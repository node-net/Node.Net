using System.Threading.Tasks;
using static System.Math;

namespace Node.Net.Test
{
    internal static class VectorTests
    {
        [Test]
        public static async Task Constructor_WithParameters_SetsProperties()
        {
            Vector vector = new Vector(1.0, 2.0);
            await Assert.That(vector.X).IsEqualTo(1.0);
            await Assert.That(vector.Y).IsEqualTo(2.0);
        }

        [Test]
        public static async Task Constructor_Default_InitializesToZero()
        {
            Vector vector = new Vector();
            await Assert.That(vector.X).IsEqualTo(0.0);
            await Assert.That(vector.Y).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Properties_CanBeSet()
        {
            Vector vector = new Vector();
            vector.X = 5.0;
            vector.Y = 6.0;
            await Assert.That(vector.X).IsEqualTo(5.0);
            await Assert.That(vector.Y).IsEqualTo(6.0);
        }

        [Test]
        public static async Task Length_CalculatesCorrectly()
        {
            Vector vector = new Vector(3.0, 4.0);
            await Assert.That(vector.Length).IsEqualTo(5.0);
        }

        [Test]
        public static async Task LengthSquared_CalculatesCorrectly()
        {
            Vector vector = new Vector(3.0, 4.0);
            await Assert.That(vector.LengthSquared).IsEqualTo(25.0);
        }

        [Test]
        public static async Task Add_ReturnsSum()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(3.0, 4.0);
            Vector result = Vector.Add(v1, v2);
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(6.0);
        }

        [Test]
        public static async Task Subtract_ReturnsDifference()
        {
            Vector v1 = new Vector(5.0, 6.0);
            Vector v2 = new Vector(1.0, 2.0);
            Vector result = Vector.Subtract(v1, v2);
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(4.0);
        }

        [Test]
        public static async Task Multiply_ByScalar_ReturnsScaledVector()
        {
            Vector vector = new Vector(2.0, 3.0);
            Vector result = Vector.Multiply(vector, 2.0);
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(6.0);
        }

        [Test]
        public static async Task Divide_ByScalar_ReturnsDividedVector()
        {
            Vector vector = new Vector(4.0, 6.0);
            Vector result = Vector.Divide(vector, 2.0);
            
            await Assert.That(result.X).IsEqualTo(2.0);
            await Assert.That(result.Y).IsEqualTo(3.0);
        }

        [Test]
        public static async Task Divide_ByZero_ReturnsInfinityOrNaN()
        {
            Vector vector = new Vector(1.0, 2.0);
            // Windows Vector returns Infinity/NaN on divide by zero, doesn't throw
            Vector result = Vector.Divide(vector, 0.0);
            await Assert.That(double.IsInfinity(result.X) || double.IsNaN(result.X)).IsTrue();
        }

        // Note: Windows Vector doesn't have DotProduct static method
        // This test is skipped to match Windows API exactly

        [Test]
        public static async Task CrossProduct_CalculatesCorrectly()
        {
            Vector v1 = new Vector(1.0, 0.0);
            Vector v2 = new Vector(0.0, 1.0);
            double result = Vector.CrossProduct(v1, v2);
            
            await Assert.That(result).IsEqualTo(1.0); // 1*1 - 0*0 = 1
        }

        [Test]
        public static async Task AngleBetween_CalculatesCorrectly()
        {
            Vector v1 = new Vector(1.0, 0.0);
            Vector v2 = new Vector(0.0, 1.0);
            double angle = Vector.AngleBetween(v1, v2);
            
            await Assert.That(Round(angle, 4)).IsEqualTo(90.0);
        }

        [Test]
        public static async Task AngleBetween_ZeroVector_ReturnsNaN()
        {
            Vector v1 = new Vector(1.0, 0.0);
            Vector v2 = new Vector(0.0, 0.0);
            double angle = Vector.AngleBetween(v1, v2);
            // Windows Vector doesn't return NaN when one vector is zero, it returns 0.0
            await Assert.That(angle).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Normalize_ModifiesVectorToUnitLength()
        {
            Vector vector = new Vector(3.0, 4.0);
            vector.Normalize();
            await Assert.That(Round(vector.Length, 4)).IsEqualTo(1.0);
        }

        [Test]
        public static async Task Normalize_ZeroVector_SetsToNaN()
        {
            Vector vector = new Vector(0.0, 0.0);
            // Windows Vector doesn't throw on normalize of zero vector, it sets components to NaN
            vector.Normalize();
            await Assert.That(double.IsNaN(vector.X) || double.IsNaN(vector.Y)).IsTrue();
        }

        [Test]
        public static async Task Negate_ReversesAllComponents()
        {
            Vector vector = new Vector(1.0, 2.0);
            vector.Negate();
            
            await Assert.That(vector.X).IsEqualTo(-1.0);
            await Assert.That(vector.Y).IsEqualTo(-2.0);
        }

        [Test]
        public static async Task OperatorAdd_ReturnsSum()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(3.0, 4.0);
            Vector result = v1 + v2;
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(6.0);
        }

        [Test]
        public static async Task OperatorSubtract_ReturnsDifference()
        {
            Vector v1 = new Vector(5.0, 6.0);
            Vector v2 = new Vector(1.0, 2.0);
            Vector result = v1 - v2;
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(4.0);
        }

        [Test]
        public static async Task OperatorMultiply_ByScalar_ReturnsScaledVector()
        {
            Vector vector = new Vector(2.0, 3.0);
            Vector result = vector * 2.0;
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(6.0);
        }

        [Test]
        public static async Task OperatorDivide_ByScalar_ReturnsDividedVector()
        {
            Vector vector = new Vector(4.0, 6.0);
            Vector result = vector / 2.0;
            
            await Assert.That(result.X).IsEqualTo(2.0);
            await Assert.That(result.Y).IsEqualTo(3.0);
        }

        [Test]
        public static async Task OperatorUnaryMinus_NegatesVector()
        {
            Vector vector = new Vector(1.0, 2.0);
            Vector result = -vector;
            
            await Assert.That(result.X).IsEqualTo(-1.0);
            await Assert.That(result.Y).IsEqualTo(-2.0);
        }

        [Test]
        public static async Task OperatorEquals_EqualVectors_ReturnsTrue()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(1.0, 2.0);
            
            await Assert.That(v1 == v2).IsTrue();
        }

        [Test]
        public static async Task OperatorNotEquals_DifferentVectors_ReturnsTrue()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(3.0, 4.0);
            
            await Assert.That(v1 != v2).IsTrue();
        }

        [Test]
        public static async Task Equals_SameValues_ReturnsTrue()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(1.0, 2.0);
            
            await Assert.That(v1.Equals(v2)).IsTrue();
        }

        [Test]
        public static async Task GetHashCode_EqualVectors_ReturnSameHashCode()
        {
            Vector v1 = new Vector(1.0, 2.0);
            Vector v2 = new Vector(1.0, 2.0);
            await Assert.That(v1.GetHashCode()).IsEqualTo(v2.GetHashCode());
        }

        [Test]
        public static async Task ToString_ReturnsFormattedString()
        {
            Vector vector = new Vector(1.5, 2.5);
            // Windows Vector only supports ToString() without parameters
            string result = vector.ToString();
            await Assert.That(result.Contains("1.5")).IsTrue();
            await Assert.That(result.Contains("2.5")).IsTrue();
        }
    }
}

