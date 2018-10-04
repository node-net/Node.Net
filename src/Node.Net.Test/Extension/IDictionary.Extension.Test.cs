using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	internal class IDictionaryExtensionTest
	{
		[Test]
		public void GetRotations()
		{
			var rotations = new Dictionary<string, string>().GetRotations();
			Assert.AreEqual(0, rotations.Z, "rotations.Z");
		}

		[Test]
		public void Find()
		{
			var assembly = typeof(IDictionaryExtensionTest).Assembly;
			var data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
			Assert.NotNull(data, nameof(data));
			//data.DeepUpdateParents();
			var a = data.Find<IDictionary>("objectA");
			Assert.NotNull(a, nameof(a));
		}
		private bool Filter(object v) { return true; }
		[Test]
		public void Collect()
		{
			var assembly = typeof(IDictionaryExtensionTest).Assembly;
			var data = new Reader().Read<IDictionary>(assembly.FindManifestResourceStream("Object.Coverage.json"));
			Assert.NotNull(data, nameof(data));
			data.Collect(typeof(IDictionary));
			data.Collect<IDictionary>(Filter);
		}
	}
}