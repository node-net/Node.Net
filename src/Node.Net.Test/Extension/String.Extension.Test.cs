using NUnit.Framework;
using System.IO;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	internal class StringExtensionTest
	{
		[Test]
		public void GetStream()
		{
			var text = new StreamReader("test".GetStream()).ReadToEnd();
			Assert.AreEqual("test", text);

			text = new StreamReader("Object.Coverage.json".GetStream()).ReadToEnd();
			Assert.True(text.Contains("array_empty"), "array_empty not found in Object.Coverage.json");
		}
	}
}