using NUnit.Framework;

namespace Node.Net.Factory.Test.Factories.TypeFactories
{
    [TestFixture]
    class ILengthFactoryTest
    {
        [Test]
        [TestCase(null, 0)]
        [TestCase(2, 2)]
        [TestCase("3 m", 3)]
        [TestCase("3.28084 feet", 1)]
        public void ILength(object source, double targetValue)
        {
            var factory = new Node.Net.Factory.Factories.TypeFactories.ILengthFactory();
            Assert.AreEqual(targetValue, factory.Create<ILength>(source).Length);
        }
    }
}
