using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    internal class TextReaderTest
    {
        [Test]
        public void FastSeek()
        {
            var stream = typeof(JsonReaderTest)
                .Assembly
                .GetManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json");
            using var reader = new StreamReader(stream);
            reader.FastSeek(':', false);
            reader.FastSeek(':', true);
            reader.FastSeek('"', false);
            reader.FastSeek('"', true);
        }
    }
}
