using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Node.Net.Json
{
    [TestFixture,Category("Json"),Category("KeyValuePair")]
    class KeyValuePair_Test
    {
        [TestCase,Category("Json"),Category("KeyValuePair")]
        public void KeyValuePair_IsKeyValuePair()
        {
            Assert.False(KeyValuePair.IsKeyValuePair(null));
            Assert.False(KeyValuePair.IsKeyValuePair(10));
            Assert.True(KeyValuePair.IsKeyValuePair(new KeyValuePair<string, object>()));
            Assert.False(KeyValuePair.IsKeyValuePair(null));
            Assert.False(KeyValuePair.IsKeyValuePair('c'));
            Assert.False(KeyValuePair.IsKeyValuePair("abc"));
            Assert.False(KeyValuePair.IsKeyValuePair(1.23));

            KeyValuePair<string, int>
                kvp = new KeyValuePair<string, int>("a", 3);
            Assert.True(KeyValuePair.IsKeyValuePair(kvp));
        }
        [TestCase]
        public void KeyValuePair_GetKey()
        {
            Assert.AreEqual(3, KeyValuePair.GetKey(3));
            KeyValuePair<string, int>
                kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual("a", KeyValuePair.GetKey(kvp));
            Assert.AreEqual("a", KeyValuePair.GetKey(new KeyValuePair<string, object>("a", null)));
        }
        [TestCase]
        public void KeyValuePair_GetValue()
        {
            Assert.AreEqual(3, KeyValuePair.GetValue(3));
            KeyValuePair<string, int>
               kvp = new KeyValuePair<string, int>("a", 3);
            Assert.AreEqual(3, KeyValuePair.GetValue(kvp));
            Assert.AreEqual("A", KeyValuePair.GetValue(new KeyValuePair<string, string>("a", "A")));
        }
    }
}
