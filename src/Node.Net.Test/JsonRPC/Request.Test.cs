using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.JsonRPC
{
	[TestFixture,Category(nameof(Request))]
	static class RequestTest
	{
		[Test]
		public static void StreamConstructor()
		{
			var request = new Request("SayHello");
			request.Parameters.Add("name", "test");

			var rs = request.ToStream();
			Request request1 = new Request(rs);
			
			var name = request1.Parameters["name"];
			Assert.AreEqual("test", name);
		}
	}
}
