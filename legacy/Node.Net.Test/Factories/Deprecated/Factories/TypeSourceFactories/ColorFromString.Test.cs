using NUnit.Framework;
using System.Windows.Media;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class ColorFromStringTest
    {
        [Test]
        [TestCase("Blue", 255, 0, 0, 255)]
        [TestCase("Green", 255, 0, 128, 0)]
        public void ColorFromString_Usage(string source, byte A, byte R, byte G, byte B)
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ColorFromString();
            Assert.AreEqual(A, factory.Create(source).A, nameof(A));
            Assert.AreEqual(R, factory.Create(source).R, nameof(R));
            Assert.AreEqual(G, factory.Create(source).G, nameof(G));
            Assert.AreEqual(B, factory.Create(source).B, nameof(B));
        }

        [Test]
        public void ColorFromString_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ColorFromString();
            //Assert.AreEqual(factory.DefaultColor, factory.Create<Color>(null));
            //Assert.AreEqual(factory.DefaultColor, factory.Create<Color>("?"));
        }
    }
}
