using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Collections
{
    [TestFixture]
    internal class DictionaryTest
    {
        [Test]
        public void Clone()
        {
            var timestamp = new DateTime(2019, 11, 14);
            var data = new Dictionary
            {
                {"Name","test" },
                {"Description","a test dictionary" },
                {"Timestamp", timestamp }
            };

            var data2 = data.Clone();
            var timestamp2 = (DateTime)data2["Timestamp"];
            Assert.AreEqual(14, timestamp2.Day, "timestamp2.Day");

            var jsonFormatter = new JsonFormatter();
            Assert.AreEqual(jsonFormatter.GetMD5(data), jsonFormatter.GetMD5(data2), "json MD5");

            //Assert.AreEqual(data.ComputeHashCode(), data2.ComputeHashCode(), "hash code");
            //Assert.True(data.Equals(data2), "equals");
        }
    }
}
