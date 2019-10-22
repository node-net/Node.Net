using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class Visual3DFromIDictionaryTest
    {
        [Test]
        public void Visual3DFromIDictionary()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["Type"] = "Cube";

            var factory = new Node.Net.Factory.Factories.CompositeFactory();
            var visualFactory = new Node.Net.Factory.Factories.TypeSourceFactories.Visual3DFromIDictionary();
            var modelFactory = new Node.Net.Factory.Factories.TypeSourceFactories.Model3DFromIDictionary();
            factory.Add(nameof(Visual3DFromIDictionary), visualFactory);
            factory.Add("Model3DFromIDictionary", modelFactory);
            factory.Add("IPrimaryModel3D", new Node.Net.Factory.Factories.TypeSourceFactories.IPrimaryModelFromIDictionary());
            factory.Add("ObjectFromString", new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromString { GetFunction = GetFunction });
            Assert.AreSame(factory, visualFactory.GetRootAncestor());

            var cube = factory.Create<Model3D>("Cube");
            Assert.NotNull(cube, nameof(cube));

            var primaryModel = factory.Create<IPrimaryModel>(dictionary);
            Assert.NotNull(primaryModel, nameof(primaryModel));

            var model3D = factory.Create<Model3D>(dictionary);
            Assert.NotNull(model3D, nameof(model3D));

            var visual3D = factory.Create<Visual3D>(dictionary);
            Assert.NotNull(visual3D, nameof(Visual3D));
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
