using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Node.Net;
using NUnit.Framework;

namespace Node.Net.Tests.Extension
{
	[TestFixture,Category("Assembly")]
	class AssemblyExtensionTest
	{
		[Test]
		public void GetStream()
		{
			Assert.IsNull(typeof(AssemblyExtensionTest).Assembly.GetStream("not.there"));
			Assert.NotNull(typeof(AssemblyExtensionTest).Assembly.GetStream("Object.Sample.json"),"Object.Sample.json");
		}
		[Test]
		public void GetManifestResourceNames()
		{
			var names =new List<string>( typeof(AssemblyExtensionTest).Assembly.GetManifestResourceNames(".json"));
			Assert.True(names.Contains("Node.Net.Test.Resources.Object.Sample.json"));
		}
		
		[Test]
		public void GetNameTypeDictionary()
		{
			var dictionary = typeof(AssemblyExtensionTest).Assembly.GetNameTypeDictionary();
			Assert.True(dictionary.ContainsKey("Bar"));
		}
		[Test]
		public void GetFullNameTypeDictionary()
		{
			var dictionary = typeof(AssemblyExtensionTest).Assembly.GetFullNameTypeDictionary();
			Assert.True(dictionary.ContainsKey("Node.Net.Bar"));
		}
	}
}
