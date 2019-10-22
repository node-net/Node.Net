using NUnit.Framework;
using System.IO;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class ObjectFromStreamTest
    {
        [Test]
        public void ObjectFromStream_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromStream();
            Assert.IsNull(factory.Create<object>(null));
        }

        [Test]
        public void ObjectFromStream_BuiltInReadFunction()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromStream();
            var stream = GlobalFixture.GetStream("Mesh.Cube.xaml");
            var instance = factory.Create<object>(stream);
            Assert.NotNull(instance);
        }

        [Test]
        public void ObjectFromStream_CustomReadFunction()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromStream
            {
                ReadFunction = ReadJson
            };
            var stream = GlobalFixture.GetStream("Scene.Cube.json");
            var instance = factory.Create<object>(stream);
            Assert.NotNull(instance);
        }
        public static object ReadJson(Stream stream)
        {
            var reader = new Node.Net.Factory.Test.Internal.JsonReader();
            return reader.Read(stream);
        }

    }
}
