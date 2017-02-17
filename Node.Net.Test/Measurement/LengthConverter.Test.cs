using NUnit.Framework;

namespace Node.Net.Measurement
{
    [TestFixture,Category("Node.Net.Measurement.LengthConverter")]
    public class LengthConverter_Test
    {
        [TestCase]
        public void Measurement_LengthConverter_Usage()
        {
            var converter = new LengthConverter();
            Assert.True(converter.CanConvertFrom(typeof(string)));
            Assert.False(converter.CanConvertFrom(typeof(bool)));
            var length = converter.ConvertFrom("2 in") as Length;
            Assert.NotNull(length);
            Assert.AreEqual(LengthUnit.Inches, length.Units);

            Assert.Throws<System.NotSupportedException>(() => converter.ConvertFrom(false));

            Assert.True(converter.CanConvertTo(typeof(string)));
            Assert.True(converter.CanConvertTo(typeof(Length)));

            var svalue = converter.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, length, typeof(string)).ToString();
            Assert.AreEqual("2 in", svalue);
            svalue = converter.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, null, typeof(string)).ToString();
        }
    }
}
