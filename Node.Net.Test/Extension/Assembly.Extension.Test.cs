using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal class AssemblyExtensionTest
    {
        [Test]
        public void FindManifestResourceStream()
        {
            System.Reflection.Assembly assembly = typeof(AssemblyExtensionTest).Assembly;
            Assert.IsNull(assembly.FindManifestResourceStream("?"));
            Assert.NotNull(assembly.FindManifestResourceStream("Object.Coverage.json"));
            Assert.NotNull(assembly.FindManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json"));
        }
    }
}