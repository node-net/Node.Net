using NUnit.Framework;

namespace Node.Net.Measurement
{
    [TestFixture,Category("Node.Net.Measurement.Length")]
    class Length_Test
    {
        [TestCase]
        public void Length_Usage()
        {
            Length length = Length.Parse("");
            NUnit.Framework.Assert.AreEqual(0, length[LengthUnit.Meter]);
        }
    }
}
