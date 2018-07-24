using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test
{
	[TestFixture]
	class AssemblyExtensionTest
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
