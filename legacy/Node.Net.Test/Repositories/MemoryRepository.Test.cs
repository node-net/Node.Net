using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Repositories
{
    [TestFixture]
    public class MemoryRepositoryTest
    {
        [Test, Apartment(ApartmentState.STA)]
        [TestCase("Node.Net.Repositories.Test.Resources.mesh.xaml", typeof(MeshGeometry3D))]
        public void MemoryRepository_Set_Get(string name, Type expectedType)
        {
            var repository = new AssemblyRepository { Assembly = typeof(AssemblyRepositoryTest).Assembly };
            var instance = repository.Get(name);
            var memoryRepo = new MemoryRepository();
            memoryRepo.Set(name, instance);
            var instance2 = memoryRepo.Get(name);
            Assert.AreSame(expectedType, instance2.GetType());
            var names = new List<string>(memoryRepo.GetNames(null));
            Assert.True(names.Contains(name));
        }
    }
}
