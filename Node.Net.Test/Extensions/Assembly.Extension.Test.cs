using NUnit.Framework;

namespace Node.Net.Extensions.Test
{
    [TestFixture]
    class AssemblyExtensionTest
    {
        [Test]
        public void AssemblyExtension_GetStream()
        {
            Assert.NotNull(AssemblyExtension.GetStream(typeof(AssemblyExtensionTest).Assembly, "States.json"));
            Assert.IsNull(AssemblyExtension.GetStream(typeof(AssemblyExtensionTest).Assembly, "bogus"));
        }
    }
}
