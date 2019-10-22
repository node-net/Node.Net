using NUnit.Framework;
using System.Collections;

namespace Node.Net.Collections.Filters
{
    class IDictionaryFilterTest : Fixture
    {
        [Test]
        [TestCase("Translations.json", "Type", "Foo", false)]
        [TestCase("Translations.json", "Type", "Widget", true)]
        public void IDictionaryFilter_KeyStringValue(string test_dictionary_name, string key, string key_string_value, object expected_result)
        {
            Assert.AreEqual(
                expected_result,
                Filters.GetIDictionaryFilter<IDictionary>(key, key_string_value)(Read(test_dictionary_name) as IDictionary));
            /*
            Assert.AreEqual(
                expected_result,
                new IDictionaryFilter<IDictionary>
                {
                    Key = key,
                    KeyStringValue = key_string_value
                }.Include(Read(test_dictionary_name) as IDictionary));*/
        }
        [Test]
        [TestCase("Translations.json", "Type", "F", false)]
        [TestCase("Translations.json", "Type", "W", true)]
        public void IDictionaryFilter_KeyStringPattern(string test_dictionary_name, string key, string key_string_pattern, object expected_result)
        {
            Assert.AreEqual(expected_result,
                Filters.GetIDictionaryFilter<IDictionary>(
                    key,null,key_string_pattern)(Read(test_dictionary_name) as IDictionary));
            /*
            Assert.AreEqual(
                expected_result,
                new IDictionaryFilter<IDictionary>
                {
                    Key = key,
                    KeyStringPattern = key_string_pattern
                }.Include(Read(test_dictionary_name) as IDictionary));*/
        }
    }
}
