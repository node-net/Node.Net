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
		

		//public static string[] GetManifestResourceNames(this Assembly assembly, string name)
		

		//public static Dictionary<string, Type> GetNameTypeDictionary(this Assembly assembly)
		

		//public static Dictionary<string, Type> GetFullNameTypeDictionary(this Assembly assembly)
	}
}
