using System;
using NUnit.Framework;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class Rect3DTests
    {
        [Test]
        public static void Constructor_WithPointAndSize_SetsProperties()
        {
            Point3D location = new Point3D(1.0, 2.0, 3.0);
            Size3D size = new Size3D(10.0, 20.0, 30.0);
            Rect3D rect = new Rect3D(location, size);
            
            Assert.That(rect.X, Is.EqualTo(1.0));
            Assert.That(rect.Y, Is.EqualTo(2.0));
            Assert.That(rect.Z, Is.EqualTo(3.0));
            Assert.That(rect.SizeX, Is.EqualTo(10.0));
            Assert.That(rect.SizeY, Is.EqualTo(20.0));
            Assert.That(rect.SizeZ, Is.EqualTo(30.0));
        }

        [Test]
        public static void Constructor_WithDoubles_SetsProperties()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            
            Assert.That(rect.X, Is.EqualTo(1.0));
            Assert.That(rect.Y, Is.EqualTo(2.0));
            Assert.That(rect.Z, Is.EqualTo(3.0));
            Assert.That(rect.SizeX, Is.EqualTo(10.0));
            Assert.That(rect.SizeY, Is.EqualTo(20.0));
            Assert.That(rect.SizeZ, Is.EqualTo(30.0));
        }

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Rect3D rect = new Rect3D();
            Assert.That(rect.X, Is.EqualTo(0.0));
            Assert.That(rect.Y, Is.EqualTo(0.0));
            Assert.That(rect.Z, Is.EqualTo(0.0));
            Assert.That(rect.SizeX, Is.EqualTo(0.0));
            Assert.That(rect.SizeY, Is.EqualTo(0.0));
            Assert.That(rect.SizeZ, Is.EqualTo(0.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Rect3D rect = new Rect3D();
            rect.X = 5.0;
            rect.Y = 6.0;
            rect.Z = 7.0;
            rect.SizeX = 10.0;
            rect.SizeY = 20.0;
            rect.SizeZ = 30.0;
            
            Assert.That(rect.X, Is.EqualTo(5.0));
            Assert.That(rect.Y, Is.EqualTo(6.0));
            Assert.That(rect.Z, Is.EqualTo(7.0));
            Assert.That(rect.SizeX, Is.EqualTo(10.0));
            Assert.That(rect.SizeY, Is.EqualTo(20.0));
            Assert.That(rect.SizeZ, Is.EqualTo(30.0));
        }

        [Test]
        public static void Location_Property_GetAndSet()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Point3D location = rect.Location;
            
            Assert.That(location.X, Is.EqualTo(1.0));
            Assert.That(location.Y, Is.EqualTo(2.0));
            Assert.That(location.Z, Is.EqualTo(3.0));
            
            rect.Location = new Point3D(5.0, 6.0, 7.0);
            Assert.That(rect.X, Is.EqualTo(5.0));
            Assert.That(rect.Y, Is.EqualTo(6.0));
            Assert.That(rect.Z, Is.EqualTo(7.0));
        }

        [Test]
        public static void Size_Property_GetAndSet()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Size3D size = rect.Size;
            
            Assert.That(size.X, Is.EqualTo(10.0));
            Assert.That(size.Y, Is.EqualTo(20.0));
            Assert.That(size.Z, Is.EqualTo(30.0));
            
            rect.Size = new Size3D(15.0, 25.0, 35.0);
            Assert.That(rect.SizeX, Is.EqualTo(15.0));
            Assert.That(rect.SizeY, Is.EqualTo(25.0));
            Assert.That(rect.SizeZ, Is.EqualTo(35.0));
        }

        [Test]
        public static void IsEmpty_WithPositiveSizes_ReturnsFalse()
        {
            Rect3D rect = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Assert.That(rect.IsEmpty, Is.False);
        }

        [Test]
        public static void Constructor_WithNegativeSizeX_ThrowsArgumentException()
        {
            // Windows Rect3D throws ArgumentException for negative dimensions
            Assert.Throws<ArgumentException>(() => new Rect3D(1.0, 2.0, 3.0, -10.0, 20.0, 30.0));
        }

        [Test]
        public static void Constructor_WithNegativeSizeY_ThrowsArgumentException()
        {
            // Windows Rect3D throws ArgumentException for negative dimensions
            Assert.Throws<ArgumentException>(() => new Rect3D(1.0, 2.0, 3.0, 10.0, -20.0, 30.0));
        }

        [Test]
        public static void Constructor_WithNegativeSizeZ_ThrowsArgumentException()
        {
            // Windows Rect3D throws ArgumentException for negative dimensions
            Assert.Throws<ArgumentException>(() => new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, -30.0));
        }

        [Test]
        public static void Empty_StaticProperty_IsEmpty()
        {
            Rect3D empty = Rect3D.Empty;
            Assert.That(empty.IsEmpty, Is.True);
        }

        [Test]
        public static void OperatorEquals_EqualRects_ReturnsTrue()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            
            Assert.That(r1 == r2, Is.True);
        }

        [Test]
        public static void OperatorNotEquals_DifferentRects_ReturnsTrue()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(5.0, 6.0, 7.0, 15.0, 25.0, 35.0);
            
            Assert.That(r1 != r2, Is.True);
        }

        [Test]
        public static void Equals_SameValues_ReturnsTrue()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            
            Assert.That(r1.Equals(r2), Is.True);
        }

        [Test]
        public static void GetHashCode_EqualRects_ReturnSameHashCode()
        {
            Rect3D r1 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Rect3D r2 = new Rect3D(1.0, 2.0, 3.0, 10.0, 20.0, 30.0);
            Assert.That(r1.GetHashCode(), Is.EqualTo(r2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Rect3D rect = new Rect3D(1.5, 2.5, 3.5, 10.5, 20.5, 30.5);
            // Windows Rect3D only supports ToString() without parameters
            string result = rect.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
            Assert.That(result, Does.Contain("3.5"));
            Assert.That(result, Does.Contain("10.5"));
            Assert.That(result, Does.Contain("20.5"));
            Assert.That(result, Does.Contain("30.5"));
        }
    }
}

