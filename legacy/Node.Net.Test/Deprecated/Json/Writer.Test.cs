using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Deprecated.Json
{
    [TestFixture,Category("Node.Net.Deprecated.Json.Writer")]
    class Writer_Test
    {
        [TestCase]
        public void Writer_Usage()
        {
            IDictionary hash = new Dictionary<string, object>();
            hash["string"] = "ABC";
            Assert.AreEqual("ABC", hash["string"] as string);
            hash["false"] = false;
            Assert.AreEqual(false, (bool)hash["false"]);
            hash["true"] = true;
            Assert.AreEqual(true, (bool)hash["true"]);
            hash["null"] = null;
            Assert.AreEqual(null, hash["null"]);
            Assert.IsNull(hash["null"]);
            hash["double"] = 1.23;
            Assert.AreEqual(1.23, (double)hash["double"]);

            using (MemoryStream memory = new MemoryStream())
            {
                Writer.Write(hash, memory);
                Assert.True(memory.Length > 0);
                memory.Seek(0, SeekOrigin.Begin);
                using(StreamReader sr = new StreamReader(memory))
                {
                    var j = sr.ReadToEnd();
                    Assert.True(j.Contains("1.23"));
                }
            }

            var json = Writer.ToString(hash);
            Assert.True(json.Contains("1.23"));
        }

        [TestCase]
        public void Writer_Backslash_Escaping()
        {
            var list =
                new List<string>();
            list.Add("\\"); //  a single backslash character
            var json = Writer.ToString(list);
            var array = new Deprecated.Collections.Array(json);
            NUnit.Framework.Assert.AreEqual("\\", array[0].ToString());
        }

        [TestCase]
        public void Writer_Backslash_Escaping2()
        {
            var hash = new Deprecated.Collections.Hash();
            //                dir "bin\Debug\".
            hash["Output"] = "dir \"bin\\Debug\\\".";
            var json = Writer.ToString(hash);
            //               "dir \"bin\\Debug\\\"."
            //Assert.True(json.IndexOf("dir \\\"bin\\\\Debug\\\".") > -1);
        }

        [TestCase]
        public void Writer_IgnoreTypes()
        {
            var hash = new Deprecated.Collections.Hash();
            hash["Name"] = "ignoreTypeTest";
            hash["Double"] = 1.23;
            var writer = new Writer();
            writer.IgnoreTypes.Add(typeof(double));
            using (MemoryStream memory = new MemoryStream())
            {
                writer.Write(memory, hash);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                var hash2 = new Deprecated.Collections.Hash(memory);
                Assert.True(hash2.ContainsKey("Name"));
                Assert.False(hash2.ContainsKey("Double"));
            }
        }
        [TestCase]
        public void Writer_IgnoreNullValues()
        {
            var hash = new Deprecated.Collections.Hash();
            hash["Name"] = "ignoreNullValuesTest";
            hash["Null"] = null;
            hash["Double"] = 1.23;
            var writer = new Writer
            {
                IgnoreNullValues = true
            };
            using (MemoryStream memory = new MemoryStream())
            {
                writer.Write(memory, hash);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                var hash2 = new Deprecated.Collections.Hash(memory);
                Assert.True(hash2.ContainsKey("Name"));
                Assert.False(hash2.ContainsKey("Null"));
                Assert.True(hash2.ContainsKey("Double"));
            }
        }
    }
}
