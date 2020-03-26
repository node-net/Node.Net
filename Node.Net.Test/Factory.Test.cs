using NUnit.Framework;
using System.Windows.Media.Media3D;

namespace Node.Net.Test
{
    [TestFixture]
    internal class FactoryTest
    {
        [Test]
        public void Create()
        {
            Factory factory = new Factory();
            Assert.NotNull(factory.Create<Matrix3D>());
            //Assert.NotNull(factory.Create(typeof(Matrix3D),null));
        }

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