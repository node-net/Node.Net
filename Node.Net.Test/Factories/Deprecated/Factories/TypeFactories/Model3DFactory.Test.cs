using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeFactories
{
    class Model3DFactoryTest
    {
        [Test, Apartment(ApartmentState.STA)]
        [TestCase("Mesh.Cube.xaml")]
        public void Model3DFactory_Create(string resourceName)
        {
            var source = GlobalFixture.GetResource(resourceName);
            Assert.IsNotNull(source);
            var factory = new Node.Net.Factory.Factories.TypeFactories.Model3DFactory();
            Assert.NotNull(factory.Create<Model3D>(source));
        }
    }
}
