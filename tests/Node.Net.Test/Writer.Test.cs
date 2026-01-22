using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Test
{
    internal static class WriterTest
    {
        [Test]
        public static async Task WriteJson()
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
                await Assert.That(json).IsEqualTo("{\"name\":\"test\"}");
            }

            writer = new Writer { JsonFormat = JsonFormat.Pretty };
            using (MemoryStream memory = new MemoryStream())
            {
                writer.Write(memory, data);
                string json = Encoding.UTF8.GetString(memory.ToArray());
                // Normalize line endings for cross-platform compatibility
                json = json.Replace("\r\n", "\n").Replace("\r", "\n");
                await Assert.That(json).IsEqualTo("{\n  \"name\":\"test\"\n}");
            }
        }

        [Test]
        public static async Task WriteISerializableJson()
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
            await Assert.That(json.Contains("abc")).IsTrue();
            await Assert.That(json.Contains("test")).IsTrue();
        }
    }
}