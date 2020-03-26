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
            Assert.AreEqual(14, timestamp2.Day, "timestamp2.Day");

            JsonFormatter jsonFormatter = new JsonFormatter();
            Assert.AreEqual(jsonFormatter.GetMD5(data), jsonFormatter.GetMD5(data2), "json MD5");

            //Assert.AreEqual(data.ComputeHashCode(), data2.ComputeHashCode(), "hash code");
            //Assert.True(data.Equals(data2), "equals");
        }

        public void Collect()
        {
            Dictionary states = Dictionary.Parse(Sample.Files.Repository.GetStream("Json/States.json"));

            System.Collections.Generic.IList<IDictionary> dictionaries = states.Collect<IDictionary>();
            Assert.AreEqual(3205, dictionaries.Count);

            System.Collections.Generic.IList<object> list = states.Collect(typeof(IDictionary), "");
            Assert.AreEqual(3205, list.Count);

            System.Collections.Generic.IList<object> counties = states.Collect("County");
            Assert.AreEqual(3105, counties.Count, "counties.Count");

            System.Collections.Generic.IList<IDictionary> dictionaries3 = states.Collect<IDictionary>(Include);
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