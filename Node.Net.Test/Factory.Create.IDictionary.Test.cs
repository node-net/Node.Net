using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
        public void CreateFromManifestResourceStream(string name)
        {
            var data = factory.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
        }
    }
}
