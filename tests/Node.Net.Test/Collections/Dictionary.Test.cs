using System;
using System.Collections;
using System.Threading.Tasks;
using Node.Net;
using Node.Net.Collections;

namespace Node.Net.Collections
{
    internal class DictionaryTest
    {
        [Test]
        public async Task Clone()
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
            await Assert.That(timestamp2.Day).IsEqualTo(14);

            Node.Net.JsonFormatter jsonFormatter = new Node.Net.JsonFormatter();
            //Assert.That(jsonFormatter.GetMD5(data), Is.EqualTo(jsonFormatter.GetMD5(data2)), "json MD5");
            await Task.CompletedTask;
        }
    }
}