using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Node.Net.Collections.Test
{
    [TestFixture, Category(nameof(KeyValuePair))]
    class KeyValuePairTest
    {
        [TestCase(null)]
        [TestCase(10)]
        [TestCase('c')]
        [TestCase("abc")]
        [TestCase(1.23)]
        public void KeyValuePair_IsNotKeyValuePair(object value)
        {
            Assert.False(KeyValuePair.IsKeyValuePair(value));
        }
        [TestCase("a", 3)]
        public void KeyValuePair_IsKeyValuePair(string key, object value)
        {
            var kvp = new KeyValuePair<string, object>(key, value);
            Assert.True(KeyValuePair.IsKeyValuePair(kvp));
            Assert.AreEqual(key, KeyValuePair.GetKey(kvp));
            Assert.AreEqual(value, KeyValuePair.GetValue(kvp));

            var entry = new DictionaryEntry(key, value);
            Assert.True(KeyValuePair.IsKeyValuePair(entry));
            Assert.AreEqual(key, KeyValuePair.GetKey(entry));
            Assert.AreEqual(value, KeyValuePair.GetValue(entry));

            Assert.True(KeyValuePair.IsKeyValuePair(new KeyValuePair<string,string>()));

        }
        [TestCase]
        public void KeyValuePair_GetKey()
        {
            Assert.AreEqual(3, KeyValuePair.GetKey(3));
            var
                kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual("a", KeyValuePair.GetKey(kvp));
            Assert.AreEqual("a", KeyValuePair.GetKey(new KeyValuePair<string, object>("a", null)));
            Assert.AreEqual("a", KeyValuePair.GetKey(new DictionaryEntry("a", null)));
            Assert.AreEqual("a", KeyValuePair.GetKey(new KeyValuePair<string,string>("a","A")));
        }
        [TestCase]
        public void KeyValuePair_GetValue()
        {
            Assert.AreEqual(3, KeyValuePair.GetValue(3));
            var
               kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual(3, KeyValuePair.GetValue(kvp));
            Assert.AreEqual("A", KeyValuePair.GetValue(new KeyValuePair<string, string>("a", "A")));
        }
    }
}
