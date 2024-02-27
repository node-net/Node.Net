using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            Assert.That(items2.Count, Is.EqualTo(3), "items2.Count");
        }

        [Test]
        public void Usage_Search()
        {
            System.IO.Stream stream = typeof(ItemsTest)
                .Assembly
                .GetManifestResourceStream(
                "Node.Net.Test.Resources.States.json");

            IDictionary states = new Reader().Read<IDictionary>(stream);

            Items<IDictionary> items = new Items<IDictionary>(states.Collect<IDictionary>());
            Assert.That(items.Search,Is.EqualTo(""));
            Assert.That(items.Count, Is.EqualTo(3205), "items.Count");

            items.Search = "County";
            Assert.That(items.Count, Is.EqualTo(3105), "item.Count, Search=\"County\"");

            items.Search = "State";
            Assert.That(items.Count, Is.EqualTo(50), "item.Count, Search=\"State\"");
            Assert.That(items[0].Get<string>("Name"), Is.EqualTo("Alabama"));

            items.Search = "Jefferson";
            Assert.That(items.Count, Is.EqualTo(28), "item.Count, Search=\"Jefferson\"");
        }

        [Test]
        public void Usage_Sort()
        {
            System.IO.Stream stream = typeof(ItemsTest)
                .Assembly
                .GetManifestResourceStream(
                "Node.Net.Test.Resources.States.json");

            IDictionary states = new Reader().Read<IDictionary>(stream);

            Items<IDictionary> items = new Items<IDictionary>(states.Collect<IDictionary>())
            {
                Search = "State",
                SortFunction = SortByNameDescending
            };
            Assert.That(items.Count, Is.EqualTo(50), "item.Count, Search=\"State\"");
            Assert.That(items[0].Get<string>("Name"),Is.EqualTo("Wyoming"));
        }

        public static IEnumerable<IDictionary> SortByNameDescending(IEnumerable<IDictionary> source)
        {
            IOrderedEnumerable<IDictionary> ordered = source.OrderByDescending(dictionary => dictionary.Get<string>("Name"));
            return ordered;
        }
    }
}