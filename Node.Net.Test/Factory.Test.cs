using NUnit.Framework;
using System.Windows.Media.Media3D;

namespace Node.Net.Test
{
    [TestFixture]
    internal class FactoryTest
    {
        [Test]
        public void ClearCache()
        {
            Factory factory = new Factory();
            
            // Cache and ClearCache are now available on all platforms
            factory.Cache = true;
            factory.ClearCache();
            if (factory.Cache)
            {
                factory.Cache = false;
            }

            Matrix3D matrix = factory.Create<Matrix3D>();
            factory.ClearCache(matrix);
            factory.ClearCache();
            
            Assert.That(matrix, Is.Not.Null);
        }

        [Test]
        public void Coverage()
        {
            Factory factory = new Factory();
            Assert.That(factory, Is.Not.Null);
        }
    }
}