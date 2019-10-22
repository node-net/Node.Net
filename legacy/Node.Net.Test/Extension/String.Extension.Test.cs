using NUnit.Framework;
using static System.Math;

namespace Node.Net.Extension
{
    [TestFixture]
    class StringExtensionTest
    {
        [Test]
        [TestCase("12in", 0.3048)]
        [TestCase("1ft", 0.3048)]
        public void GetMeters(string value, double lengthMeters)
        {
            var meters = value.GetMeters();
            Assert.AreEqual(lengthMeters, Round(meters, 4));
        }
    }
}
