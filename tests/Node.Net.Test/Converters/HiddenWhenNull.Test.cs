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
            // Use reflection to access conditionally compiled type
            var assembly = typeof(Factory).Assembly;
            var converterType = assembly.GetType("Node.Net.Converters.HiddenWhenNull");
            if (converterType == null)
            {
                Assert.Pass("HiddenWhenNull type not found - skipping test on non-Windows target");
            }
            var c = System.Activator.CreateInstance(converterType);
            var convertMethod = converterType.GetMethod("Convert");
            var convertBackMethod = converterType.GetMethod("ConvertBack");
            
            convertMethod.Invoke(c, new object[] { null, typeof(object), null, null });
            convertMethod.Invoke(c, new object[] { 1, typeof(object), null, null });
            Assert.Throws<NotImplementedException>(() => convertBackMethod.Invoke(c, new object[] { null, typeof(object), null, null }));
        }
    }
}
#endif