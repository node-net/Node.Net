using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class IScaleFromIDictionaryTest
    {
        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(10, 20, 30)]
        public void IScaleFromIDictionary_Usage(double scaleX, double scaleY, double scaleZ)
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.IScaleFromIDictionary();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["ScaleX"] = $"{scaleX} m";
            dictionary["ScaleY"] = $"{scaleY} m";
            dictionary["ScaleZ"] = $"{scaleZ} m";

            var scale = factory.Create<IScale>(dictionary).Scale;
            Assert.AreEqual(scaleX, scale.X);
            Assert.AreEqual(scaleY, scale.Y);
            Assert.AreEqual(scaleZ, scale.Z);

        }

        [Test]
        public void IScaleFromIDictionary_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.IScaleFromIDictionary();
            var scale = factory.Create<IScale>(null).Scale;
            Assert.AreEqual(1, scale.X);
            Assert.AreEqual(1, scale.Y);
            Assert.AreEqual(1, scale.Z);
        }
    }
}
