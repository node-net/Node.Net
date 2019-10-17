using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    internal class MetaDataTest
    {
        [Test]
        public void Clean()
        {
            var a = new Dictionary<string, object>();
            var meta = new MetaData();
            meta.SetMetaData(a, "Test", 0);
            meta.ClearMetaData(a);
            meta.SetMetaData(a, "Test", 11);
            Assert.True(meta.HasMetaData(a));
            meta.Clean();
        }
    }
}
