using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.SourceFactories
{
    [TestFixture]
    class StreamSourceFactoryTest
    {
        [Test]
        [TestCase(typeof(MeshGeometry3D), "Mesh.Square.xaml")]
        [TestCase(typeof(IDictionary), "Scene.Cube.json")]
        public void CreateFromStream(Type type, string resourceName)
        {
            
            var stream = GlobalFixture.GetStream(resourceName);
            Assert.NotNull(stream, $"resourceName {resourceName} produced a null stream");
            Func<Stream, object> readFunction = Node.Net.Factory.Factories.SourceFactories.StreamSourceFactory.DefaultReadFunction;
            if (resourceName.Contains(".json")) readFunction = JsonRead;
            var streamSourceFactory = new Node.Net.Factory.Factories.SourceFactories.StreamSourceFactory(readFunction, null);
            Assert.NotNull(streamSourceFactory.Create(type, stream));
        }

        private static object JsonRead(Stream stream)
        {
            var reader = new Internal.JsonReader();
            return reader.Read(stream);
        }
    }
}
