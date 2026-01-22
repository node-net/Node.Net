using NUnit.Framework;
using Node.Net;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class WriterTest
    {
        [Test]
        public static void WriteJson()
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"name","test" }
            };

            Writer writer = new Writer { JsonFormat = JsonFormat.Compact };
            using (MemoryStream memory = new MemoryStream())
            {
                writer.Write(memory, data);
                string json = Encoding.UTF8.GetString(memory.ToArray());
                Assert.That( json,Is.EqualTo("{\"name\":\"test\"}"));
            }

            writer = new Writer { JsonFormat = JsonFormat.Pretty };
            using (MemoryStream memory = new MemoryStream())
            {
                writer.Write(memory, data);
                string json = Encoding.UTF8.GetString(memory.ToArray());
                // Normalize line endings for cross-platform compatibility
                json = json.Replace("\r\n", "\n").Replace("\r", "\n");
                Assert.That(json, Is.EqualTo("{\n  \"name\":\"test\"\n}"));
            }
        }

        [Test]
        public static void WriteISerializableJson()
        {
            Widget widget = new Widget
            {
                Name = "abc",
                Description = "test"
            };

            using MemoryStream memory = new MemoryStream();
            new Writer().Write(memory, widget);
            memory.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(memory).ReadToEnd();
            Assert.That(json.Contains("abc"),Is.True);
            Assert.That(json.Contains("test"), Is.True);
        }
    }
}