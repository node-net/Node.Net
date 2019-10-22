using NUnit.Framework;
using System.Windows.Media;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class ColorFactoryTest
    {
        [Test]
        [TestCase("Blue", 255, 0, 0, 255)]
        [TestCase("Red", 255, 255, 0, 0)]
        [TestCase("Green", 255, 0, 128, 0)]
        [TestCase("128,15,55", 255, 128, 15, 55)]
        [TestCase("128,100,130,140", 128, 100, 130, 140)]
        public void ColorFactory_Usage(string name, byte A, byte R, byte G, byte B)
        {
            var factory = new ColorFactory();
            var color = (Color)factory.Create(typeof(Color), name);
            Assert.AreEqual(A, color.A, "color.A");
            Assert.AreEqual(R, color.R, "color.R");
            Assert.AreEqual(G, color.G, "color.G");
            Assert.AreEqual(B, color.B, "color.B");
        }
    }
}
