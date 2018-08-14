using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	internal class AssemblyExtensionTest
	{
		[Test]
		public void FindManifestResourceStream()
		{
			var assembly = typeof(AssemblyExtensionTest).Assembly;
			Assert.IsNull(assembly.FindManifestResourceStream("?"));
			Assert.NotNull(assembly.FindManifestResourceStream("Object.Coverage.json"));
		}
	}
}