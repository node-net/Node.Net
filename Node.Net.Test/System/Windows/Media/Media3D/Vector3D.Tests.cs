using System;
using NUnit.Framework;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class Vector3DTests
    {
        [Test]
        public static void Constructor_WithParameters_SetsProperties()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(vector.X, Is.EqualTo(1.0));
            Assert.That(vector.Y, Is.EqualTo(2.0));
            Assert.That(vector.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Vector3D vector = new Vector3D();
            Assert.That(vector.X, Is.EqualTo(0.0));
            Assert.That(vector.Y, Is.EqualTo(0.0));
            Assert.That(vector.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Vector3D vector = new Vector3D();
            vector.X = 5.0;
            vector.Y = 6.0;
            vector.Z = 7.0;
            Assert.That(vector.X, Is.EqualTo(5.0));
            Assert.That(vector.Y, Is.EqualTo(6.0));
            Assert.That(vector.Z, Is.EqualTo(7.0));
        }

        [Test]
        public static void Length_CalculatesCorrectly()
        {
            Vector3D vector = new Vector3D(3.0, 4.0, 0.0);
            Assert.That(Round(vector.Length, 4), Is.EqualTo(5.0));
        }

        [Test]
        public static void Length_ZeroVector_ReturnsZero()
        {
            Vector3D vector = new Vector3D(0.0, 0.0, 0.0);
            Assert.That(vector.Length, Is.EqualTo(0.0));
        }

        [Test]
        public static void LengthSquared_CalculatesCorrectly()
        {
            Vector3D vector = new Vector3D(3.0, 4.0, 0.0);
            Assert.That(vector.LengthSquared, Is.EqualTo(25.0));
        }

        [Test]
        public static void Add_StaticMethod_ReturnsCorrectResult()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            Vector3D result = Vector3D.Add(v1, v2);
            Assert.That(result.X, Is.EqualTo(5.0));
            Assert.That(result.Y, Is.EqualTo(7.0));
            Assert.That(result.Z, Is.EqualTo(9.0));
        }

        [Test]
        public static void Subtract_StaticMethod_ReturnsCorrectResult()
        {
            Vector3D v1 = new Vector3D(5.0, 7.0, 9.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = Vector3D.Subtract(v1, v2);
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(5.0));
            Assert.That(result.Z, Is.EqualTo(6.0));
        }

        [Test]
        public static void Multiply_VectorByScalar_ReturnsCorrectResult()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = Vector3D.Multiply(vector, 2.0);
            Assert.That(result.X, Is.EqualTo(2.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
            Assert.That(result.Z, Is.EqualTo(6.0));
        }

        [Test]
        public static void Multiply_ScalarByVector_ReturnsCorrectResult()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = Vector3D.Multiply(2.0, vector);
            Assert.That(result.X, Is.EqualTo(2.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
            Assert.That(result.Z, Is.EqualTo(6.0));
        }

        [Test]
        public static void Divide_VectorByScalar_ReturnsCorrectResult()
        {
            Vector3D vector = new Vector3D(2.0, 4.0, 6.0);
            Vector3D result = Vector3D.Divide(vector, 2.0);
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Divide_ByZero_ReturnsInfinityOrNaN()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            // Windows Vector3D doesn't throw on divide by zero, it returns a vector with Infinity/NaN
            Vector3D result = Vector3D.Divide(vector, 0.0);
            Assert.That(double.IsInfinity(result.X) || double.IsNaN(result.X), Is.True);
        }

        [Test]
        public static void DotProduct_CalculatesCorrectly()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            double result = Vector3D.DotProduct(v1, v2);
            Assert.That(result, Is.EqualTo(32.0)); // 1*4 + 2*5 + 3*6 = 4 + 10 + 18 = 32
        }

        [Test]
        public static void DotProduct_PerpendicularVectors_ReturnsZero()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 1.0, 0.0);
            double result = Vector3D.DotProduct(v1, v2);
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public static void CrossProduct_CalculatesCorrectly()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 1.0, 0.0);
            Vector3D result = Vector3D.CrossProduct(v1, v2);
            Assert.That(Round(result.X, 4), Is.EqualTo(0.0));
            Assert.That(Round(result.Y, 4), Is.EqualTo(0.0));
            Assert.That(Round(result.Z, 4), Is.EqualTo(1.0));
        }

        [Test]
        public static void CrossProduct_IsAntiCommutative()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            Vector3D result1 = Vector3D.CrossProduct(v1, v2);
            Vector3D result2 = Vector3D.CrossProduct(v2, v1);
            Assert.That(result1, Is.EqualTo(-result2));
        }

        [Test]
        public static void AngleBetween_ParallelVectors_ReturnsZero()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(2.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            Assert.That(Round(angle, 4), Is.EqualTo(0.0));
        }

        [Test]
        public static void AngleBetween_IdenticalVectors_ReturnsZero()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(1.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            // AngleBetween with identical vectors should return 0.0, not NaN
            Assert.That(Round(angle, 4), Is.EqualTo(0.0));
            Assert.That(double.IsNaN(angle), Is.False);
        }

        [Test]
        public static void AngleBetween_PerpendicularVectors_ReturnsNinety()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 1.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            Assert.That(Round(angle, 4), Is.EqualTo(90.0));
        }

        [Test]
        public static void AngleBetween_OppositeVectors_ReturnsOneEighty()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(-1.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            Assert.That(Round(angle, 4), Is.EqualTo(180.0));
        }

        [Test]
        public static void AngleBetween_ZeroVector_ReturnsNaN()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            // Windows Vector3D returns NaN when one vector is zero
            Assert.That(double.IsNaN(angle), Is.True);
        }

        [Test]
        public static void Normalize_ModifiesVectorToUnitLength()
        {
            Vector3D vector = new Vector3D(3.0, 4.0, 0.0);
            vector.Normalize();
            Assert.That(Round(vector.Length, 4), Is.EqualTo(1.0));
        }

        [Test]
        public static void Normalize_ZeroVector_SetsToNaN()
        {
            Vector3D vector = new Vector3D(0.0, 0.0, 0.0);
            // Windows Vector3D doesn't throw on normalize of zero vector, it sets components to NaN
            vector.Normalize();
            Assert.That(double.IsNaN(vector.X) || double.IsNaN(vector.Y) || double.IsNaN(vector.Z), Is.True);
        }

        [Test]
        public static void Negate_ReversesAllComponents()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            vector.Negate();
            Assert.That(vector.X, Is.EqualTo(-1.0));
            Assert.That(vector.Y, Is.EqualTo(-2.0));
            Assert.That(vector.Z, Is.EqualTo(-3.0));
        }

        [Test]
        public static void Operator_Addition_WorksCorrectly()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            Vector3D result = v1 + v2;
            Assert.That(result.X, Is.EqualTo(5.0));
            Assert.That(result.Y, Is.EqualTo(7.0));
            Assert.That(result.Z, Is.EqualTo(9.0));
        }

        [Test]
        public static void Operator_Subtraction_WorksCorrectly()
        {
            Vector3D v1 = new Vector3D(5.0, 7.0, 9.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = v1 - v2;
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(5.0));
            Assert.That(result.Z, Is.EqualTo(6.0));
        }

        [Test]
        public static void Operator_MultiplyVectorByScalar_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = vector * 2.0;
            Assert.That(result.X, Is.EqualTo(2.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
            Assert.That(result.Z, Is.EqualTo(6.0));
        }

        [Test]
        public static void Operator_MultiplyScalarByVector_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = 2.0 * vector;
            Assert.That(result.X, Is.EqualTo(2.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
            Assert.That(result.Z, Is.EqualTo(6.0));
        }

        [Test]
        public static void Operator_Division_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(2.0, 4.0, 6.0);
            Vector3D result = vector / 2.0;
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Operator_UnaryNegation_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = -vector;
            Assert.That(result.X, Is.EqualTo(-1.0));
            Assert.That(result.Y, Is.EqualTo(-2.0));
            Assert.That(result.Z, Is.EqualTo(-3.0));
        }

        [Test]
        public static void Operator_Equality_ReturnsTrueForEqualVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(v1 == v2, Is.True);
        }

        [Test]
        public static void Operator_Equality_ReturnsFalseForDifferentVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 4.0);
            Assert.That(v1 == v2, Is.False);
        }

        [Test]
        public static void Operator_Inequality_ReturnsTrueForDifferentVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 4.0);
            Assert.That(v1 != v2, Is.True);
        }

        [Test]
        public static void Operator_Inequality_ReturnsFalseForEqualVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(v1 != v2, Is.False);
        }

        [Test]
        public static void Equals_WithEqualVector_ReturnsTrue()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(v1.Equals(v2), Is.True);
        }

        [Test]
        public static void Equals_WithDifferentVector_ReturnsFalse()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 4.0);
            Assert.That(v1.Equals(v2), Is.False);
        }

        [Test]
        public static void Equals_WithNull_ReturnsFalse()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(v1.Equals(null), Is.False);
        }

        [Test]
        public static void Equals_WithNonVector_ReturnsFalse()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(v1.Equals("not a vector"), Is.False);
        }

        [Test]
        public static void GetHashCode_EqualVectors_ReturnSameHashCode()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Assert.That(v1.GetHashCode(), Is.EqualTo(v2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Vector3D vector = new Vector3D(1.5, 2.5, 3.5);
            // Windows Vector3D only supports ToString() without parameters
            string result = vector.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
            Assert.That(result, Does.Contain("3.5"));
        }

        [Test]
        public static void Parse_WithoutParentheses_ParsesCorrectly()
        {
            // Arrange - Format without parentheses (supported format)
            string value = "1.0000002742460514,4.982079117255012E-06,0";

            // Act
            Vector3D result = Vector3D.Parse(value);

            // Assert
            Assert.That(result.X, Is.EqualTo(1.0000002742460514).Within(0.000000000000001));
            Assert.That(result.Y, Is.EqualTo(4.982079117255012E-06).Within(0.000000000000001));
            Assert.That(result.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Parse_WithParentheses_ThrowsFormatException()
        {
            // Arrange - Format with parentheses (not supported by system Vector3D.Parse on Windows)
            string value = "(1.0000002742460514,4.982079117255012E-06,0)";

            // Act & Assert
#if IS_WINDOWS
            // On Windows, the system Vector3D.Parse does not support parentheses
            // It expects the format "1.0,0.0,0.0" (without parentheses)
            Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#else
            // On non-Windows, the custom Vector3D.Parse supports parentheses by stripping them
            Vector3D result = Vector3D.Parse(value);
            Assert.That(result.X, Is.EqualTo(1.0000002742460514).Within(0.000000000000001));
            Assert.That(result.Y, Is.EqualTo(4.982079117255012E-06).Within(0.000000000000001));
            Assert.That(result.Z, Is.EqualTo(0.0));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_ExactRochambeauFormat_ThrowsFormatException()
        {
            // Arrange - Exact format from rochambeau file conversion error
            string value = "(1.0000002742460514,4.982079117255012E-06,0)";

            // Act & Assert
#if IS_WINDOWS
            // This is the exact format that causes Matrix3DFactory.CreateFromIDictionary to fail
            // On Windows, Vector3D.Parse does not support parentheses format
            // Matrix3DFactory strips parentheses before calling Parse, so it works there
            Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#else
            // On non-Windows, the custom Vector3D.Parse supports parentheses by stripping them
            Vector3D result = Vector3D.Parse(value);
            Assert.That(result.X, Is.EqualTo(1.0000002742460514).Within(0.000000000000001));
            Assert.That(result.Y, Is.EqualTo(4.982079117255012E-06).Within(0.000000000000001));
            Assert.That(result.Z, Is.EqualTo(0.0));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_OnlyOpeningParenthesis_Fails()
        {
            // Arrange - Only opening parenthesis (invalid format)
            string value = "(1.0,2.0,3.0";

            // Act & Assert
            FormatException ex = Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#if IS_WINDOWS
            // On Windows, system Vector3D.Parse throws with different message
            Assert.That(ex.Message, Does.Contain("not in a correct format").Or.Contains("Invalid Vector3D format"));
#else
            // On non-Windows, custom Vector3D.Parse throws with specific message
            Assert.That(ex.Message, Does.Contain("Invalid Vector3D format"));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_OnlyClosingParenthesis_Fails()
        {
            // Arrange - Only closing parenthesis (invalid format)
            string value = "1.0,2.0,3.0)";

            // Act & Assert
            FormatException ex = Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#if IS_WINDOWS
            // On Windows, system Vector3D.Parse may parse successfully (no opening paren to cause error)
            // or throw with different message
            Assert.That(ex.Message, Does.Contain("not in a correct format").Or.Contains("Invalid Vector3D format"));
#else
            // On non-Windows, custom Vector3D.Parse throws with specific message
            Assert.That(ex.Message, Does.Contain("Invalid Vector3D format"));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_MismatchedParentheses_Fails()
        {
            // Arrange - Mismatched parentheses (invalid format)
            string value = "(1.0,2.0,3.0))";

            // Act & Assert
            FormatException ex = Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#if IS_WINDOWS
            // On Windows, system Vector3D.Parse throws with different message
            Assert.That(ex.Message, Does.Contain("not in a correct format").Or.Contains("Invalid Vector3D format"));
#else
            // On non-Windows, custom Vector3D.Parse throws with specific message
            Assert.That(ex.Message, Does.Contain("Invalid Vector3D format"));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_ReversedParentheses_Fails()
        {
            // Arrange - Reversed parentheses (invalid format)
            string value = ")1.0,2.0,3.0(";

            // Act & Assert
            FormatException ex = Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#if IS_WINDOWS
            // On Windows, system Vector3D.Parse throws with different message
            Assert.That(ex.Message, Does.Contain("not in a correct format").Or.Contains("Invalid Vector3D format"));
#else
            // On non-Windows, custom Vector3D.Parse throws with specific message
            Assert.That(ex.Message, Does.Contain("Invalid Vector3D format"));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_ExtraContentAfterParentheses_Fails()
        {
            // Arrange - Extra content after closing parenthesis (invalid format)
            string value = "(1.0,2.0,3.0)extra";

            // Act & Assert
            FormatException ex = Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#if IS_WINDOWS
            // On Windows, system Vector3D.Parse throws with different message
            Assert.That(ex.Message, Does.Contain("not in a correct format").Or.Contains("Invalid Vector3D format"));
#else
            // On non-Windows, custom Vector3D.Parse throws with specific message
            Assert.That(ex.Message, Does.Contain("Invalid Vector3D format"));
#endif
        }

        [Test]
        public static void Parse_WithParentheses_ExtraContentBeforeParentheses_Fails()
        {
            // Arrange - Extra content before opening parenthesis (invalid format)
            string value = "extra(1.0,2.0,3.0)";

            // Act & Assert
            FormatException ex = Assert.Throws<FormatException>(() => Vector3D.Parse(value));
#if IS_WINDOWS
            // On Windows, system Vector3D.Parse throws with different message
            Assert.That(ex.Message, Does.Contain("not in a correct format").Or.Contains("Invalid Vector3D format"));
#else
            // On non-Windows, custom Vector3D.Parse throws with specific message
            Assert.That(ex.Message, Does.Contain("Invalid Vector3D format"));
#endif
        }
    }
}

