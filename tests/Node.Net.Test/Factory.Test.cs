using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Test
{
    internal class FactoryTest
    {
        [Test]
        public async Task ClearCache()
        {
            Factory factory = new Factory();
            factory.Cache = true;
            factory.ClearCache();
            if (factory.Cache) factory.Cache = false;

            Matrix3D matrix = factory.Create<Matrix3D>();
            factory.ClearCache(matrix);
            factory.ClearCache();
            await Task.CompletedTask;
        }

        [Test]
        public async Task Coverage()
        {
            Factory factory = new Factory();
            await Task.CompletedTask;
        }
    }
}