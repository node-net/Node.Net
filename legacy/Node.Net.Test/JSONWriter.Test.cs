using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections;

namespace Node.Net.Tests
{
    class JSONWriterTest
    {
        [Test]
        [TestCase("Object.Sample.json")]
        public void ReadWrite(string name)
        {
            var data = Factory.Default.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
            var json = JSONWriter.Default.WriteToString(data);
            Assert.True(json.Length > 10);

            using (var memory = new MemoryStream())
            {
                var sw = new StreamWriter(memory);
                sw.Write(json);
                sw.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                var data2 = Reader.Default.Read(memory);
                Assert.NotNull(data2, nameof(data2));
            }
        }
    }
}
