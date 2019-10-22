using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers.Test
{
    class TextReaderTest
    {
        [Test]
        [TestCase("simple.txt", typeof(IList),3)]
        [TestCase("array.json", typeof(IList),1)]
        [TestCase("hash.json", typeof(IList),1)]
        [TestCase("arrays.json", typeof(IList),6)]
        public void TextReader_Read(string name, Type type,int expected_line_count)
        {
            var stream = GlobalFixture.GetStream(name);
            Assert.NotNull(stream, nameof(stream));

            var reader = new TextReader();
            var item = reader.Read(stream);

            Assert.NotNull(item, nameof(item));
            Assert.True(type.IsAssignableFrom(item.GetType()), $"{name} was not Assignable to {type.FullName}");

            var list = item as IList;
            Assert.AreEqual(expected_line_count, list.Count);
        }
    }
}
