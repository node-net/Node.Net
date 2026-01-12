extern alias NodeNet;
using NUnit.Framework;
using NodeNet::System.Windows;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class PointTests
    {
        [Test]
        public static void Constructor_WithParameters_SetsProperties()
        {
            Point point = new Point(1.0, 2.0);
            Assert.That(point.X, Is.EqualTo(1.0));
            Assert.That(point.Y, Is.EqualTo(2.0));
        }

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Point point = new Point();
            Assert.That(point.X, Is.EqualTo(0.0));
            Assert.That(point.Y, Is.EqualTo(0.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Point point = new Point();
            point.X = 5.0;
            point.Y = 6.0;
            Assert.That(point.X, Is.EqualTo(5.0));
            Assert.That(point.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void Offset_WithDoubles_ModifiesPoint()
        {
            Point point = new Point(1.0, 2.0);
            point.Offset(10.0, 20.0);
            
            Assert.That(point.X, Is.EqualTo(11.0));
            Assert.That(point.Y, Is.EqualTo(22.0));
        }

        [Test]
        public static void Add_PointAndVector_ReturnsNewPoint()
        {
            Point point = new Point(1.0, 2.0);
            Vector vector = new Vector(3.0, 4.0);
            Point result = Point.Add(point, vector);
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void Subtract_PointFromPoint_ReturnsVector()
        {
            Point point1 = new Point(5.0, 6.0);
            Point point2 = new Point(1.0, 2.0);
            Vector result = Point.Subtract(point1, point2);
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
        }

        [Test]
        public static void Subtract_VectorFromPoint_ReturnsPoint()
        {
            Point point = new Point(5.0, 6.0);
            Vector vector = new Vector(1.0, 2.0);
            Point result = Point.Subtract(point, vector);
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
        }

        // Note: Windows Point doesn't have Divide static method
        // These tests are skipped to match Windows API exactly

        [Test]
        public static void OperatorAdd_PointAndVector_ReturnsPoint()
        {
            Point point = new Point(1.0, 2.0);
            Vector vector = new Vector(3.0, 4.0);
            Point result = point + vector;
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(6.0));
        }

        [Test]
        public static void OperatorSubtract_PointFromPoint_ReturnsVector()
        {
            Point point1 = new Point(5.0, 6.0);
            Point point2 = new Point(1.0, 2.0);
            Vector result = point1 - point2;
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
        }

        [Test]
        public static void OperatorSubtract_VectorFromPoint_ReturnsPoint()
        {
            Point point = new Point(5.0, 6.0);
            Vector vector = new Vector(1.0, 2.0);
            Point result = point - vector;
            
            Assert.That(result.X, Is.EqualTo(4.0));
            Assert.That(result.Y, Is.EqualTo(4.0));
        }

        // Note: Windows Point doesn't have division operator
        // This test is skipped to match Windows API exactly

        [Test]
        public static void OperatorEquals_EqualPoints_ReturnsTrue()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(1.0, 2.0);
            
            Assert.That(p1 == p2, Is.True);
        }

        [Test]
        public static void OperatorNotEquals_DifferentPoints_ReturnsTrue()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(3.0, 4.0);
            
            Assert.That(p1 != p2, Is.True);
        }

        [Test]
        public static void Equals_SameValues_ReturnsTrue()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(1.0, 2.0);
            
            Assert.That(p1.Equals(p2), Is.True);
        }

        [Test]
        public static void GetHashCode_EqualPoints_ReturnSameHashCode()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(1.0, 2.0);
            Assert.That(p1.GetHashCode(), Is.EqualTo(p2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Point point = new Point(1.5, 2.5);
            // Windows Point only supports ToString() without parameters
            string result = point.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
        }
    }
}

