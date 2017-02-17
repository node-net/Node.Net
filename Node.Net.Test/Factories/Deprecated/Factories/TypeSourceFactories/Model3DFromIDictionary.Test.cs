using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class Model3DFromIDictionaryTest
    {
        [Test]
        public void Model3DFromIDictionary_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.Model3DFromIDictionary();
            Assert.IsNull(factory.Create<Model3D>(null));
        }

        [Test]
        public void Model3DFromIDictionary()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Type"] = "Cube";

            var factory = new Node.Net.Factory.Factories.CompositeFactory();
            var modelFactory = new Node.Net.Factory.Factories.TypeSourceFactories.Model3DFromIDictionary();
            factory.Add(nameof(Model3DFromIDictionary), modelFactory);
            factory.Add("IPrimaryModel3D", new Node.Net.Factory.Factories.TypeSourceFactories.IPrimaryModelFromIDictionary());
            factory.Add("ObjectFromString", new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromString { GetFunction = GetHelper.GetResource });
            Assert.AreSame(factory, modelFactory.GetRootAncestor());

            var cube = factory.Create<Model3D>("Cube");
            Assert.NotNull(cube,nameof(cube));

            var primaryModel = factory.Create<IPrimaryModel>(dictionary);
            Assert.NotNull(primaryModel, nameof(primaryModel));

            var model3D = factory.Create<Model3D>(dictionary);
            Assert.NotNull(model3D,nameof(model3D));
        }


    }
}
