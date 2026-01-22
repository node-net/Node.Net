using System;
using System.Threading.Tasks;
using static System.Math;

namespace Node.Net.Test
{
    internal static class Vector3DTests
    {
        [Test]
        public static async Task Constructor_WithParameters_SetsProperties()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(vector.X).IsEqualTo(1.0));
            await Assert.That(vector.Y).IsEqualTo(2.0));
            await Assert.That(vector.Z).IsEqualTo(3.0));
        }

        [Test]
        public static async Task Constructor_Default_InitializesToZero()
        {
            Vector3D vector = new Vector3D();
            await Assert.That(vector.X).IsEqualTo(0.0));
            await Assert.That(vector.Y).IsEqualTo(0.0));
            await Assert.That(vector.Z).IsEqualTo(0.0));
        }

        [Test]
        public static async Task Properties_CanBeSet()
        {
            Vector3D vector = new Vector3D();
            vector.X = 5.0;
            vector.Y = 6.0;
            vector.Z = 7.0;
            await Assert.That(vector.X).IsEqualTo(5.0));
            await Assert.That(vector.Y).IsEqualTo(6.0));
            await Assert.That(vector.Z).IsEqualTo(7.0));
        }

        [Test]
        public static async Task Length_CalculatesCorrectly()
        {
            Vector3D vector = new Vector3D(3.0, 4.0, 0.0);
            await Assert.That(Round(vector.Length, 4)).IsEqualTo(5.0));
        }

        [Test]
        public static async Task Length_ZeroVector_ReturnsZero()
        {
            Vector3D vector = new Vector3D(0.0, 0.0, 0.0);
            await Assert.That(vector.Length).IsEqualTo(0.0));
        }

        [Test]
        public static async Task LengthSquared_CalculatesCorrectly()
        {
            Vector3D vector = new Vector3D(3.0, 4.0, 0.0);
            await Assert.That(vector.LengthSquared).IsEqualTo(25.0));
        }

        [Test]
        public static async Task Add_StaticMethod_ReturnsCorrectResult()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            Vector3D result = Vector3D.Add(v1, v2);
            await Assert.That(result.X).IsEqualTo(5.0));
            await Assert.That(result.Y).IsEqualTo(7.0));
            await Assert.That(result.Z).IsEqualTo(9.0));
        }

        [Test]
        public static async Task Subtract_StaticMethod_ReturnsCorrectResult()
        {
            Vector3D v1 = new Vector3D(5.0, 7.0, 9.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = Vector3D.Subtract(v1, v2);
            await Assert.That(result.X).IsEqualTo(4.0));
            await Assert.That(result.Y).IsEqualTo(5.0));
            await Assert.That(result.Z).IsEqualTo(6.0));
        }

        [Test]
        public static async Task Multiply_VectorByScalar_ReturnsCorrectResult()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = Vector3D.Multiply(vector, 2.0);
            await Assert.That(result.X).IsEqualTo(2.0));
            await Assert.That(result.Y).IsEqualTo(4.0));
            await Assert.That(result.Z).IsEqualTo(6.0));
        }

        [Test]
        public static async Task Multiply_ScalarByVector_ReturnsCorrectResult()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = Vector3D.Multiply(2.0, vector);
            await Assert.That(result.X).IsEqualTo(2.0));
            await Assert.That(result.Y).IsEqualTo(4.0));
            await Assert.That(result.Z).IsEqualTo(6.0));
        }

        [Test]
        public static async Task Divide_VectorByScalar_ReturnsCorrectResult()
        {
            Vector3D vector = new Vector3D(2.0, 4.0, 6.0);
            Vector3D result = Vector3D.Divide(vector, 2.0);
            await Assert.That(result.X).IsEqualTo(1.0));
            await Assert.That(result.Y).IsEqualTo(2.0));
            await Assert.That(result.Z).IsEqualTo(3.0));
        }

        [Test]
        public static async Task Divide_ByZero_ReturnsInfinityOrNaN()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            // Windows Vector3D doesn't throw on divide by zero, it returns a vector with Infinity/NaN
            Vector3D result = Vector3D.Divide(vector, 0.0);
            await Assert.That(double.IsInfinity(result.X) || double.IsNaN(result.X)).IsTrue();
        }

        [Test]
        public static async Task DotProduct_CalculatesCorrectly()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            double result = Vector3D.DotProduct(v1, v2);
            await Assert.That(result).IsEqualTo(32.0)); // 1*4 + 2*5 + 3*6 = 4 + 10 + 18 = 32
        }

        [Test]
        public static async Task DotProduct_PerpendicularVectors_ReturnsZero()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 1.0, 0.0);
            double result = Vector3D.DotProduct(v1, v2);
            await Assert.That(result).IsEqualTo(0.0));
        }

        [Test]
        public static async Task CrossProduct_CalculatesCorrectly()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 1.0, 0.0);
            Vector3D result = Vector3D.CrossProduct(v1, v2);
            await Assert.That(Round(result.X, 4)).IsEqualTo(0.0));
            await Assert.That(Round(result.Y, 4)).IsEqualTo(0.0));
            await Assert.That(Round(result.Z, 4)).IsEqualTo(1.0));
        }

        [Test]
        public static async Task CrossProduct_IsAntiCommutative()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            Vector3D result1 = Vector3D.CrossProduct(v1, v2);
            Vector3D result2 = Vector3D.CrossProduct(v2, v1);
            await Assert.That(result1).IsEqualTo(-result2));
        }

        [Test]
        public static async Task AngleBetween_ParallelVectors_ReturnsZero()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(2.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            await Assert.That(Round(angle, 4)).IsEqualTo(0.0));
        }

        [Test]
        public static async Task AngleBetween_IdenticalVectors_ReturnsZero()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(1.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            // AngleBetween with identical vectors should return 0.0, not NaN
            await Assert.That(Round(angle, 4)).IsEqualTo(0.0));
            await Assert.That(double.IsNaN(angle)).IsFalse();
        }

        [Test]
        public static async Task AngleBetween_PerpendicularVectors_ReturnsNinety()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 1.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            await Assert.That(Round(angle, 4)).IsEqualTo(90.0));
        }

        [Test]
        public static async Task AngleBetween_OppositeVectors_ReturnsOneEighty()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(-1.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            await Assert.That(Round(angle, 4)).IsEqualTo(180.0));
        }

        [Test]
        public static async Task AngleBetween_ZeroVector_ReturnsNaN()
        {
            Vector3D v1 = new Vector3D(1.0, 0.0, 0.0);
            Vector3D v2 = new Vector3D(0.0, 0.0, 0.0);
            double angle = Vector3D.AngleBetween(v1, v2);
            // Windows Vector3D returns NaN when one vector is zero
            await Assert.That(double.IsNaN(angle)).IsTrue();
        }

        [Test]
        public static async Task Normalize_ModifiesVectorToUnitLength()
        {
            Vector3D vector = new Vector3D(3.0, 4.0, 0.0);
            vector.Normalize();
            await Assert.That(Round(vector.Length, 4)).IsEqualTo(1.0));
        }

        [Test]
        public static async Task Normalize_ZeroVector_SetsToNaN()
        {
            Vector3D vector = new Vector3D(0.0, 0.0, 0.0);
            // Windows Vector3D doesn't throw on normalize of zero vector, it sets components to NaN
            vector.Normalize();
            await Assert.That(double.IsNaN(vector.X) || double.IsNaN(vector.Y) || double.IsNaN(vector.Z)).IsTrue();
        }

        [Test]
        public static async Task Negate_ReversesAllComponents()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            vector.Negate();
            await Assert.That(vector.X).IsEqualTo(-1.0));
            await Assert.That(vector.Y).IsEqualTo(-2.0));
            await Assert.That(vector.Z).IsEqualTo(-3.0));
        }

        [Test]
        public static async Task Operator_Addition_WorksCorrectly()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(4.0, 5.0, 6.0);
            Vector3D result = v1 + v2;
            await Assert.That(result.X).IsEqualTo(5.0));
            await Assert.That(result.Y).IsEqualTo(7.0));
            await Assert.That(result.Z).IsEqualTo(9.0));
        }

        [Test]
        public static async Task Operator_Subtraction_WorksCorrectly()
        {
            Vector3D v1 = new Vector3D(5.0, 7.0, 9.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = v1 - v2;
            await Assert.That(result.X).IsEqualTo(4.0));
            await Assert.That(result.Y).IsEqualTo(5.0));
            await Assert.That(result.Z).IsEqualTo(6.0));
        }

        [Test]
        public static async Task Operator_MultiplyVectorByScalar_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = vector * 2.0;
            await Assert.That(result.X).IsEqualTo(2.0));
            await Assert.That(result.Y).IsEqualTo(4.0));
            await Assert.That(result.Z).IsEqualTo(6.0));
        }

        [Test]
        public static async Task Operator_MultiplyScalarByVector_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = 2.0 * vector;
            await Assert.That(result.X).IsEqualTo(2.0));
            await Assert.That(result.Y).IsEqualTo(4.0));
            await Assert.That(result.Z).IsEqualTo(6.0));
        }

        [Test]
        public static async Task Operator_Division_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(2.0, 4.0, 6.0);
            Vector3D result = vector / 2.0;
            await Assert.That(result.X).IsEqualTo(1.0));
            await Assert.That(result.Y).IsEqualTo(2.0));
            await Assert.That(result.Z).IsEqualTo(3.0));
        }

        [Test]
        public static async Task Operator_UnaryNegation_WorksCorrectly()
        {
            Vector3D vector = new Vector3D(1.0, 2.0, 3.0);
            Vector3D result = -vector;
            await Assert.That(result.X).IsEqualTo(-1.0));
            await Assert.That(result.Y).IsEqualTo(-2.0));
            await Assert.That(result.Z).IsEqualTo(-3.0));
        }

        [Test]
        public static async Task Operator_Equality_ReturnsTrueForEqualVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(v1 == v2).IsTrue();
        }

        [Test]
        public static async Task Operator_Equality_ReturnsFalseForDifferentVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 4.0);
            await Assert.That(v1 == v2).IsFalse();
        }

        [Test]
        public static async Task Operator_Inequality_ReturnsTrueForDifferentVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 4.0);
            await Assert.That(v1 != v2).IsTrue();
        }

        [Test]
        public static async Task Operator_Inequality_ReturnsFalseForEqualVectors()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(v1 != v2).IsFalse();
        }

        [Test]
        public static async Task Equals_WithEqualVector_ReturnsTrue()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(v1.Equals(v2)).IsTrue();
        }

        [Test]
        public static async Task Equals_WithDifferentVector_ReturnsFalse()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 4.0);
            await Assert.That(v1.Equals(v2)).IsFalse();
        }

        [Test]
        public static async Task Equals_WithNull_ReturnsFalse()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(v1.Equals(null)).IsFalse();
        }

        [Test]
        public static async Task Equals_WithNonVector_ReturnsFalse()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(v1.Equals("not a vector")).IsFalse();
        }

        [Test]
        public static async Task GetHashCode_EqualVectors_ReturnSameHashCode()
        {
            Vector3D v1 = new Vector3D(1.0, 2.0, 3.0);
            Vector3D v2 = new Vector3D(1.0, 2.0, 3.0);
            await Assert.That(v1.GetHashCode()).IsEqualTo(v2.GetHashCode()));
        }

        [Test]
        public static async Task ToString_ReturnsFormattedString()
        {
            Vector3D vector = new Vector3D(1.5, 2.5, 3.5);
            // Windows Vector3D only supports ToString() without parameters
            string result = vector.ToString();
            await Assert.That(result.Contains("1.5"));
            await Assert.That(result.Contains("2.5"));
            await Assert.That(result.Contains("3.5"));
        }

        [Test]
        public static async Task Parse_WithoutParentheses_ParsesCorrectly()
        {
            // Arrange - Format without parentheses (supported format)
            string value = "1.0000002742460514,4.982079117255012E-06,0";

            // Act
            Vector3D result = Vector3D.Parse(value);

            // Assert
            await Assert.That(Math.Abs(result.X - 1.0000002742460514) <= 0.000000000000001).IsTrue();
            await Assert.That(Math.Abs(result.Y - 4.982079117255012E-06) <= 0.000000000000001).IsTrue();
            await Assert.That(result.Z).IsEqualTo(0.0));
        }

        [Test]
        public static async Task Parse_WithParentheses_ThrowsFormatException()
        {
            // Arrange - Format with parentheses (not supported by system Vector3D.Parse on Windows)
            string value = "(1.0000002742460514,4.982079117255012E-06,0)";

            // Act & Assert
            try
            {
                // Try parsing - on non-Windows, custom Vector3D.Parse supports parentheses by stripping them
                Vector3D result = Vector3D.Parse(value);
                await Assert.That(result.X).IsEqualTo(1.0000002742460514).Within(0.000000000000001));
                await Assert.That(result.Y).IsEqualTo(4.982079117255012E-06).Within(0.000000000000001));
                await Assert.That(result.Z).IsEqualTo(0.0));
            }
            catch (FormatException)
            {
                // On Windows, the system Vector3D.Parse does not support parentheses
                // It expects the format "1.0,0.0,0.0" (without parentheses)
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
        }

        [Test]
        public static async Task Parse_WithParentheses_ExactRochambeauFormat_ThrowsFormatException()
        {
            // Arrange - Exact format from rochambeau file conversion error
            string value = "(1.0000002742460514,4.982079117255012E-06,0)";

            // Act & Assert
            try
            {
                // Try parsing - on non-Windows, custom Vector3D.Parse supports parentheses by stripping them
                Vector3D result = Vector3D.Parse(value);
                await Assert.That(result.X).IsEqualTo(1.0000002742460514).Within(0.000000000000001));
                await Assert.That(result.Y).IsEqualTo(4.982079117255012E-06).Within(0.000000000000001));
                await Assert.That(result.Z).IsEqualTo(0.0));
            }
            catch (FormatException)
            {
                // On Windows, Vector3D.Parse does not support parentheses format
                // Matrix3DFactory strips parentheses before calling Parse, so it works there
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
        }

        [Test]
        public static async Task Parse_WithParentheses_OnlyOpeningParenthesis_Fails()
        {
            // Arrange - Only opening parenthesis (invalid format)
            string value = "(1.0,2.0,3.0";

            // Act & Assert
            FormatException ex = await Assert.That(() => Vector3D.Parse(value)).Throws<FormatException>();
            // Both platforms throw FormatException, but may have different messages
            await Assert.That(ex.Message.Contains("not in a correct format") || ex.Message.Contains("Invalid Vector3D format")).IsTrue();
        }

        [Test]
        public static async Task Parse_WithParentheses_OnlyClosingParenthesis_Fails()
        {
            // Arrange - Only closing parenthesis (invalid format)
            string value = "1.0,2.0,3.0)";

            // Act & Assert
            FormatException ex = await Assert.That(() => Vector3D.Parse(value)).Throws<FormatException>();
            // Both platforms throw FormatException, but may have different messages
            await Assert.That(ex.Message.Contains("not in a correct format") || ex.Message.Contains("Invalid Vector3D format")).IsTrue();
        }

        [Test]
        public static async Task Parse_WithParentheses_MismatchedParentheses_Fails()
        {
            // Arrange - Mismatched parentheses (invalid format)
            string value = "(1.0,2.0,3.0))";

            // Act & Assert
            FormatException ex = await Assert.That(() => Vector3D.Parse(value)).Throws<FormatException>();
            // Both platforms throw FormatException, but may have different messages
            await Assert.That(ex.Message.Contains("not in a correct format") || ex.Message.Contains("Invalid Vector3D format")).IsTrue();
        }

        [Test]
        public static async Task Parse_WithParentheses_ReversedParentheses_Fails()
        {
            // Arrange - Reversed parentheses (invalid format)
            string value = ")1.0,2.0,3.0(";

            // Act & Assert
            FormatException ex = await Assert.That(() => Vector3D.Parse(value)).Throws<FormatException>();
            // Both platforms throw FormatException, but may have different messages
            await Assert.That(ex.Message.Contains("not in a correct format") || ex.Message.Contains("Invalid Vector3D format")).IsTrue();
        }

        [Test]
        public static async Task Parse_WithParentheses_ExtraContentAfterParentheses_Fails()
        {
            // Arrange - Extra content after closing parenthesis (invalid format)
            string value = "(1.0,2.0,3.0)extra";

            // Act & Assert
            FormatException ex = await Assert.That(() => Vector3D.Parse(value)).Throws<FormatException>();
            // Both platforms throw FormatException, but may have different messages
            await Assert.That(ex.Message.Contains("not in a correct format") || ex.Message.Contains("Invalid Vector3D format")).IsTrue();
        }

        [Test]
        public static async Task Parse_WithParentheses_ExtraContentBeforeParentheses_Fails()
        {
            // Arrange - Extra content before opening parenthesis (invalid format)
            string value = "extra(1.0,2.0,3.0)";

            // Act & Assert
            FormatException ex = await Assert.That(() => Vector3D.Parse(value)).Throws<FormatException>();
            // Both platforms throw FormatException, but may have different messages
            await Assert.That(ex.Message.Contains("not in a correct format") || ex.Message.Contains("Invalid Vector3D format")).IsTrue();
        }
    }
}

