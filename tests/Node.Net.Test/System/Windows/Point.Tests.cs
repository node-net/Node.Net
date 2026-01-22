using System.Threading.Tasks;
using static System.Math;

namespace Node.Net.Test
{
    internal class PointTests
    {
        [Test]
        public async Task Constructor_WithParameters_SetsProperties()
        {
            Point point = new Point(1.0, 2.0);
            await Assert.That(point.X).IsEqualTo(1.0);
            await Assert.That(point.Y).IsEqualTo(2.0);
        }

        [Test]
        public async Task Constructor_Default_InitializesToZero()
        {
            Point point = new Point();
            await Assert.That(point.X).IsEqualTo(0.0);
            await Assert.That(point.Y).IsEqualTo(0.0);
        }

        [Test]
        public async Task Properties_CanBeSet()
        {
            Point point = new Point();
            point.X = 5.0;
            point.Y = 6.0;
            await Assert.That(point.X).IsEqualTo(5.0);
            await Assert.That(point.Y).IsEqualTo(6.0);
        }

        [Test]
        public async Task Offset_WithDoubles_ModifiesPoint()
        {
            Point point = new Point(1.0, 2.0);
            point.Offset(10.0, 20.0);
            
            await Assert.That(point.X).IsEqualTo(11.0);
            await Assert.That(point.Y).IsEqualTo(22.0);
        }

        [Test]
        public async Task Add_PointAndVector_ReturnsNewPoint()
        {
            Point point = new Point(1.0, 2.0);
            Vector vector = new Vector(3.0, 4.0);
            Point result = Point.Add(point, vector);
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(6.0);
        }

        [Test]
        public async Task Subtract_PointFromPoint_ReturnsVector()
        {
            Point point1 = new Point(5.0, 6.0);
            Point point2 = new Point(1.0, 2.0);
            Vector result = Point.Subtract(point1, point2);
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(4.0);
        }

        [Test]
        public async Task Subtract_VectorFromPoint_ReturnsPoint()
        {
            Point point = new Point(5.0, 6.0);
            Vector vector = new Vector(1.0, 2.0);
            Point result = Point.Subtract(point, vector);
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(4.0);
        }

        // Note: Windows Point doesn't have Divide static method
        // These tests are skipped to match Windows API exactly

        [Test]
        public async Task OperatorAdd_PointAndVector_ReturnsPoint()
        {
            Point point = new Point(1.0, 2.0);
            Vector vector = new Vector(3.0, 4.0);
            Point result = point + vector;
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(6.0);
        }

        [Test]
        public async Task OperatorSubtract_PointFromPoint_ReturnsVector()
        {
            Point point1 = new Point(5.0, 6.0);
            Point point2 = new Point(1.0, 2.0);
            Vector result = point1 - point2;
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(4.0);
        }

        [Test]
        public async Task OperatorSubtract_VectorFromPoint_ReturnsPoint()
        {
            Point point = new Point(5.0, 6.0);
            Vector vector = new Vector(1.0, 2.0);
            Point result = point - vector;
            
            await Assert.That(result.X).IsEqualTo(4.0);
            await Assert.That(result.Y).IsEqualTo(4.0);
        }

        // Note: Windows Point doesn't have division operator
        // This test is skipped to match Windows API exactly

        [Test]
        public async Task OperatorEquals_EqualPoints_ReturnsTrue()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(1.0, 2.0);
            
            await Assert.That(p1 == p2).IsTrue();
        }

        [Test]
        public async Task OperatorNotEquals_DifferentPoints_ReturnsTrue()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(3.0, 4.0);
            
            await Assert.That(p1 != p2).IsTrue();
        }

        [Test]
        public async Task Equals_SameValues_ReturnsTrue()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(1.0, 2.0);
            
            await Assert.That(p1.Equals(p2)).IsTrue();
        }

        [Test]
        public async Task GetHashCode_EqualPoints_ReturnSameHashCode()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(1.0, 2.0);
            await Assert.That(p1.GetHashCode()).IsEqualTo(p2.GetHashCode());
        }

        [Test]
        public async Task ToString_ReturnsFormattedString()
        {
            Point point = new Point(1.5, 2.5);
            // Windows Point only supports ToString() without parameters
            string result = point.ToString();
            await Assert.That(result.Contains("1.5")).IsTrue();
            await Assert.That(result.Contains("2.5")).IsTrue();
        }
    }
}

