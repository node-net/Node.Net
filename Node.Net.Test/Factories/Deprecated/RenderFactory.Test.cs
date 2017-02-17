using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Test
{
    [TestFixture]
    class RenderFactoryTest
    {
        [Test]
        public void RenderFactory_Usage()
        {
            var renderFactory = new Node.Net.Factories.Deprecated.RenderFactory();
            renderFactory.ResourceFactories.Add("Node.Net.Factories.Test", new ResourceFactory { Assembly = typeof(RenderFactoryTest).Assembly });

            var scene = new Dictionary<string, dynamic>();
            scene["Type"] = "Teapot";

            var v3d = renderFactory.Create<Visual3D>(scene, null);
            //Assert.NotNull(v3d, nameof(v3d));
        }
    }
}
