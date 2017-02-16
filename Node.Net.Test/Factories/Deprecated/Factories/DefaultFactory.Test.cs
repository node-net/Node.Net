using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Factory.Factories
{
    [TestFixture, Category(nameof(DefaultFactory))]
    class DefaultFactoryTest
    {
        [TestCase(typeof(Material))]
        public void IsNull(Type type)
        {
            var factory = new DefaultFactory();
            Assert.IsNull(factory.Create(type, null));
        }
        [TestCase(typeof(Color))]
        public void Factory_IsNotNull(Type type)
        {
            var factory = new DefaultFactory();
            Assert.IsNotNull(factory.Create(type, null));
        }

        [Test, Category(nameof(ILength))]
        [TestCase(0, null)]
        [TestCase(10, 10)]
        [TestCase(1.23, 1.23)]
        [TestCase(49, "49")]
        [TestCase(1, "1m")]
        [TestCase(1.7E+308, "1.7E+308")]
        [TestCase(1, "3.28084 feet")]
        [TestCase(1, "3.28084 ft")]
        [TestCase(1, "3.28084 '")]
        [TestCase(1, "39.3701 inch")]
        [TestCase(1, "39.3701 \"")]
        [TestCase(1, "1 meter")]
        [TestCase(1, "100 cm")]
        [TestCase(1, "1000 mm")]
        public void Create_ILength(double value, object source)
        {
            var factory = new DefaultFactory();
            Assert.AreEqual(value, factory.Create<ILength>(source).Length);
        }
        [Test, Category(nameof(Color))]
        [TestCase(255, 240, 255, 255, "Azure")]
        [TestCase(255, 0, 0, 241, "0,0,241")]
        [TestCase(10, 20, 30, 40, "10,20,30,40")]
        public void Create_Color(int a, int r, int g, int b, object source)
        {
            var factory = new DefaultFactory();
            Assert.AreEqual(a, factory.Create<Color>(source).A, "color.A");
            Assert.AreEqual(r, factory.Create<Color>(source).R, "color.R");
            Assert.AreEqual(g, factory.Create<Color>(source).G, "color.G");
            Assert.AreEqual(b, factory.Create<Color>(source).B, "color.B");
        }
        [Test, Category(nameof(IAngle))]
        [TestCase(0, null)]
        [TestCase(10, 10)]
        [TestCase(1, "1 deg")]
        [TestCase(1, "1 degree")]
        [TestCase(1, "1 degrees")]
        [TestCase(1, "0.0174533 radians")]
        [TestCase(1, "0.0174533 rad")]
        public void Create_IAngle(double angle, object source)
        {
            var factory = new DefaultFactory();
            Assert.AreEqual(angle, factory.Create<IAngle>(source).Angle);
        }
        [Test,Category(nameof(ITranslation))]
        public void Create_ITranslation()
        {
            var factory = new DefaultFactory();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";
            dictionary["Y"] = "20 m";
            dictionary["Z"] = "30 m";
            Assert.AreEqual(10, factory.Create<ITranslation>(dictionary).Translation.X);
            Assert.AreEqual(20, factory.Create<ITranslation>(dictionary).Translation.Y);
            Assert.AreEqual(30, factory.Create<ITranslation>(dictionary).Translation.Z);
        }

        
    }
}
