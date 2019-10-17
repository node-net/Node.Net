using NUnit.Framework;
using System.Collections;
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
    }
}