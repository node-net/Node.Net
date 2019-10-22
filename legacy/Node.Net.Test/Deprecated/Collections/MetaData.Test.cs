using NUnit.Framework;
using System.Collections.Generic;

namespace Node.Net.Deprecated.Collections
{
    [TestFixture, Category(nameof(Collections))]
    class MetaDataTest
    {
        [Test]
        public void MetaData_Usage()
        {
            var widget = new Dictionary<string, dynamic>();

            var meta = new MetaDataManager();
            meta.SetMetaData(widget, "test", "a");
            Assert.AreEqual("a", meta.GetMetaData(widget, "test"));
        }
    }
}
