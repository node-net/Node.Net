using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal class AssemblyExtensionTest
    {
        [Test]
        public void FindManifestResourceStream()
        {
            global::System.Reflection.Assembly assembly = typeof(AssemblyExtensionTest).Assembly;
            Assert.That(assembly.FindManifestResourceStream("?"),Is.Null);
            Assert.That(assembly.FindManifestResourceStream("Object.Coverage.json"),Is.Not.Null);
            Assert.That(assembly.FindManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json"),Is.Not.Null);
        }
    }
}