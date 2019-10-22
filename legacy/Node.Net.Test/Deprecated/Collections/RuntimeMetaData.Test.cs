using NUnit.Framework;
using System;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Collections
{
    [TestFixture, Category("Collections.RuntimeMetaData")]
    class RuntimeMetaDataTest
    {
        [Test]
        public void RuntimeMetaData_Usage()
        {
            var md = new RuntimeMetaData();
            Assert.AreEqual(0, md.Count);

            {
                var point = new Point3D();
                md.Set(point, "Test", "A");
                Assert.AreEqual(1, md.Count);
                Assert.AreEqual("A", md.Get(point, "Test"));
                GC.Collect();
            }
            md.Clean();
            Assert.AreEqual(0, md.Count);
        }
    }
}
