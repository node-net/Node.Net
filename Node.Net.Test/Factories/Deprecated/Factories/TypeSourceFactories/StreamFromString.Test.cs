using NUnit.Framework;
using System.IO;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class StreamFromStringTest
    {
        [Test, Apartment(System.Threading.ApartmentState.STA)]
        [TestCase("Mesh.Cube.xaml")]
        public void StreamFromString(string source)
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.StreamFromString();
            factory.ResourceAssemblies.Add(typeof(StreamFromStringTest).Assembly);
            Assert.IsNotNull(factory.Create<Stream>(source));
        }
    }
}
