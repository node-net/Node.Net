using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeFactories
{
    [TestFixture]
    class StreamFactoryTest
    {
        /*
        [Test]
        [TestCase(typeof(MeshGeometry3D), "Mesh.Square.xaml")]
        [TestCase(typeof(IDictionary), "Scene.Cube.json")]
        public void CreateFromResourceName(Type type, string resourceName)
        {
            Func<Stream, object> readFunction = Node.Net.Factory.Factories.SourceFactories.StreamSourceFactory.DefaultReadFunction;
            if (resourceName.Contains(".json")) readFunction = JsonRead;

            var streamFactory = new Node.Net.Factory.Factories.TypeFactories.StreamFactory(typeof(StreamFactoryTest).Assembly);
            var stream = streamFactory.Create<Stream>(resourceName);
            Assert.NotNull(stream, $"resourceName {resourceName} produced a null stream");

            var item = streamFactory.Create(type, resourceName);
            Assert.NotNull(item);
            Assert.True(type.IsAssignableFrom(item.GetType()));
        }*/
        /*
        private static object JsonRead(Stream stream)
        {
            var reader = new Internal.JsonReader();
            return reader.Read(stream);
        }*/
    }
}
