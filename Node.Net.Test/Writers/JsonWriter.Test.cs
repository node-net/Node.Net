using NUnit.Framework;
using System.IO;

namespace Node.Net.Writers
{
    [TestFixture]
    class JsonWriterTest
    {
        [Test]
        [TestCase("hash.json")]
        [TestCase("primitives.json")]
        [TestCase("simple.A.json")]
        [TestCase("States.json")]
        public void JsonWriter_Write_Indented(string name)
        {
            var stream = Node.Net.Writers.Test.GlobalFixture.GetStream(name);
            Assert.NotNull(stream, nameof(stream));
            string text;
            using (StreamReader sr = new StreamReader(stream)) { text = sr.ReadToEnd(); }

            var reader = new Node.Net.Readers.Reader();
            var item = reader.Read(Node.Net.Writers.Test.GlobalFixture.GetStream(name));
            using (MemoryStream memory = new MemoryStream())
            {
                var writer = new JsonWriter { Format = JsonFormat.Indented };
                writer.Write(memory, item);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(memory))
                {
                    string text2 = sr.ReadToEnd();

                    Assert.AreEqual(text, text2);
                }
            }
        }
    }
}
