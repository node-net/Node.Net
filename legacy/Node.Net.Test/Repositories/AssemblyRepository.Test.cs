using NUnit.Framework;
using System;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Repositories
{
    [TestFixture]
    class AssemblyRepositoryTest
    {
        [Test, Apartment(ApartmentState.STA)]
        [TestCase("Node.Net.Repositories.Test.Resources.mesh.xaml", typeof(MeshGeometry3D))]
        public void AssemblyRepository_Get(string name, Type expectedType)
        {
            var repository = new AssemblyRepository { Assembly = typeof(AssemblyRepositoryTest).Assembly };
            var instance = repository.Get(name);
            Assert.NotNull(instance, nameof(instance));
            Assert.AreSame(expectedType, instance.GetType());
        }
    }
}
