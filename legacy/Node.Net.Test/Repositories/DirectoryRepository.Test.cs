using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Repositories
{
    [TestFixture]
    public class DirectoryRepositoryTest
    {
        [Test,Apartment(ApartmentState.STA)]
        [TestCase("Node.Net.Repositories.Test.Resources.mesh.xaml", typeof(MeshGeometry3D))]
        public void DirectoryRepository_Set_Get(string name, Type expectedType)
        {
            var repository = new AssemblyRepository { Assembly = typeof(AssemblyRepositoryTest).Assembly };
            var instance = repository.Get(name);
            var directoryRepo = new DirectoryRepository { Directory = Path.GetTempPath() };
            directoryRepo.Set(name, instance);
            var instance2 = directoryRepo.Get(name);
            Assert.AreSame(expectedType, instance2.GetType());
        }
    }
}
