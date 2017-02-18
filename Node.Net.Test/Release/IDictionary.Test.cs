using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Node.Net;

namespace Node.Net.Tests
{
    [TestFixture]
    public class IDictionaryTest : Fixture
    {
        #region CollectKeys
        [Test]
        [TestCase("Translations.json", "Type", true)]
        [TestCase("States.Partial.json", "Colorado", true)]
        [TestCase("States.Partial.json", "Jefferson", true)]
        [TestCase("States.Partial.json", "Softball", false)]
        public void IDictionary_CollectKeys_Value(string name, string key_value, bool contains_value)
        {
            using (var reader = new Node.Net.Reader
            {
                DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                DefaultObjectType = typeof(Dictionary<string, dynamic>)
            })
            {

                // abc
                var data = reader.Read(name) as IDictionary;
                var keys = new List<string>(data.CollectKeys());
                Assert.AreEqual(contains_value, keys.Contains(key_value));
            }
        }
        #endregion
        #region Collect
        [Test]
        [TestCase("Translations.json", "Type", 3)]
        [TestCase("info.json", "path", 2)]
        [TestCase("States.Partial.json", "Name", 127)]
        public void IDictionary_Collect_StringValues(string name, string key_value, int count)
        {
            using (var reader = new Node.Net.Reader
            {
                DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                DefaultObjectType = typeof(Dictionary<string, dynamic>)
            })
            {

                // abc
                var dictionary = reader.Read(name) as IDictionary;
                var keys = dictionary.Collect<string>(null, Filters.GetStringFilter(key_value));
                Assert.AreEqual(count, keys.Count);
            }
        }
        [Test]
        [TestCase("States.Partial.json", "State", false, 2)]
        [TestCase("States.Partial.json", "County", false, 0)]
        [TestCase("States.Partial.json", "County", true, 125)]
        public void IDictionaryExtension_Collect_IDictionary(string name, string type_name, bool deep, int count)
        {
            

            var dictionary = Read(name) as IDictionary;
            var valueInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", type_name);
            var keys = dictionary.Collect<IDictionary>(valueInclude, null, null, deep);
            Assert.AreEqual(count, keys.Count);
        }
        [Test]
        public void IDictionaryExtension_Collect_DeepFilter()
        {
            var dictionary = Read("Translations.json") as IDictionary;
            var valueInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", "Bar");
            var deepInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", "Foo");
            var instances = dictionary.Collect<IDictionary>(valueInclude, null, deepInclude);
            Assert.AreEqual(1, instances.Count);
            deepInclude = Filters.GetIDictionaryFilter<IDictionary>("Type", "X");
            instances = dictionary.Collect<IDictionary>(valueInclude, null, deepInclude);
            Assert.AreEqual(0, instances.Count);
        }
        #endregion

        #region CollectValues
        [Test]
        [TestCase("Translations.json", "Type", 3)]
        [TestCase("Translations.json", "Height", 1)]
        [TestCase("Translations.json", "Z", 3)]
        public void IDictionaryExtension_CollectValues(string name, string key, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var values = dictionary.CollectValues<string>( key);
            Assert.AreEqual(count, values.Length);
        }
        #endregion

        #region CollectTypes
        [Test]
        [TestCase("Translations.json", 3)]
        [TestCase("info.json", 3)]
        [TestCase("States.Partial.json", 2)]
        public void IDictionaryExtension_CollectTypes(string name, int count)
        {
            var dictionary = Read(name) as IDictionary;
            var types = dictionary.CollectTypes();
            Assert.AreEqual(count, types.Length);
        }
        #endregion
        [Test]
        public void IDictionary_GetLocalToParent()
        {
            using (var factory = new global::Node.Net.Factory())
            {
                var dictionary = new Dictionary<string, dynamic>();
                Assert.True(dictionary.GetLocalToParent().IsIdentity);
            }
        }
    }
}
