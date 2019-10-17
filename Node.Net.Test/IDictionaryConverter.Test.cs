using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net
{
    [TestFixture]
    internal class IDictionaryConverterTest
    {
        [Test]
        public void Convert()
        {
            var data = new Dictionary<string, object>
            {
                {"Type","Widget" },
                {"Name","widget" }
            };

            var converter = new IDictionaryConverter
            {
                ConversionTypeNames = new Dictionary<string, Type>
                {
                    {"Widget",typeof(Widget) }
                }
            };

            var w = converter.Convert(data) as Widget;
            Assert.NotNull(w, nameof(w));

            data = new Dictionary<string, object>();
            Assert.AreSame(data, converter.Convert(data));
        }
    }
}
