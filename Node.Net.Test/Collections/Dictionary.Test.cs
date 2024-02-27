using NUnit.Framework;
using System;
using System.Collections;

namespace Node.Net.Collections
{
    [TestFixture]
    internal class DictionaryTest
    {
        [Test]
        public void Clone()
        {
            DateTime timestamp = new DateTime(2019, 11, 14);
            Dictionary data = new Dictionary
            {
                {"Name","test" },
                {"Description","a test dictionary" },
                {"Timestamp", timestamp }
            };

            IDictionary data2 = (data as IDictionary).Clone();
            DateTime timestamp2 = (DateTime)data2["Timestamp"];
            Assert.That(timestamp2.Day,Is.EqualTo(14), "timestamp2.Day");

            JsonFormatter jsonFormatter = new JsonFormatter();
            Assert.That(jsonFormatter.GetMD5(data), Is.EqualTo(jsonFormatter.GetMD5(data2)), "json MD5");
        }
    }
}