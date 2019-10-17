using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    class JsonReaderTest
    {
        [Test]
        public void Read()
        {
            var stream = typeof(JsonReaderTest)
                .Assembly
                .GetManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json");

            var dictionary = new JsonReader().Read(stream) as IDictionary;
            Assert.NotNull(dictionary, nameof(dictionary));
            Assert.AreEqual(14, dictionary.Count, "dictionary.Count");
        }
    }
}
