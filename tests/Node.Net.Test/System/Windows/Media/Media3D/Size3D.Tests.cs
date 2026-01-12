extern alias NodeNet;
using NUnit.Framework;
using NodeNet::System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class Size3DTests
    {
        [Test]
        public static void Constructor_WithParameters_SetsProperties()
        {
            Size3D size = new Size3D(1.0, 2.0, 3.0);
            Assert.That(size.X, Is.EqualTo(1.0));
            Assert.That(size.Y, Is.EqualTo(2.0));
            Assert.That(size.Z, Is.EqualTo(3.0));
        }

        [Test]
        public static void Constructor_Default_InitializesToZero()
        {
            Size3D size = new Size3D();
            Assert.That(size.X, Is.EqualTo(0.0));
            Assert.That(size.Y, Is.EqualTo(0.0));
            Assert.That(size.Z, Is.EqualTo(0.0));
        }

        [Test]
        public static void Properties_CanBeSet()
        {
            Size3D size = new Size3D();
            size.X = 5.0;
            size.Y = 6.0;
            size.Z = 7.0;
            Assert.That(size.X, Is.EqualTo(5.0));
            Assert.That(size.Y, Is.EqualTo(6.0));
            Assert.That(size.Z, Is.EqualTo(7.0));
        }

        [Test]
        public static void OperatorEquals_EqualSizes_ReturnsTrue()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(1.0, 2.0, 3.0);
            
            Assert.That(s1 == s2, Is.True);
        }

        [Test]
        public static void OperatorNotEquals_DifferentSizes_ReturnsTrue()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(3.0, 4.0, 5.0);
            
            Assert.That(s1 != s2, Is.True);
        }

        [Test]
        public static void Equals_SameValues_ReturnsTrue()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(1.0, 2.0, 3.0);
            
            Assert.That(s1.Equals(s2), Is.True);
        }

        [Test]
        public static void GetHashCode_EqualSizes_ReturnSameHashCode()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(1.0, 2.0, 3.0);
            Assert.That(s1.GetHashCode(), Is.EqualTo(s2.GetHashCode()));
        }

        [Test]
        public static void ToString_ReturnsFormattedString()
        {
            Size3D size = new Size3D(1.5, 2.5, 3.5);
            // Windows Size3D only supports ToString() without parameters
            string result = size.ToString();
            Assert.That(result, Does.Contain("1.5"));
            Assert.That(result, Does.Contain("2.5"));
            Assert.That(result, Does.Contain("3.5"));
        }
    }
}

