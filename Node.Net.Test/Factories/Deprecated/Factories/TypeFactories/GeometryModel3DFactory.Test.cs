using NUnit.Framework;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeFactories
{
    [TestFixture]
    class GeometryModel3DTest
    {
        [Test, Apartment(ApartmentState.STA)]
        [TestCase("Mesh.Cube.xaml")]
        public void GeometryModel3DFactory_Create(string resourceName)
        {
            var source = GlobalFixture.GetResource(resourceName);
            Assert.IsNotNull(source);
            var factory = new Node.Net.Factory.Factories.TypeFactories.GeometryModel3DFactory();
            Assert.NotNull(factory.Create<GeometryModel3D>(source));
        }
        /*
        [Test]
        [TestCase(typeof(Material), "Blue")]
        public void Factory_CreateFromString(Type targetType, string value)
        {
            var factory = new Factory(typeof(FactoryTest).Assembly) { GetFunction = GetHelper.GetResource };
            var instance = factory.Create(targetType, value);
            Assert.NotNull(instance);
            Assert.True(targetType.IsAssignableFrom(value.GetType()));
        }*/
    }
}
