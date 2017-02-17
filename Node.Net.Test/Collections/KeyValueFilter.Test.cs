using NUnit.Framework;
using System;
using System.Collections;

namespace Node.Net.Collections
{
    [TestFixture]
    public class KeyValueFilterTest : Fixture
    {
        [Test]
        [TestCase("Translations.json", "Type", "Widget",true,true)]
        [TestCase("Translations.json", "Type", "Wid", true, false)]
        [TestCase("Translations.json", "Type", "Wid", false, true)]
        [TestCase("Translations.json", "Type", "Foo", true,false)]
        [TestCase("Translations.json", "X", "", false, true)]
        public void KeyValueFilter_Include_IDictionary(string name, string key, string value,bool exact_match,bool include)
        {
            var dictionary = Read(name) as IDictionary;
            var filter = new KeyValueFilter
            {
                Key = key,
                Values = {value},
                ExactMatch = exact_match
            };
            Assert.AreEqual(include, filter.Include(dictionary));
            Assert.AreEqual(include, Include(filter.Include, dictionary));
        }

        private static bool? Include(Func<object,bool?> include_function,object value)
        {
            if (include_function != null) return include_function(value);
            return null;
        }
    }
}
