using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture]
    class FactoryCreateVisual3DTest
    {
        [Test]
        public void SceneCubes()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var m3d = factory.Create<Model3D>("Scene.Cubes.json");
            Assert.NotNull(m3d, nameof(m3d), "from 'Scene.Cubes.json'");

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d),"from IDictionary");

            

            var v3d = factory.Create<Visual3D>(scene);
            Assert.NotNull(v3d, nameof(v3d));

            v3d = factory.Create<Visual3D>("Scene.Cubes.json");
            Assert.NotNull(v3d, nameof(v3d));
        }

        [Test]
        public void Scene()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.json");
            Assert.NotNull(scene, nameof(scene));

            var m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d), "from IDictionary");

            m3d = factory.Create<Model3D>("Scene.json");
            Assert.NotNull(m3d, nameof(m3d), "from 'Scene.json'");

            

            

            var v3d = factory.Create<Visual3D>(scene);
            Assert.NotNull(v3d, nameof(v3d));

            v3d = factory.Create<Visual3D>("Scene.json");
            Assert.NotNull(v3d, nameof(v3d));
        }
    }
}
