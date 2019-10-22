using NUnit.Framework;

namespace Node.Net._Measurement
{
    [TestFixture,Category(nameof(Measurement))]
    class Length_Test
    {
        [TestCase]
        public void Length_Parse()
        {
            var length = Length.Parse(string.Empty);
            Assert.AreEqual(0, length[LengthUnit.Meter]);
        }
    }
}
