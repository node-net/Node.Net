using NUnit.Framework;
using System.Windows.Media;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class BrushFactoryTest
    {
        [Test]
        public void BrushFactory_Usage()
        {
            var factory = new BrushFactory();
            Assert.AreSame(Brushes.Red, factory.Create("Red"));
            Assert.AreEqual(Brushes.Blue, factory.Create("Blue"));
            Assert.AreEqual(Brushes.Green, factory.Create("Green"));

            factory.Helper = new ColorFactory();
            var brush = factory.Create("255,0,0") as SolidColorBrush;
            //Assert.NotNull(brush, nameof(brush));
        }

        [Test]
        [TestCase("Blue", 255, 0, 0, 255)]
        [TestCase("Red", 255, 255, 0, 0)]
        [TestCase("Green", 255, 0, 128, 0)]
        [TestCase("128,15,55", 255, 128, 15, 55)]
        [TestCase("128,100,130,140", 128, 100, 130, 140)]
        public void BrushFactory_From_Color_Usage(string name, byte A, byte R, byte G, byte B)
        {
            var factory = new BrushFactory();
            var brush = factory.Create(typeof(Brush), name) as Brush;

            var scb = brush as SolidColorBrush;
            Assert.AreEqual(A, scb.Color.A, "color.A");
            Assert.AreEqual(R, scb.Color.R, "color.R");
            Assert.AreEqual(G, scb.Color.G, "color.G");
            Assert.AreEqual(B, scb.Color.B, "color.B");
        }
    }
}
