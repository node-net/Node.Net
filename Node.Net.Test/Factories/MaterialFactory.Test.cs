using NUnit.Framework;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    public class MaterialFactoryTest
    {
        [Test]
        public void MaterialFactory_Usage()
        {
            var factory = new MaterialFactory();
            Assert.IsNull(factory.Create(null), "default Material should be null");
            factory.DefaultMaterial = new DiffuseMaterial(Brushes.Blue);
            Assert.NotNull(factory.Create(null), "default material should not be null");
            factory.DefaultMaterial = null;
            Assert.NotNull(factory.Create(Brushes.Blue), "Brushes.Blue");

            var brushFactory = new BrushFactory
            {
                Helper = new ColorFactory()
            };
            factory.Helper = brushFactory;
            Assert.NotNull(factory.Create("Blue"),"create 'Blue'");
            Assert.NotNull(factory.Create("255,0,0"),"create '255,0,0'");
        }

        [Test]
        [TestCase("Blue", 255, 0, 0, 255)]
        [TestCase("Red", 255, 255, 0, 0)]
        [TestCase("Green", 255, 0, 128, 0)]
        [TestCase("128,15,55", 255, 128, 15, 55)]
        [TestCase("128,100,130,140", 128, 100, 130, 140)]
        public void MaterialFactory_From_Color_Usage(string name, byte A, byte R, byte G, byte B)
        {
            var factory = new MaterialFactory();
            var material = factory.Create(typeof(Material), name) as Material;

            var diffuseMaterial = material as DiffuseMaterial;
            var scb = diffuseMaterial.Brush as SolidColorBrush;// brush as SolidColorBrush;
            Assert.AreEqual(A, scb.Color.A, "color.A");
            Assert.AreEqual(R, scb.Color.R, "color.R");
            Assert.AreEqual(G, scb.Color.G, "color.G");
            Assert.AreEqual(B, scb.Color.B, "color.B");
        }

        [TestCase("Blue")]
        public void MaterialFactory_FromString(string name)
        {
            var factory = new MaterialFactory();
            var material = factory.Create(typeof(Material), name);
            Assert.NotNull(material, nameof(material));
        }
    }
}
