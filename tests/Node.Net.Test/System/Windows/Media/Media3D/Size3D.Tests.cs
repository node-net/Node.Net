using System.Threading.Tasks;
using static System.Math;

namespace Node.Net.Test
{
    internal static class Size3DTests
    {
        [Test]
        public static async Task Constructor_WithParameters_SetsProperties()
        {
            Size3D size = new Size3D(1.0, 2.0, 3.0);
            await Assert.That(size.X).IsEqualTo(1.0);
            await Assert.That(size.Y).IsEqualTo(2.0);
            await Assert.That(size.Z).IsEqualTo(3.0);
        }

        [Test]
        public static async Task Constructor_Default_InitializesToZero()
        {
            Size3D size = new Size3D();
            await Assert.That(size.X).IsEqualTo(0.0);
            await Assert.That(size.Y).IsEqualTo(0.0);
            await Assert.That(size.Z).IsEqualTo(0.0);
        }

        [Test]
        public static async Task Properties_CanBeSet()
        {
            Size3D size = new Size3D();
            size.X = 5.0;
            size.Y = 6.0;
            size.Z = 7.0;
            await Assert.That(size.X).IsEqualTo(5.0);
            await Assert.That(size.Y).IsEqualTo(6.0);
            await Assert.That(size.Z).IsEqualTo(7.0);
        }

        [Test]
        public static async Task OperatorEquals_EqualSizes_ReturnsTrue()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(1.0, 2.0, 3.0);
            
            await Assert.That(s1 == s2).IsTrue();
        }

        [Test]
        public static async Task OperatorNotEquals_DifferentSizes_ReturnsTrue()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(3.0, 4.0, 5.0);
            
            await Assert.That(s1 != s2).IsTrue();
        }

        [Test]
        public static async Task Equals_SameValues_ReturnsTrue()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(1.0, 2.0, 3.0);
            
            await Assert.That(s1.Equals(s2)).IsTrue();
        }

        [Test]
        public static async Task GetHashCode_EqualSizes_ReturnSameHashCode()
        {
            Size3D s1 = new Size3D(1.0, 2.0, 3.0);
            Size3D s2 = new Size3D(1.0, 2.0, 3.0);
            await Assert.That(s1.GetHashCode()).IsEqualTo(s2.GetHashCode());
        }

        [Test]
        public static async Task ToString_ReturnsFormattedString()
        {
            Size3D size = new Size3D(1.5, 2.5, 3.5);
            // Windows Size3D only supports ToString() without parameters
            string result = size.ToString();
            await Assert.That(result.Contains("1.5")).IsTrue();
            await Assert.That(result.Contains("2.5")).IsTrue();
            await Assert.That(result.Contains("3.5")).IsTrue();
        }
    }
}

