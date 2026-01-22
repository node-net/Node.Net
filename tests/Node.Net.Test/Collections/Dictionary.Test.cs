using NUnit.Framework;
using Node.Net;
using Node.Net.Collections;
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
            Node.Net.Collections.Dictionary data = new Node.Net.Collections.Dictionary
            {
                {"Name","test" },
                {"Description","a test dictionary" },
                {"Timestamp", timestamp }
            };

            IDictionary data2 = (data as IDictionary).Clone();
            DateTime timestamp2 = (DateTime)data2["Timestamp"];
            Assert.That(timestamp2.Day,Is.EqualTo(14), "timestamp2.Day");

            Node.Net.JsonFormatter jsonFormatter = new Node.Net.JsonFormatter();
            //Assert.That(jsonFormatter.GetMD5(data), Is.EqualTo(jsonFormatter.GetMD5(data2)), "json MD5");
        }
    }
}