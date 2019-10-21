using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Collections
{
    [TestFixture]
    internal class ItemsTest
    {
        [Test]
        public void Usage()
        {
            var stream = typeof(JsonReaderTest)
                .Assembly
                .GetManifestResourceStream("Node.Net.Test.Resources.Spatial.json");

            var dictionary = new JsonReader().Read(stream) as IDictionary;
            var items = new Items<IDictionary>(dictionary.Collect<IDictionary>());
            Assert.AreEqual(5, items.Count, "items.Count");

            // Filter
            items.Search = "Foo";
            Assert.AreEqual(2, items.Count, "item.Count, Search=\"Foo\"");
        }
    }
}
