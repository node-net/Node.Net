using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Node.Net.Collections
{
    [TestFixture]
    internal class ItemsTest
    {
        [Test]
        public void JsonSerialization()
        {
            Items<string> items = new Items<string>(new string[] { "red", "green", "blue" });
            Items<string> items2 = items.Clone() as Items<string>;
            Assert.AreEqual(3, items2.Count, "items2.Count");
        }

        [Test]
        public void Usage_Search()
        {
            System.IO.Stream stream = typeof(ItemsTest)
                .Assembly
                .GetManifestResourceStream(
                "Node.Net.Test.Resources.States.json");
            Assert.NotNull(stream, nameof(stream));

            IDictionary states = new Reader().Read<IDictionary>(stream);

            Items<IDictionary> items = new Items<IDictionary>(states.Collect<IDictionary>());
            Assert.AreEqual("", items.Search);
            Assert.AreEqual(3205, items.Count, "items.Count");

            items.Search = "County";
            Assert.AreEqual(3105, items.Count, "item.Count, Search=\"County\"");

            items.Search = "State";
            Assert.AreEqual(50, items.Count, "item.Count, Search=\"State\"");
            Assert.AreEqual("Alabama", items[0].Get<string>("Name"));

            items.Search = "Jefferson";
            Assert.AreEqual(28, items.Count, "item.Count, Search=\"Jefferson\"");
        }

        [Test]
        public void Usage_Sort()
        {
            System.IO.Stream stream = typeof(ItemsTest)
                .Assembly
                .GetManifestResourceStream(
                "Node.Net.Test.Resources.States.json");
            Assert.NotNull(stream, nameof(stream));

            IDictionary states = new Reader().Read<IDictionary>(stream);

            Items<IDictionary> items = new Items<IDictionary>(states.Collect<IDictionary>())
            {
                Search = "State",
                SortFunction = SortByNameDescending
            };
            Assert.AreEqual(50, items.Count, "item.Count, Search=\"State\"");
            Assert.AreEqual("Wyoming", items[0].Get<string>("Name"));
        }

        public static IEnumerable<IDictionary> SortByNameDescending(IEnumerable<IDictionary> source)
        {
            IOrderedEnumerable<IDictionary> ordered = source.OrderByDescending(dictionary => dictionary.Get<string>("Name"));
            return ordered;
        }
    }
}