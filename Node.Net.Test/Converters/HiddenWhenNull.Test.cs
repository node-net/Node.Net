#if IS_WINDOWS
using NUnit.Framework;
using System;

namespace Node.Net.Test.Converters
{
    [TestFixture]
    internal class HiddenWhenNullTest
    {
        [Test]
        public void Usage()
        {
            Net.Converters.HiddenWhenNull c = new Node.Net.Converters.HiddenWhenNull();
            c.Convert(null, typeof(object), null, null);
            c.Convert(1, typeof(object), null, null);
            Assert.Throws<NotImplementedException>(() => c.ConvertBack(null, typeof(object), null, null));
        }
    }
}
#endif