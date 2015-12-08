using NUnit.Framework;

namespace Node.Net.Measurement
{
    [TestFixture,Category("Node.Net.Measurement.LengthConverter")]
    public class LengthConverter_Test
    {
        [TestCase]
        public void Measurement_LengthConverter_Usage()
        {
            LengthConverter converter = new LengthConverter();
            NUnit.Framework.Assert.True(converter.CanConvertFrom(typeof(string)));
            NUnit.Framework.Assert.False(converter.CanConvertFrom(typeof(bool)));
            Length length = converter.ConvertFrom("2 in") as Length;
            NUnit.Framework.Assert.NotNull(length);
            NUnit.Framework.Assert.AreEqual(LengthUnit.Inches, length.Units);

            NUnit.Framework.Assert.Throws<System.NotSupportedException>(() => converter.ConvertFrom(false));

            NUnit.Framework.Assert.True(converter.CanConvertTo(typeof(string)));
            NUnit.Framework.Assert.True(converter.CanConvertTo(typeof(Length)));

            string svalue = converter.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, length, typeof(string)).ToString();
            NUnit.Framework.Assert.AreEqual("2 in", svalue);
            svalue = converter.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, null, typeof(string)).ToString();
        }
    }
}
