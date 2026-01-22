using System.Threading.Tasks;
using Node.Net; // Extension methods are in Node.Net namespace

namespace Node.Net.Test
{
    internal class AssemblyExtensionTest
    {
        [Test]
        public async Task FindManifestResourceStream()
        {
            global::System.Reflection.Assembly assembly = typeof(AssemblyExtensionTest).Assembly;
            await Assert.That(assembly.FindManifestResourceStream("?")).IsNull();
            await Assert.That(assembly.FindManifestResourceStream("Object.Coverage.json")).IsNotNull();
            await Assert.That(assembly.FindManifestResourceStream("Node.Net.Test.Resources.Object.Coverage.json")).IsNotNull();
        }
    }
}