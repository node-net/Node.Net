using NUnit.Framework;
using System.IO;

namespace Node.Net.Test.Extension
{
    [TestFixture]
    internal class StringExtensionTest
    {
        [Test]
        public void GetStream()
        {
            string text = new StreamReader("test".GetStream()).ReadToEnd();
            Assert.That(text, Is.EqualTo("test"));

            text = new StreamReader("Object.Coverage.json".GetStream()).ReadToEnd();
            Assert.That(text.Contains("array_empty"), Is.True,"array_empty not found in Object.Coverage.json");
        }

        [Test]
        public void GetRawValue()
        {
            Assert.That("10'".GetRawValue(), Is.EqualTo(10.0));
            Assert.That("10 ft".GetRawValue(), Is.EqualTo(10.0));
            Assert.That("10 m".GetRawValue(), Is.EqualTo(10.0));
        }
    }
}