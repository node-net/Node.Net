using NUnit.Framework;
using System.Windows.Media;

namespace Node.Net.Factories.Deprecated.Test.Factories
{
    [TestFixture]
    class CompositeFactoryTest
    {
        [Test]
        public void CompositeFactory_Usage()
        {
            var factory = new Node.Net.Factories.Deprecated.Factories.CompositeFactory();
            Assert.IsNull(factory.Create<object>(null, null));

            /*
            factory.Add("Color", new Node.Net.Factories.Factories.TypeSourceFactories.ColorFromString());
            Assert.AreEqual(Colors.Blue, factory.Create<Color>("Blue"));

            factory.Add("Length", new Node.Net.Factories.Factories.TypeFactories.ILengthFactory());
            Assert.AreEqual(2, factory.Create<ILength>("2 m").Length);*/
        }
    }
}
