﻿using NUnit.Framework;
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
                Assert.AreEqual("{\"name\":\"test\"}", json);
            }

            writer = new Writer { JsonFormat = JsonFormat.Pretty };
            using (MemoryStream memory = new MemoryStream())
            {
                writer.Write(memory, data);
                string json = Encoding.UTF8.GetString(memory.ToArray());
                Assert.AreEqual("{\r\n  \"name\":\"test\"\r\n}", json);
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

            using (MemoryStream memory = new MemoryStream())
            {
                new Writer().Write(memory, widget);
                memory.Seek(0, SeekOrigin.Begin);
                string json = new StreamReader(memory).ReadToEnd();
                Assert.True(json.Contains("abc"));
                Assert.True(json.Contains("test"));
            }
        }
    }
}