using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class GeometryModel3DFactoryTest
    {
        [Test]
        [TestCase("Scene.Cube.json")]
        public void GeometryModel3DFactory_From_IDictionary(string name)
        {
            var helper = new StandardFactory();
            helper.ResourceFactory.ImportManifestResources(typeof(GeometryModel3DFactoryTest).Assembly);
            //helper.ManifestResourceFactory.Assemblies.Add(typeof(GeometryModel3DFactoryTest).Assembly);
            //helper.ManifestResourceFactory.ManifestResourceNameIgnorePatterns.Add(".json");
            var mesh = helper.Create(typeof(MeshGeometry3D), "Cube") as MeshGeometry3D;
            Assert.NotNull(mesh, nameof(mesh));

            var model = GlobalFixture.Read(name) as IDictionary;
            Assert.NotNull(model, nameof(model));
            var factory = new GeometryModel3DFactory
            {
                Helper = helper
            };
            var gm3d = factory.Create(typeof(GeometryModel3D), model);
            Assert.NotNull(gm3d,nameof(gm3d));
        }
    }
}
