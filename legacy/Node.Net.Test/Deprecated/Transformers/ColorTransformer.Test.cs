using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net.Deprecated.Transformers
{
    [TestFixture,Category("Transformers.ColorTransformer")]
    class ColorTransformerTest
    {
        [Test]
        public void ColorTransformer_Usage()
        {
            var t = new ColorTransformer();
            Assert.AreEqual(Colors.Blue,(Color)t.Transform("Blue"));
            Assert.AreEqual(Colors.Transparent, (Color)t.Transform("Transparent"));
            var rgb = (Color)t.Transform("255,0,0");
            Assert.AreEqual(255, rgb.A);
            Assert.AreEqual(255, rgb.R);
            Assert.AreEqual(0, rgb.G);
            Assert.AreEqual(0, rgb.B);

            var argb = (Color)t.Transform("100,50,20,10");
            Assert.AreEqual(100, argb.A);
            Assert.AreEqual(50, argb.R);
            Assert.AreEqual(20, argb.G);
            Assert.AreEqual(10, argb.B);
        }
    }
}
