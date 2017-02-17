using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Node.Net.Factories.Deprecated;

namespace Node.Net.Tests.Factory
{
    class FactoryTest
    {
        [Test]
        [TestCase(typeof(Node.Net.Factories.Deprecated.IColor), "Blue")]
        [TestCase(typeof(Material), "Blue")]
        public void Factory_Default(Type type, object source)
        {
            Node.Net.Factories.Deprecated.IFactory factory = Node.Net.Factories.Deprecated.Factory.Default.Create<Node.Net.Factories.Deprecated.IFactory>(null,null);
            Assert.IsNotNull(factory);

            var instance = factory.Create(type, source,null);
            Assert.NotNull(instance);
            Assert.True(type.IsAssignableFrom(instance.GetType()));
        }

        [Test]
        [TestCase("Scene.Cubes.json")]
        public void Factory_Rendering(string scene_name)
        {

        }
    }
}
