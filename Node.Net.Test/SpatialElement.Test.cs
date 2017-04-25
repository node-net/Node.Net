using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Node.Net
{
    [TestFixture]
    class SpatialElementTest
    {
        [Test,Apartment(ApartmentState.STA),Explicit]
        [TestCase("Scene.json")]
        public void SpatialElement_Visual3D(string name)
        {
            var data = Factory.Default.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));

            var factory = new Factory
            {
                ReadFunction = new Reader { DefaultObjectType = typeof(SpatialElement) }.Read
            };
            //factory.I
            //var v3d = Factory.Default.
        }
    }
}
