using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class IPrimaryModelFromIDictionaryTest
    {
        [Test]
        public void IPrimaryModelFromIDictionary()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Type"] = "Cube";

            var factory = new Node.Net.Factory.Factories.CompositeFactory();
            var modelFactory = new Node.Net.Factory.Factories.TypeSourceFactories.IPrimaryModelFromIDictionary();
            factory.Add(nameof(IPrimaryModelFromIDictionary), modelFactory);
            factory.Add("ObjectFromString", new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromString { GetFunction = GetFunction });
            Assert.AreSame(factory, modelFactory.GetRootAncestor());

            var cube = factory.Create<Model3D>("Cube");
            Assert.NotNull(cube);
            var iprimaryModel = factory.Create<IPrimaryModel>(dictionary);
            Assert.NotNull(iprimaryModel);
            Assert.NotNull(iprimaryModel.Model3D);
            //Assert.NotNull(factory.Create<Model3D>(dictionary));
        }

        public static object GetFunction(string name)
        {
            if (name == "Cube")
            {
                var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
                meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                return new GeometryModel3D
                {
                    Geometry = meshBuilder.ToMesh(),
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue),
                    BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Gray)
                };
            }
            return null;
        }
    }
}
