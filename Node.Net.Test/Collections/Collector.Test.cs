using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Collections
{
    [TestFixture]
    public class CollectorTest : Fixture
    {

        #region CollectKeys (moved to IDictionaryExtension.Test)
        /*
        [Test]
        [TestCase("Translations.json", "Type", false, 1)]
        [TestCase("States.json", "Colorado", false, 1)]
        [TestCase("States.json", "Jefferson", true, 1)]
        public void Collector_CollectKeys_Value(string name, string key_value, bool deep, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var filter = new Filters.StringFilter { Value = key_value };
            var collector = new Collector { KeyFilter = filter.Include };
            var keys = collector.CollectKeys(dictionary, deep);
            Assert.AreEqual(count, keys.Length);
        }
        [Test]
        [TestCase("Translations.json", "", false, 9)]
        [TestCase("Translations.json", "e", false, 5)]
        [TestCase("Translations.json", "o", true, 1)]
        [TestCase("States.json", "", false, 50)]
        [TestCase("States.json", "", true, 1884)]
        [TestCase("States.json", "A", true, 101)]
        public void Collector_CollectKeys_Pattern(string name, string key_pattern, bool deep, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var filter = new Filters.StringFilter { Pattern = key_pattern };
            var collector = new Collector { KeyFilter = filter.Include };
            var keys = collector.CollectKeys(dictionary,deep);
            Assert.AreEqual(count, keys.Length);
        }
        */
        #endregion
        #region Collect (moved to IDictionaryExtension.Test)
        /*
        [Test]
        [TestCase("Translations.json","Type",false,1)]
        [TestCase("Translations.json", "Type", true, 3)]
        [TestCase("info.json","path",true,2)]
        public void Collector_Collect_StringValues(string name,string key_value,bool deep,int count)
        {
            var dictionary = Read(name) as IDictionary;
            var filter = new Filters.StringFilter { Value = key_value };
            var collector = new Collector { KeyFilter = filter.Include };
            var keys = collector.Collect<string>(dictionary, deep);
            Assert.AreEqual(count, keys.Count);
        }
        [Test]
        [TestCase("States.json","State",false,50)]
        [TestCase("States.json", "County", false, 0)]
        [TestCase("States.json", "County", true, 3105)]
        public void Collector_Collect_IDictionary(string name,string type_name,bool deep,int count)
        {
            var dictionary = Read(name) as IDictionary;
            var filter = new Filters.IDictionaryFilter<IDictionary>
            {
                Key = "Type",
                KeyStringValue = type_name
            };
            var collector = new Collector { ValueFilter = filter.Include };
            var keys = collector.Collect<IDictionary>(dictionary, deep);
            Assert.AreEqual(count, keys.Count);
        }
        [Test]
        public void Collector_Collect_DeepFilter()
        {
            var dictionary = Read("Translations.json") as IDictionary;
            var collector = new Collector
            {
                ValueFilter = new Filters.IDictionaryFilter<IDictionary>
                {
                    Key = "Type",
                    KeyStringValue = "Bar"
                }.Include,
                DeepFilter = new Filters.IDictionaryFilter<IDictionary>
                {
                    Key = "Type",
                    KeyStringValue = "Foo"
                }.Include
            };
            Assert.AreEqual(1, collector.Collect<IDictionary>(dictionary,true).Count);
            collector.DeepFilter = new Filters.IDictionaryFilter<IDictionary>
            {
                Key = "Type",
                KeyStringValue = "X"
            }.Include;
            Assert.AreEqual(0, collector.Collect<IDictionary>(dictionary, true).Count);
        }
        */
        #endregion

        #region CollectValues (moved to IDictionaryExtension.Test)
        /*
        [Test]
        [TestCase("Translations.json","Type",3)]
        [TestCase("Translations.json", "Height", 1)]
        [TestCase("Translations.json", "Z", 3)]
        public void Collector_CollectValues(string name,string key,int count)
        {
            var dictionary = Read(name) as IDictionary;
            Assert.AreEqual(count,Collector.CollectValues<string>(dictionary, key).Length);
        }
        */
        #endregion

        #region CollectTypes (moved to IDictionaryExtension.Test)
        /*
        [Test]
        [TestCase("Translations.json",3)]
        [TestCase("info.json", 3)]
        [TestCase("States.json", 2)]
        public void Collector_CollectTypes(string name,int count)
        {
            var dictionary = Read(name) as IDictionary;
            Assert.AreEqual(count, Collector.CollectTypes(dictionary).Length);
        }
        */
        #endregion
    }
}
