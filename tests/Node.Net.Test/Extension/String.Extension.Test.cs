using System.IO;
using System.Threading.Tasks;
using Node.Net;

namespace Node.Net.Test.Extension
{
    internal class StringExtensionTest
    {
        [Test]
        public async Task GetStream()
        {
            string text = new StreamReader("test".GetStream()).ReadToEnd();
            await Assert.That(text).IsEqualTo("test");

            text = new StreamReader("Object.Coverage.json".GetStream()).ReadToEnd();
            await Assert.That(text.Contains("array_empty")).IsTrue();
        }

        [Test]
        public async Task GetRawValue()
        {
            await Assert.That("10'".GetRawValue()).IsEqualTo(10.0);
            await Assert.That("10 ft".GetRawValue()).IsEqualTo(10.0);
            await Assert.That("10 m".GetRawValue()).IsEqualTo(10.0);
        }
    }
}