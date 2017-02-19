using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Deprecated
{
    [TestFixture]
    class GlobalPropertiesTest
    {
        [Test]
        public void GlobalProperties_PerspectiveCamera()
        {
            Assert.NotNull(GlobalProperties.PerspectiveCameras);
        }
    }
}
