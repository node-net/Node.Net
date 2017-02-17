using NUnit.Framework;

namespace Node.Net.Controls
{
    [TestFixture]
    public class FactoryTest
    {
        [Test]
        public void Factory_Usage()
        {
            var factory = Factory.Default;
            Assert.IsNotNull(factory.Create<object>(null));
        }
    }
}
