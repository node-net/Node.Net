using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Node.Net;
using Node.Net.Collections;

namespace Node.Net.Collections
{
    internal class ItemsTest
    {
        [Test]
        public async Task JsonSerialization()
        {
            Node.Net.Collections.Items<string> items = new Node.Net.Collections.Items<string>(new string[] { "red", "green", "blue" });
            //Items<string> items2 = items.Clone() as Items<string>;
            //Assert.That(items2.Count, Is.EqualTo(3), "items2.Count");
            await Task.CompletedTask;
        }

        [Test]
        public async Task Usage_Search()
        {
            System.IO.Stream stream = typeof(ItemsTest)
                .Assembly
                .GetManifestResourceStream(
                "Node.Net.Test.Resources.States.json");

            IDictionary states = new Reader().Read<IDictionary>(stream);

            Node.Net.Collections.Items<IDictionary> items = new Node.Net.Collections.Items<IDictionary>(states.Collect<IDictionary>());
            await Assert.That(items.Search).IsEqualTo("");
            await Assert.That(items.Count).IsEqualTo(3205);

            items.Search = "County";
            await Assert.That(items.Count).IsEqualTo(3105);

            items.Search = "State";
            await Assert.That(items.Count).IsEqualTo(50);
            await Assert.That(items[0].Get<string>("Name")).IsEqualTo("Alabama");

            items.Search = "Jefferson";
            await Assert.That(items.Count).IsEqualTo(28);
        }

        [Test]
        public async Task Usage_Sort()
        {
            System.IO.Stream stream = typeof(ItemsTest)
                .Assembly
                .GetManifestResourceStream(
                "Node.Net.Test.Resources.States.json");

            IDictionary states = new Reader().Read<IDictionary>(stream);

            Node.Net.Collections.Items<IDictionary> items = new Node.Net.Collections.Items<IDictionary>(states.Collect<IDictionary>())
            {
                Search = "State",
                SortFunction = SortByNameDescending
            };
            await Assert.That(items.Count).IsEqualTo(50);
            await Assert.That(items[0].Get<string>("Name")).IsEqualTo("Wyoming");
        }

        public static IEnumerable<IDictionary> SortByNameDescending(IEnumerable<IDictionary> source)
        {
            IOrderedEnumerable<IDictionary> ordered = source.OrderByDescending(dictionary => dictionary.Get<string>("Name"));
            return ordered;
        }
    }
}