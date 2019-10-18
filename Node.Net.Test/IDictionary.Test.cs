using NUnit.Framework;
using System.Collections;
using System.Linq;
using static System.Math;

namespace Node.Net
{
    public class IDictionaryTest
    {
        [Test]
        public void Collect()
        {
            var stream = typeof(JsonReaderTest)
                .Assembly
                .GetManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json");

            var dictionary = new JsonReader().Read(stream) as IDictionary;
            Assert.NotNull(dictionary, nameof(dictionary));
            Assert.AreEqual(4, dictionary.Collect<IDictionary>().Count);
        }

        [Test]
        public void Get()
        {
            var stream = typeof(JsonReaderTest)
                .Assembly
                .GetManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json");

            var dictionary = new JsonReader().Read(stream) as IDictionary;
            Assert.NotNull(dictionary, nameof(dictionary));
            Assert.AreEqual(string.Empty, dictionary.Get<string>("not_there"), "not_there");
            Assert.AreEqual("base64:dGVzdA==", dictionary.Get<string>("base64"), "base64");
            Assert.AreEqual("base64:dGVzdA==", dictionary.Get<string>("not_there,base64"), "not_there,base64");
            Assert.AreEqual(1.23, Round(dictionary.Get<float>("float_1"),2), "float_1");
        }

        [Test]
        public void Spatial()
        {
            var stream = typeof(JsonReaderTest)
                .Assembly
                .GetManifestResourceStream("Node.Net.Test.Resources.Spatial.json");

            var dictionary = new JsonReader().Read(stream) as IDictionary;
            Assert.NotNull(dictionary, nameof(dictionary));
            dictionary.DeepUpdateParents();

            var widgetA = dictionary.Get<IDictionary>("widgetA");
            Assert.NotNull(widgetA, nameof(widgetA));
            Assert.True(widgetA.GetLocalToParent().IsIdentity, "widgetA.GetLocalToParent().IsIdentity");

            var widgetB = dictionary.Get<IDictionary>("widgetB");
            Assert.NotNull(widgetB, nameof(widgetB));
            Assert.False(widgetB.GetLocalToParent().IsIdentity, "widgetB.GetLocalToParent().IsIdentity");

            var fooA = dictionary.Collect<IDictionary>().First(d => d.GetName() == "fooA");
            Assert.NotNull(fooA, nameof(fooA));
            Assert.True(fooA.GetLocalToParent().IsIdentity, "fooA.GetLocalToParent().IsIdentity");

            var fooB = dictionary.Collect<IDictionary>().First(d => d.GetName() == "fooB");
            Assert.NotNull(fooB, nameof(fooB));
            Assert.False(fooB.GetLocalToParent().IsIdentity, "fooB.GetLocalToParent().IsIdentity");
        }
    }
}