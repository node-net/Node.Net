using NUnit.Framework;
using System.Collections;

namespace Node.Net
{
    [TestFixture]
    public class FactoryCreateIDictionaryTest
    {
        private Factory factory;
        [SetUp]
        public void SetUp()
        {
            factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryCreateIDictionaryTest).Assembly);
        }
        [Test]
        [TestCase("Scene.500.json")]
        [TestCase("Scene.12500.json")]
        [TestCase("Scene.24500.json")]
        [TestCase("Scene.50000.json")]
        public void CreateFromManifestResourceStream(string name)
        {
            var data = factory.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
        }

        [Test]
        public void CreateFromJSONString()
        {
            var data = factory.Create<IDictionary>("{}");
            Assert.NotNull(data, nameof(data));
        }
    }
}
