using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Test.Extension
{
	[TestFixture]
	class StringExtensionTest
	{
		[Test]
		public void GetStream()
		{
			var text = new StreamReader("test".GetStream()).ReadToEnd();
			Assert.AreEqual("test", text);

			text = new StreamReader("Object.Coverage.json".GetStream()).ReadToEnd();
			Assert.True(text.Contains("array_empty"),"array_empty not found in Object.Coverage.json");
		}
	}
}
