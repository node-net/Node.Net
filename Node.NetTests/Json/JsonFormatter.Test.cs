using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Json
{
    [TestFixture,Category("JsonFormatter")]
    class JsonFormatterTest
    {
        [TestCase]
        public void JsonFormatter_Usage()
        {
            Dictionary<string, dynamic> dictionary = new Dictionary<string, dynamic>();
            dictionary["a"] = 0;
            dictionary["b"] = 1;

            MemoryStream memory = new MemoryStream();
            JsonFormatter jsonFormatter = new JsonFormatter();
            jsonFormatter.Serialize(memory, dictionary);

            memory.Seek(0, SeekOrigin.Begin);
            Dictionary<string, dynamic> d2 = (Dictionary<string, dynamic>)jsonFormatter.Deserialize(memory);
            Assert.NotNull(d2);
            Assert.AreEqual(0, d2["a"]);
            Assert.AreEqual(1, d2["b"]);
        }
    }
}
