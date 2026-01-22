using System;
using System.Threading.Tasks;
using static System.Math;

namespace Node.Net.Test
{
    internal static class Point3DTests
    {
        [Test]
        public static async Task Constructor_WithParameters_SetsProperties()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            await Assert.That(point.X).IsEqualTo(1.0);
            await Assert.That(point.Y).IsEqualTo(2.0);
            await Assert.That(point.Z).IsEqualTo(3.0);
        }

        [Test]
        public static async Task Constructor_Default_InitializesToZero()
        {
            Point3D point = new Point3D();
            await Assert.That(point.X).IsEqualTo(0.0);
            await Assert.That(point.Y).IsEqualTo(0.0);
            await Assert.That(point.Z).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Properties_CanBeSet()
        {
            Point3D point = new Point3D();
            point.X = 5.0;
            point.Y = 6.0;
            point.Z = 7.0;
            await Assert.That(point.X).IsEqualTo(5.0);
            await Assert.That(point.Y).IsEqualTo(6.0);
            await Assert.That(point.Z).IsEqualTo(7.0);
        }

        [Test]
        public static async Task Offset_WithDoubles_ModifiesPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            point.Offset(10.0, 20.0, 30.0);
            
            await Assert.That(point.X).IsEqualTo(11.0);
            await Assert.That(point.Y).IsEqualTo(22.0);
            await Assert.That(point.Z).IsEqualTo(33.0);
        }

        [Test]
        public static async Task Add_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = Point3D.Add(point, vector);
            
