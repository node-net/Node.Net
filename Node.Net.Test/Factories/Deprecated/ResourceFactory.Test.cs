using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Test
{
    [TestFixture]
    class ResourceFactoryTest
    {
        [Test]
        public void ResourceFactory_Usage()
        {
            var resourceFactory = new ResourceFactory { Assembly = typeof(ResourceFactoryTest).Assembly };
            var mesh = resourceFactory.Create<MeshGeometry3D>("Mesh.Square", null);
            Assert.NotNull(mesh, nameof(mesh));
        }
    }
}
