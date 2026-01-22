using System;
using System.Threading.Tasks;
using static System.Math;

namespace Node.Net.Test
{
    internal static class Rect3DTests
    {
        [Test]
        public static async Task Constructor_WithPointAndSize_SetsProperties()
        {
            Point3D location = new Point3D(1.0, 2.0, 3.0);
            Size3D size = new Size3D(10.0, 20.0, 30.0);
            Rect3D rect = new Rect3D(location, size);
            
            await Assert.That(rect.X).IsEqualTo(1.0);
            await Assert.That(rect.Y).IsEqualTo(2.0);
            await Assert.That(rect.Z).IsEqualTo(3.0);
            await Assert.That(rect.SizeX).IsEqualTo(10.0);
            await Assert.That(rect.SizeY).IsEqualTo(20.0);
            await Assert.That(rect.SizeZ).IsEqualTo(30.0);
        }

        [Test]
        public static async Task Constructor_WithDoubles_SetsProperties()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            
            await Assert.That(rect.X).IsEqualTo(1.0);
            await Assert.That(rect.Y).IsEqualTo(2.0);
            await Assert.That(rect.Z).IsEqualTo(3.0);
            await Assert.That(rect.SizeX).IsEqualTo(10.0);
            await Assert.That(rect.SizeY).IsEqualTo(20.0);
            await Assert.That(rect.SizeZ).IsEqualTo(30.0);
        }

        [Test]
        public static async Task Constructor_Default_InitializesToZero()
        {
            Rect3D rect = new Rect3D();
            await Assert.That(rect.X).IsEqualTo(0.0);
            await Assert.That(rect.Y).IsEqualTo(0.0);
            await Assert.That(rect.Z).IsEqualTo(0.0);
            await Assert.That(rect.SizeX).IsEqualTo(0.0);
            await Assert.That(rect.SizeY).IsEqualTo(0.0);
            await Assert.That(rect.SizeZ).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Properties_CanBeSet()
        {
            Rect3D rect = new Rect3D();
            rect.X = 5.0;
            rect.Y = 6.0;
            rect.Z = 7.0;
            rect.SizeX = 10.0;
            rect.SizeY = 20.0;
            rect.SizeZ = 30.0;
            
            await Assert.That(rect.X).IsEqualTo(5.0);
            await Assert.That(rect.Y).IsEqualTo(6.0);
            await Assert.That(rect.Z).IsEqualTo(7.0);
            await Assert.That(rect.SizeX).IsEqualTo(10.0);
            await Assert.That(rect.SizeY).IsEqualTo(20.0);
            await Assert.That(rect.SizeZ).IsEqualTo(30.0);
        }

        [Test]
        public static async Task Location_Property_GetAndSet()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Point3D location = rect.Location;
            
            await Assert.That(location.X).IsEqualTo(1.0);
            await Assert.That(location.Y).IsEqualTo(2.0);
            await Assert.That(location.Z).IsEqualTo(3.0);
            
            rect.Location = new Point3D(5.0, 6.0, 7.0);
            await Assert.That(rect.X).IsEqualTo(5.0);
            await Assert.That(rect.Y).IsEqualTo(6.0);
            await Assert.That(rect.Z).IsEqualTo(7.0);
        }

        [Test]
        public static async Task Size_Property_GetAndSet()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Size3D size = rect.Size;
            
            await Assert.That(size.X).IsEqualTo(10.0);
            await Assert.That(size.Y).IsEqualTo(20.0);
            await Assert.That(size.Z).IsEqualTo(30.0);
            
            rect.Size = new Size3D(15.0, 25.0, 35.0);
            await Assert.That(rect.SizeX).IsEqualTo(15.0);
            await Assert.That(rect.SizeY).IsEqualTo(25.0);
            await Assert.That(rect.SizeZ).IsEqualTo(35.0);
        }

        [Test]
        public static async Task IsEmpty_WithPositiveSizes_ReturnsFalse()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            await Assert.That(rect.IsEmpty).IsFalse();
        }

        [Test]
        public static async Task Constructor_WithNegativeSizeX_ThrowsArgumentException()
        {
            // Windows Rect3D throws ArgumentException for negative dimensions
            await Assert.That(() => new Rect3D(1.0, 2.0, 3.0, -10.0, 20.0, 30.0)).Throws<ArgumentException>();
        }

        [Test]
        public static async Task Constructor_WithNegativeSizeY_ThrowsArgumentException()
        {
            // Windows Rect3D throws ArgumentException for negative dimensions
            await Assert.That(() => new Rect3D(1.0, 2.0, 3.0, 10.0, -20.0, 30.0)).Throws<ArgumentException>();
        }

        [Test]
        public static async Task Constructor_WithNegativeSizeZ_ThrowsArgumentException()
        {
            // Windows Rect3D throws ArgumentException for negative dimensions
            await Assert.That(() => new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, -30.0)).Throws<ArgumentException>();
        }

        [Test]
        public static async Task Empty_StaticProperty_IsEmpty()
        {
            Rect3D empty = Rect3D.Empty;
            await Assert.That(empty.IsEmpty).IsTrue();
        }

        [Test]
        public static async Task OperatorEquals_EqualRects_ReturnsTrue()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            
            await Assert.That(r1 == r2).IsTrue();
        }

        [Test]
        public static async Task OperatorNotEquals_DifferentRects_ReturnsTrue()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(5.0, 6.0, 7.0, 15.0, 25.0, 35.0);
            
            await Assert.That(r1 != r2).IsTrue();
        }

        [Test]
        public static async Task Equals_SameValues_ReturnsTrue()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            
            await Assert.That(r1.Equals(r2)).IsTrue();
        }

        [Test]
        public static async Task GetHashCode_EqualRects_ReturnSameHashCode()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            await Assert.That(r1.GetHashCode()).IsEqualTo(r2.GetHashCode());
        }

        [Test]
        public static async Task ToString_ReturnsFormattedString()
        {
            Rect3D rect = new Rect3D(1.5, 2.5, 3.5, 10.5, 20.5, 30.5);
            // Windows Rect3D only supports ToString() without parameters
            string result = rect.ToString();
            await Assert.That(result.Contains("1.5")).IsTrue();
            await Assert.That(result.Contains("2.5")).IsTrue();
            await Assert.That(result.Contains("3.5")).IsTrue();
            await Assert.That(result.Contains("10.5")).IsTrue();
            await Assert.That(result.Contains("20.5")).IsTrue();
            await Assert.That(result.Contains("30.5")).IsTrue();
        }
    }
}

