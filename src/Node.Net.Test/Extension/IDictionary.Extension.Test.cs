using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	class IDictionaryExtensionTest
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
	}
}
