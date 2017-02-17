using NUnit.Framework;
using System;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Factories
{
    [TestFixture]
    class ManifestResourceFactoryTest
    {
        [Test, Apartment(ApartmentState.STA)]
        [TestCase(typeof(MeshGeometry3D), "Node.Net.Factory.Test.Resources.Mesh.Cube.xaml")]
        [TestCase(typeof(MeshGeometry3D), "Mesh.Cube.xaml")]
        [TestCase(typeof(Model3D), "Model.Cube.Red")]
        public void Create(Type targetType, string name)
        {
            var factory = new ManifestResourceFactory();
            factory.Assemblies.Add(typeof(ManifestResourceFactoryTest).Assembly);
            Assert.IsNotNull(factory.Create(targetType, name));
        }
    }
}
