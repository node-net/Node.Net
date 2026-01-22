using NUnit.Framework;
using Node.Net;

namespace Node.Net.Test
{
    [TestFixture]
    internal class FactoryTest
    {
        [Test]
        public void ClearCache()
        {
            Factory factory = new Factory();
            factory.Cache = true;
            factory.ClearCache();
            if (factory.Cache) factory.Cache = false;

            Matrix3D matrix = factory.Create<Matrix3D>();
            factory.ClearCache(matrix);
            factory.ClearCache();
        }

        [Test]
        public void Coverage()
        {
            Factory factory = new Factory();
        }
    }
}