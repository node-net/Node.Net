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
    class ITranslationFromIDictionaryTest
    {
        [Test]
        [TestCase(1, 1, 1)]
        [TestCase(10, 20, 30)]
        public void ITranslationFromIDictionary_Usage(double x, double y, double z)
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ITranslationFromIDictionary();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = $"{x} m";
            dictionary["Y"] = $"{y} m";
            dictionary["Z"] = $"{z} m";

            var translation = factory.Create<ITranslation>(dictionary).Translation;
            Assert.AreEqual(x, translation.X);
            Assert.AreEqual(y, translation.Y);
            Assert.AreEqual(z, translation.Z);

        }

        [Test]
        public void ITranslationFromIDictionary_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ITranslationFromIDictionary();
            var translation = factory.Create<ITranslation>(null).Translation;
            Assert.AreEqual(0, translation.X);
            Assert.AreEqual(0, translation.Y);
            Assert.AreEqual(0, translation.Z);
        }
    }
}
