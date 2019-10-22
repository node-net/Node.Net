using NUnit.Framework;
using System;
using System.Collections;

namespace Node.Net.Readers.Test
{
    [TestFixture]
    public class JsonReaderTest
    {
        [Test]
        [TestCase("simple.json", typeof(IDictionary))]
        [TestCase("array.json", typeof(IList))]
        [TestCase("hash.json", typeof(IDictionary))]
        [TestCase("arrays.json", typeof(IDictionary))]
        public void JsonReader_Read(string name, Type type)
        {
            var stream = GlobalFixture.GetStream(name);
            Assert.NotNull(stream, nameof(stream));

            var reader = new JsonReader();
            var item = reader.Read(stream);

            Assert.NotNull(item, nameof(item));
            Assert.True(type.IsAssignableFrom(item.GetType()), $"{name} was not Assignable to {type.FullName}");
        }

        [Test]
        public void JsonReader_Read_Arrays()
        {
            var reader = new JsonReader();
            var dictionary = reader.Read(GlobalFixture.GetStream("arrays.json")) as IDictionary;

            var int_array = dictionary["int_array"];
            Assert.AreSame(typeof(double[]), int_array.GetType());
        }
    }
}
