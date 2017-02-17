using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    public class ManifestResourceFactoryTest
    {
        [Test]
        public void ManifestResourceFactory_Usage()
        {
            var factory = new ManifestResourceFactory { RequireExactMatch = true };
            factory.ManifestResourceNameIgnorePatterns.Add(".json");
            Assert.IsNull(factory.Create(typeof(MeshGeometry3D), "MeshGeometry3D.Cube.xaml"));

            factory.Assemblies.Add(typeof(ManifestResourceFactoryTest).Assembly);
            Assert.IsNull(factory.Create(typeof(MeshGeometry3D), "MeshGeometry3D.Cube.xaml"));

            factory.RequireExactMatch = false;
            factory.Assemblies.Add(typeof(ManifestResourceFactoryTest).Assembly);
            Assert.IsNotNull(factory.Create(typeof(MeshGeometry3D), "MeshGeometry3D.Cube.xaml"));

            var mesh = factory.Create(typeof(MeshGeometry3D), "Cube") as MeshGeometry3D;
            Assert.NotNull(mesh, nameof(mesh));
        }
    }
}