            await Assert.That(result.X).IsEqualTo(11.0);
            await Assert.That(result.Y).IsEqualTo(22.0);
            await Assert.That(result.Z).IsEqualTo(33.0);
            // Original point should not be modified
            await Assert.That(point.X).IsEqualTo(1.0);
        }

        [Test]
        public static async Task Subtract_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(11.0, 22.0, 33.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = Point3D.Subtract(point, vector);
            
            await Assert.That(result.X).IsEqualTo(1.0);
            await Assert.That(result.Y).IsEqualTo(2.0);
            await Assert.That(result.Z).IsEqualTo(3.0);
        }

        [Test]
        public static async Task Subtract_TwoPoints_ReturnsVector()
        {
            Point3D point1 = new Point3D(11.0, 22.0, 33.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            Vector3D result = Point3D.Subtract(point1, point2);
            
            await Assert.That(result.X).IsEqualTo(10.0);
            await Assert.That(result.Y).IsEqualTo(20.0);
            await Assert.That(result.Z).IsEqualTo(30.0);
        }

        [Test]
        public static async Task Multiply_PointAndMatrix_ReturnsTransformedPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Point3D result = Point3D.Multiply(point, matrix);
            
            await Assert.That(result.X).IsEqualTo(11.0);
            await Assert.That(result.Y).IsEqualTo(22.0);
            await Assert.That(result.Z).IsEqualTo(33.0);
        }

        [Test]
        public static async Task OperatorAdd_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = point + vector;
            
            await Assert.That(result.X).IsEqualTo(11.0);
            await Assert.That(result.Y).IsEqualTo(22.0);
            await Assert.That(result.Z).IsEqualTo(33.0);
        }

        [Test]
        public static async Task OperatorSubtract_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(11.0, 22.0, 33.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = point - vector;
            
            await Assert.That(result.X).IsEqualTo(1.0);
            await Assert.That(result.Y).IsEqualTo(2.0);
            await Assert.That(result.Z).IsEqualTo(3.0);
        }

        [Test]
        public static async Task OperatorSubtract_TwoPoints_ReturnsVector()
        {
            Point3D point1 = new Point3D(11.0, 22.0, 33.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            Vector3D result = point1 - point2;
            
            await Assert.That(result.X).IsEqualTo(10.0);
            await Assert.That(result.Y).IsEqualTo(20.0);
            await Assert.That(result.Z).IsEqualTo(30.0);
        }

        [Test]
        public static async Task OperatorMultiply_PointAndMatrix_ReturnsTransformedPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Point3D result = point * matrix;
            
            await Assert.That(result.X).IsEqualTo(11.0);
            await Assert.That(result.Y).IsEqualTo(22.0);
            await Assert.That(result.Z).IsEqualTo(33.0);
        }

        [Test]
        public static async Task OperatorEquality_EqualPoints_ReturnsTrue()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            await Assert.That(point1 == point2).IsTrue();
        }

        [Test]
        public static async Task OperatorEquality_DifferentPoints_ReturnsFalse()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 4.0);
            
            await Assert.That(point1 == point2).IsFalse();
        }

        [Test]
        public static async Task OperatorInequality_EqualPoints_ReturnsFalse()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            await Assert.That(point1 != point2).IsFalse();
        }

        [Test]
        public static async Task OperatorInequality_DifferentPoints_ReturnsTrue()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 4.0);
            
            await Assert.That(point1 != point2).IsTrue();
        }

        [Test]
        public static async Task Equals_WithSameValues_ReturnsTrue()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            await Assert.That(point1.Equals(point2)).IsTrue();
        }

        [Test]
        public static async Task Equals_WithDifferentValues_ReturnsFalse()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 4.0);
            
            await Assert.That(point1.Equals(point2)).IsFalse();
        }

        [Test]
        public static async Task Equals_WithNull_ReturnsFalse()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            
            await Assert.That(point.Equals(null)).IsFalse();
        }

        [Test]
        public static async Task Equals_WithNonPoint3D_ReturnsFalse()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            
            await Assert.That(point.Equals("not a point")).IsFalse();
        }

        [Test]
        public static async Task GetHashCode_SameValues_ReturnsSameHashCode()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            await Assert.That(point1.GetHashCode()).IsEqualTo(point2.GetHashCode());
        }

        [Test]
        public static async Task ToString_ReturnsFormattedString()
        {
            Point3D point = new Point3D(1.5, 2.5, 3.5);
            // Windows Point3D only supports ToString() without parameters
            string result = point.ToString();
            await Assert.That(result.Contains("1.5")).IsTrue();
            await Assert.That(result.Contains("2.5")).IsTrue();
            await Assert.That(result.Contains("3.5")).IsTrue();
        }

        [Test]
        public static async Task Offset_MultipleTimes_Accumulates()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            point.Offset(10.0, 20.0, 30.0);
            point.Offset(5.0, 5.0, 5.0);
            
            await Assert.That(point.X).IsEqualTo(16.0);
            await Assert.That(point.Y).IsEqualTo(27.0);
            await Assert.That(point.Z).IsEqualTo(38.0);
        }

        [Test]
        public static async Task Subtract_PointFromItself_ReturnsZeroVector()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Vector3D result = Point3D.Subtract(point, point);
            
            await Assert.That(result.X).IsEqualTo(0.0);
            await Assert.That(result.Y).IsEqualTo(0.0);
            await Assert.That(result.Z).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Multiply_WithIdentityMatrix_ReturnsSamePoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Matrix3D matrix = new Matrix3D(); // Identity matrix
            Point3D result = Point3D.Multiply(point, matrix);
            
            await Assert.That(result.X).IsEqualTo(1.0);
            await Assert.That(result.Y).IsEqualTo(2.0);
            await Assert.That(result.Z).IsEqualTo(3.0);
        }

        [Test]
        public static async Task Multiply_WithRotationMatrix_RotatesPoint()
        {
            Point3D point = new Point3D(1.0, 0.0, 0.0);
            Matrix3D matrix = new Matrix3D();
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), 90));
            Point3D result = Point3D.Multiply(point, matrix);
            
            // 90 degree rotation around Z axis: (1,0,0) -> (0,1,0)
            await Assert.That(Round(result.X, 6)).IsEqualTo(0.0);
            await Assert.That(Round(result.Y, 6)).IsEqualTo(1.0);
            await Assert.That(Round(result.Z, 6)).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Parse_ValidInput_ReturnsExpectedPoint()
        {
            string input = "1.5,2.5,3.5";
            Point3D expected = new Point3D(1.5, 2.5, 3.5);
            Point3D result = Point3D.Parse(input);
            
            await Assert.That(result.X).IsEqualTo(1.5);
            await Assert.That(result.Y).IsEqualTo(2.5);
            await Assert.That(result.Z).IsEqualTo(3.5);
            await Assert.That(result).IsEqualTo(expected);
        }

        [Test]
        public static async Task Parse_WithWhitespace_ReturnsExpectedPoint()
        {
            string input = " 1.5 , 2.5 , 3.5 ";
            Point3D result = Point3D.Parse(input);
            
            await Assert.That(result.X).IsEqualTo(1.5);
            await Assert.That(result.Y).IsEqualTo(2.5);
            await Assert.That(result.Z).IsEqualTo(3.5);
        }

        [Test]
        public static async Task Parse_NullInput_ThrowsException()
        {
            // Windows Point3D.Parse throws ArgumentException for null (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // TUnit: use Throws<Exception> to catch any exception type
            await Assert.That(() => Point3D.Parse(null!)).Throws<Exception>();
        }

        [Test]
        public static async Task Parse_EmptyInput_ThrowsException()
        {
            // Windows Point3D.Parse throws FormatException for empty string (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // TUnit: use Throws<Exception> to catch any exception type
            await Assert.That(() => Point3D.Parse("")).Throws<Exception>();
        }

        [Test]
        public static async Task Parse_InvalidFormat_ThrowsException()
        {
            // Windows Point3D.Parse throws FormatException for invalid formats (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // TUnit: use Throws<Exception> to catch any exception type
            await Assert.That(() => Point3D.Parse("1,2")).Throws<Exception>();
            await Assert.That(() => Point3D.Parse("1,2,3,4")).Throws<Exception>();
            await Assert.That(() => Point3D.Parse("invalid")).Throws<Exception>();
        }

        [Test]
        public static async Task Parse_InvalidNumbers_ThrowsFormatException()
        {
            // Windows Point3D.Parse throws FormatException for invalid numbers (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // TUnit: use Throws<Exception> to catch any exception type
            await Assert.That(() => Point3D.Parse("a,b,c")).Throws<Exception>();
        }
    }
}


