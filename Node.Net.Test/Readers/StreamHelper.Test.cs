using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    [TestFixture]
    class StreamHelperTest
    {
        [Test]
        public void StreamHelper_GetStream_Filename()
        {
            var filename = Path.GetTempFileName();
            File.WriteAllText(filename,"this is a test");
            using (StreamReader reader = new StreamReader(StreamHelper.GetStream(filename)))
            {
                Assert.AreEqual("this is a test", reader.ReadToEnd());
            }
            File.Delete(filename);

   
        }

        [Test]
        public void StreamHelper_GetStream_Url()
        {
            using (StreamReader reader = new StreamReader(StreamHelper.GetStream("http://www.google.com")))
            {
                var html = reader.ReadToEnd();
                Assert.True(html.Contains("google"));
                Assert.True(html.Contains(nameof(html)));
            }

            var bytes = new List<byte>();
            var stream = StreamHelper.GetStream("ftp://speedtest.tele2.net/1KB.zip");
            var ibyte = stream.ReadByte();
            while(ibyte > -1)
            {
                bytes.Add((byte)ibyte);
                ibyte = stream.ReadByte();
            }
            Assert.AreEqual(1024, bytes.Count);
        }

        [Test]
        public void StreamHelper_GetStream_String()
        {

            using (StreamReader reader = new StreamReader(StreamHelper.GetStream("{}")))
            {
                Assert.AreEqual("{}", reader.ReadToEnd());
            }
        }
        [Test]
        public void StreamHelper_GetStream_ManifestResource()
        {
            var stream = StreamHelper.GetStream("simple.json");
            Assert.NotNull(stream, nameof(stream));
        }
    }
}
