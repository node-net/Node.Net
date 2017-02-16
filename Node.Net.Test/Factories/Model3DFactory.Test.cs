using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Factories.Test
{
    [TestFixture]
    class Model3DFactoryTest
    {
        [Test]
        public void Model3DFactory_Usage()
        {
            var factory = new Model3DFactory();
            Assert.IsNull(factory.Create(null));


            //factory.Add("Cube", XamlReader.Load(typeof(MeshGeometry3DFactoryTest).Assembly.GetManifestResourceStream("Node.Net.Factories.Test.Resources.MeshGeometry3D.Cube.xaml")) as MeshGeometry3D);
            //Assert.IsNotNull(factory.Create("Cube"));
        }
    }
}
