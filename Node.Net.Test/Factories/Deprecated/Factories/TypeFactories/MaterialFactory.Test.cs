using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeFactories
{
    [TestFixture]
    class MaterialFactoryTest
    {
        [Test]
        public void MaterialFactory_Usage()
        {
            var factory = new Node.Net.Factory.Factories.TypeFactories.MaterialFactory();
            Assert.AreSame(typeof(Material), factory.TargetType);
            //Assert.IsNotNull(factory.Create<Material>(Colors.Blue));
        }

        [Test]
        [TestCase("Blue")]
        public void MaterialFactory_CreateFromString(string source)
        {
            var factory = new Node.Net.Factory.Factories.TypeFactories.MaterialFactory();
            var instance = factory.Create<Material>(source);
            Assert.NotNull(instance);
        }

        [Test]
        public void MaterialFactory_CreateFromDictionary()
        {
            var factory = new Node.Net.Factory.Factories.TypeFactories.MaterialFactory();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Material"] = "Blue";
            var instance = factory.Create<Material>(dictionary);
            Assert.IsNotNull(instance);
        }
        /*

        [Test]
        public void Create_Material()
        {
            var factory = new Factories.DefaultFactory();
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Material"] = "Blue";
            Assert.IsNotNull(factory.Create<Material>(dictionary));
        }*/
    }
}
