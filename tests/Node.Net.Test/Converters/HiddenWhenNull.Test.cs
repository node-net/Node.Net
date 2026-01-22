#if IS_WINDOWS
using System;
using System.Threading.Tasks;

namespace Node.Net.Test.Converters
{
    internal class HiddenWhenNullTest
    {
        [Test]
        public async Task Usage()
        {
            // Use reflection to access conditionally compiled type
            var assembly = typeof(Factory).Assembly;
            var converterType = assembly.GetType("Node.Net.Converters.HiddenWhenNull");
            if (converterType == null)
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }
            var c = System.Activator.CreateInstance(converterType);
            var convertMethod = converterType.GetMethod("Convert");
            var convertBackMethod = converterType.GetMethod("ConvertBack");
            
            convertMethod.Invoke(c, new object[] { null, typeof(object), null, null });
            convertMethod.Invoke(c, new object[] { 1, typeof(object), null, null });
            await Assert.That(() => convertBackMethod.Invoke(c, new object[] { null, typeof(object), null, null })).Throws<NotImplementedException>();
        }
    }
}
#endif