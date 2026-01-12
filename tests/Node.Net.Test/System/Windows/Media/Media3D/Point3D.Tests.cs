extern alias NodeNet;
using NUnit.Framework;
using System;
using NodeNet::System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class Point3DTests
    {
        [Test]
        public static void Constructor_WithParameters_SetsProperties()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Assert.That(point.X, Is.EqualTo(1.0));
            Assert.That(point.Y, Is.EqualTo(2.0));
            Assert.That(point.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Point3D point = new Point3D();
            Assert.That(point.X, Is.EqualTo(0.0));
            Assert.That(point.Y, Is.EqualTo(0.0));
            Assert.That(point.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Point3D point = new Point3D();
            point.X = 5.0;
            point.Y = 6.0;
            point.Z = 7.0;
            Assert.That(point.X, Is.EqualTo(5.0));
            Assert.That(point.Y, Is.EqualTo(6.0));
            Assert.That(point.Z, Is.EqualTo(7.0));
        }

        [Test]
        public static void Offset_WithDoubles_ModifiesPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            point.Offset(10.0, 20.0, 30.0);
            
            Assert.That(point.X, Is.EqualTo(11.0));
            Assert.That(point.Y, Is.EqualTo(22.0));
            Assert.That(point.Z, Is.EqualTo(33.0));
        }

        [Test]
        public static void Add_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = Point3D.Add(point, vector);
            
            Assert.That(result.X, Is.EqualTo(11.0));
            Assert.That(result.Y, Is.EqualTo(22.0));
            Assert.That(result.Z, Is.EqualTo(33.0));
            // Original point should not be modified
            Assert.That(point.X, Is.EqualTo(1.0));
        }

        [Test]
        public static void Subtract_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(11.0, 22.0, 33.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = Point3D.Subtract(point, vector);
            
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Subtract_TwoPoints_ReturnsVector()
        {
            Point3D point1 = new Point3D(11.0, 22.0, 33.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            Vector3D result = Point3D.Subtract(point1, point2);
            
            Assert.That(result.X, Is.EqualTo(10.0));
            Assert.That(result.Y, Is.EqualTo(20.0));
            Assert.That(result.Z, Is.EqualTo(30.0));
        }

        [Test]
        public static void Multiply_PointAndMatrix_ReturnsTransformedPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Point3D result = Point3D.Multiply(point, matrix);
            
            Assert.That(result.X, Is.EqualTo(11.0));
            Assert.That(result.Y, Is.EqualTo(22.0));
            Assert.That(result.Z, Is.EqualTo(33.0));
        }

        [Test]
        public static void OperatorAdd_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = point + vector;
            
            Assert.That(result.X, Is.EqualTo(11.0));
            Assert.That(result.Y, Is.EqualTo(22.0));
            Assert.That(result.Z, Is.EqualTo(33.0));
        }

        [Test]
        public static void OperatorSubtract_PointAndVector_ReturnsNewPoint()
        {
            Point3D point = new Point3D(11.0, 22.0, 33.0);
            Vector3D vector = new Vector3D(10.0, 20.0, 30.0);
            Point3D result = point - vector;
            
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void OperatorSubtract_TwoPoints_ReturnsVector()
        {
            Point3D point1 = new Point3D(11.0, 22.0, 33.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            Vector3D result = point1 - point2;
            
            Assert.That(result.X, Is.EqualTo(10.0));
            Assert.That(result.Y, Is.EqualTo(20.0));
            Assert.That(result.Z, Is.EqualTo(30.0));
        }

        [Test]
        public static void OperatorMultiply_PointAndMatrix_ReturnsTransformedPoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Matrix3D matrix = new Matrix3D();
            matrix.Translate(new Vector3D(10.0, 20.0, 30.0));
            Point3D result = point * matrix;
            
            Assert.That(result.X, Is.EqualTo(11.0));
            Assert.That(result.Y, Is.EqualTo(22.0));
            Assert.That(result.Z, Is.EqualTo(33.0));
        }

        [Test]
        public static void OperatorEquality_EqualPoints_ReturnsTrue()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            Assert.That(point1 == point2, Is.True);
        }

        [Test]
        public static void OperatorEquality_DifferentPoints_ReturnsFalse()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 4.0);
            
            Assert.That(point1 == point2, Is.False);
        }

        [Test]
        public static void OperatorInequality_EqualPoints_ReturnsFalse()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            Assert.That(point1 != point2, Is.False);
        }

        [Test]
        public static void OperatorInequality_DifferentPoints_ReturnsTrue()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 4.0);
            
            Assert.That(point1 != point2, Is.True);
        }

        [Test]
        public static void Equals_WithSameValues_ReturnsTrue()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            Assert.That(point1.Equals(point2), Is.True);
        }

        [Test]
        public static void Equals_WithDifferentValues_ReturnsFalse()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 4.0);
            
            Assert.That(point1.Equals(point2), Is.False);
        }

        [Test]
        public static void Equals_WithNull_ReturnsFalse()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            
            Assert.That(point.Equals(null), Is.False);
        }

        [Test]
        public static void Equals_WithNonPoint3D_ReturnsFalse()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            
            Assert.That(point.Equals("not a point"), Is.False);
        }

        [Test]
        public static void GetHashCode_SameValues_ReturnsSameHashCode()
        {
            Point3D point1 = new Point3D(1.0, 2.0, 3.0);
            Point3D point2 = new Point3D(1.0, 2.0, 3.0);
            
            Assert.That(point1.GetHashCode(), Is.EqualTo(point2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Point3D point = new Point3D(1.5, 2.5, 3.5);
            // Windows Point3D only supports ToString() without parameters
            string result = point.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
            Assert.That(result, Does.Contain("3.5"));
        }

        [Test]
        public static void Offset_MultipleTimes_Accumulates()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            point.Offset(10.0, 20.0, 30.0);
            point.Offset(5.0, 5.0, 5.0);
            
            Assert.That(point.X, Is.EqualTo(16.0));
            Assert.That(point.Y, Is.EqualTo(27.0));
            Assert.That(point.Z, Is.EqualTo(38.0));
        }

        [Test]
        public static void Subtract_PointFromItself_ReturnsZeroVector()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Vector3D result = Point3D.Subtract(point, point);
            
            Assert.That(result.X, Is.EqualTo(0.0));
            Assert.That(result.Y, Is.EqualTo(0.0));
            Assert.That(result.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Multiply_WithIdentityMatrix_ReturnsSamePoint()
        {
            Point3D point = new Point3D(1.0, 2.0, 3.0);
            Matrix3D matrix = new Matrix3D(); // Identity matrix
            Point3D result = Point3D.Multiply(point, matrix);
            
            Assert.That(result.X, Is.EqualTo(1.0));
            Assert.That(result.Y, Is.EqualTo(2.0));
            Assert.That(result.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Multiply_WithRotationMatrix_RotatesPoint()
        {
            Point3D point = new Point3D(1.0, 0.0, 0.0);
            Matrix3D matrix = new Matrix3D();
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), 90));
            Point3D result = Point3D.Multiply(point, matrix);
            
            // 90 degree rotation around Z axis: (1,0,0) -> (0,1,0)
            Assert.That(Round(result.X, 6), Is.EqualTo(0.0));
            Assert.That(Round(result.Y, 6), Is.EqualTo(1.0));
            Assert.That(Round(result.Z, 6), Is.EqualTo(0.0));
        }

        [Test]
        public static void Parse_ValidInput_ReturnsExpectedPoint()
        {
            string input = "1.5,2.5,3.5";
            Point3D expected = new Point3D(1.5, 2.5, 3.5);
            Point3D result = Point3D.Parse(input);
            
            Assert.That(result.X, Is.EqualTo(1.5));
            Assert.That(result.Y, Is.EqualTo(2.5));
            Assert.That(result.Z, Is.EqualTo(3.5));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public static void Parse_WithWhitespace_ReturnsExpectedPoint()
        {
            string input = " 1.5 , 2.5 , 3.5 ";
            Point3D result = Point3D.Parse(input);
            
            Assert.That(result.X, Is.EqualTo(1.5));
            Assert.That(result.Y, Is.EqualTo(2.5));
            Assert.That(result.Z, Is.EqualTo(3.5));
        }

        [Test]
        public static void Parse_NullInput_ThrowsException()
        {
            // Windows Point3D.Parse throws ArgumentException for null (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // Use Assert.Catch to catch any exception type
            Assert.Catch(() => Point3D.Parse(null!));
        }

        [Test]
        public static void Parse_EmptyInput_ThrowsException()
        {
            // Windows Point3D.Parse throws FormatException for empty string (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // Use Assert.Catch to catch any exception type
            Assert.Catch(() => Point3D.Parse(""));
        }

        [Test]
        public static void Parse_InvalidFormat_ThrowsException()
        {
            // Windows Point3D.Parse throws FormatException for invalid formats (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // Use Assert.Catch to catch any exception type
            Assert.Catch(() => Point3D.Parse("1,2"));
            Assert.Catch(() => Point3D.Parse("1,2,3,4"));
            Assert.Catch(() => Point3D.Parse("invalid"));
        }

        [Test]
        public static void Parse_InvalidNumbers_ThrowsFormatException()
        {
            // Windows Point3D.Parse throws FormatException for invalid numbers (our implementation)
            // Windows native throws from TokenizerHelper (different exception type)
            // Use Assert.Catch to catch any exception type
            Assert.Catch(() => Point3D.Parse("a,b,c"));
        }
    }
}


