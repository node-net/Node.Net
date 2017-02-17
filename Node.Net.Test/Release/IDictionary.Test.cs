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
        [TestCase("States.json", "Colorado", true)]
        [TestCase("States.json", "Jefferson", true)]
        [TestCase("States.json", "Softball", false)]
        public void IDictionary_CollectKeys_Value(string name, string key_value, bool contains_value)
        {
            var reader = new Node.Net.Reader
            {
                DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                DefaultObjectType = typeof(Dictionary<string, dynamic>)
            };

            // abc
            var data = reader.Read(name) as IDictionary;
            var keys = new List<string>(data.CollectKeys());
            Assert.AreEqual(contains_value, keys.Contains(key_value));
        }
        #endregion
        #region Collect
        [Test]
        [TestCase("Translations.json", "Type", 3)]
        [TestCase("info.json", "path", 2)]
        [TestCase("States.json", "Name", 3155)]
        public void IDictionary_Collect_StringValues(string name, string key_value, int count)
        {
            var reader = new Node.Net.Reader
            {
                DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                DefaultObjectType = typeof(Dictionary<string, dynamic>)
            };

            // abc
            var dictionary = reader.Read(name) as IDictionary;
            var keys = dictionary.Collect<string>(null,Filters.GetStringFilter(key_value));
            Assert.AreEqual(count, keys.Count);
        }
        [Test]
        [TestCase("States.json", "State", false, 50)]
        [TestCase("States.json", "County", false, 0)]
        [TestCase("States.json", "County", true, 3105)]
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
        [TestCase("States.json", 2)]
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

        [Test]
        [TestCase("States.json", "Type", "State", 50)]
        [TestCase("States.json", "Type", "County", 3105)]
        [TestCase("States.json", "Name", "Jefferson", 28)]
        public void IDictionary_Key_Type_Count(string name, string key, string value, int count)
        {
            var data = Read(name) as IDictionary;
            Assert.NotNull(data, nameof(data));
            //Assert.AreEqual(count,data.Collect<IDictionary>()








            /*
            Assert.AreEqual(count, data.DeepCollect<IDictionary>(
                new Node.Net.Collections.KeyValueFilter { Key = key, Values = { value } }.Include).Count);*/
        }
    }
}
