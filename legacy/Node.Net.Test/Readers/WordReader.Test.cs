using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    [TestFixture]
    class WordReaderTest
    {
        [Test]
        public void WordReader_Usage()
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(memory, Encoding.UTF8, 1024, true))
                {
                    writer.WriteLine("string");
                    writer.WriteLine("45");
                    writer.WriteLine("1.23");
                }
                memory.Seek(0, SeekOrigin.Begin);
                using (var reader = new Node.Net.Readers.WordReader(new StreamReader(memory)))
                {
                    Assert.AreEqual("string", reader.ReadWord());
                    Assert.AreEqual(45, reader.ReadInt32());
                    Assert.AreEqual(1.23, reader.ReadDouble());
                }
            }
        }
    }
}
