using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Test
{
    [TestFixture]
    internal class ReaderTest
    {
        [TestCase("Object.Coverage.json")]
        public void Read(string name)
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
            Assert.That(i.Contains("string_symbol"),Is.True, "i.Contains 'string_symbol'");
            Assert.That(i["string_symbol"].ToString(), Is.EqualTo("0°"), "i['string_symbol']");

            memory2.Seek(0, SeekOrigin.Begin);
            string filename = Path.GetTempFileName();
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                memory2.CopyTo(fs);
            }
            IDictionary d = reader.Read(filename) as IDictionary;
            Assert.That(d.Contains("string_symbol"),Is.True, "d.Contains 'string_symbol'");
            Assert.That(d["string_symbol"].ToString(), Is.EqualTo("0°"), "d['string_symbol']");

            using MemoryStream memory3 = new MemoryStream();
            d.Save(memory3);
        }

        [Test]
        public void PreserveBackslash()
        {
            // https://stackoverflow.com/questions/19176024/how-to-escape-special-characters-in-building-a-json-string
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"User",@"Domain\User" }
            };

            string json = (data as IDictionary).ToJson();
            Assert.That(json.Contains(@"Domain\u005cUser"),Is.True, "json contains 'Domain\u005cUser'");

            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                IDictionary d = new Reader().Read<IDictionary>(memory);
                Assert.That(d.Contains("User"),Is.True);
                string user = d["User"].ToString();
                Assert.That( d["User"].ToString(),Is.EqualTo(@"Domain\User"));
            }

            json = "{\"User\":\"Domain\\User\"}";
            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                IDictionary d = new Reader().Read<IDictionary>(memory);
                Assert.That(d.Contains("User"),Is.True);
                string user = d["User"].ToString();
                Assert.That( d["User"].ToString(),Is.EqualTo(@"Domain\User"));
            }
        }

        [Test]
        public void PreserveBackslash2()
        {
            const string json = "{ \"path\" : \"C:\\\\tmp\" }";
            IDictionary data = new Reader()
                .Read<IDictionary>(new MemoryStream(Encoding.UTF8.GetBytes(json)));
            Assert.That(data.Contains("path"), Is.True, "data.Contains 'path'");
            string path = data["path"].ToString();
            Assert.That(path.Contains("\\"),Is.True, "path contains \\");
            Assert.That(path, Is.EqualTo("C:\\tmp"));
        }
    }
}