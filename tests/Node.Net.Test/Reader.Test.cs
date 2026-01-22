using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Test
{
    internal class ReaderTest
    {
        [Test]
        [Arguments("Object.Coverage.json")]
        public async Task Read(string name)
        {
            Reader reader = new Reader();
            global::System.Reflection.Assembly assembly = typeof(ReaderTest).Assembly;
            Stream stream = assembly.FindManifestResourceStream(name);
            using MemoryStream memory = new MemoryStream();
            stream.CopyTo(memory);
            memory.Seek(0, SeekOrigin.Begin);

            MemoryStream memory2 = new MemoryStream();
            memory.CopyTo(memory2);
            memory.Seek(0, SeekOrigin.Begin);

            IDictionary i = reader.Read<IDictionary>(memory);
            await Assert.That(i.Contains("string_symbol")).IsTrue();
            await Assert.That(i["string_symbol"].ToString()).IsEqualTo("0°");

            memory2.Seek(0, SeekOrigin.Begin);
            string filename = Path.GetTempFileName();
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                memory2.CopyTo(fs);
            }
            IDictionary d = reader.Read(filename) as IDictionary;
            await Assert.That(d.Contains("string_symbol")).IsTrue();
            await Assert.That(d["string_symbol"].ToString()).IsEqualTo("0°");

            using MemoryStream memory3 = new MemoryStream();
            d.Save(memory3);
        }

        [Test]
        public async Task PreserveBackslash()
        {
            // https://stackoverflow.com/questions/19176024/how-to-escape-special-characters-in-building-a-json-string
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"User",@"Domain\User" }
            };

            string json = (data as IDictionary).ToJson();
            await Assert.That(json.Contains(@"Domain\u005cUser")).IsTrue();

            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                IDictionary d = new Reader().Read<IDictionary>(memory);
                await Assert.That(d.Contains("User")).IsTrue();
                string user = d["User"].ToString();
                await Assert.That(d["User"].ToString()).IsEqualTo(@"Domain\User");
            }

            json = "{\"User\":\"Domain\\User\"}";
            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                IDictionary d = new Reader().Read<IDictionary>(memory);
                await Assert.That(d.Contains("User")).IsTrue();
                string user = d["User"].ToString();
                await Assert.That(d["User"].ToString()).IsEqualTo(@"Domain\User");
            }
        }

        [Test]
        public async Task PreserveBackslash2()
        {
            const string json = "{ \"path\" : \"C:\\\\tmp\" }";
            IDictionary data = new Reader()
                .Read<IDictionary>(new MemoryStream(Encoding.UTF8.GetBytes(json)));
            await Assert.That(data.Contains("path")).IsTrue();
            string path = data["path"].ToString();
            await Assert.That(path.Contains("\\")).IsTrue();
            await Assert.That(path).IsEqualTo("C:\\tmp");
        }
    }
}