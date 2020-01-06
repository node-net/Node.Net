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

        public void Collect()
        {
            var states = Dictionary.Parse(Sample.Files.Repository.GetStream("Json/States.json"));

            var dictionaries = states.Collect<IDictionary>();
            Assert.AreEqual(3205, dictionaries.Count);

            var list = states.Collect(typeof(IDictionary), "");
            Assert.AreEqual(3205, list.Count);

            var counties = states.Collect("County");
            Assert.AreEqual(3105, counties.Count, "counties.Count");

            var dictionaries3 = states.Collect<IDictionary>(Include);
            Assert.AreEqual(3205, dictionaries3.Count, "dictionaries3.Count");

            states.DeepUpdateParents();
        }

        private bool Include(object item)
        {
            return true;
        }

        [Test]
        public void Collect1() => Collect();

        [Test]
        public void Collect2() => Collect();

        [Test]
        public void Collect3() => Collect();

        [Test]
        public void Collect4() => Collect();

        [Test]
        public void Collect5() => Collect();
    }
}