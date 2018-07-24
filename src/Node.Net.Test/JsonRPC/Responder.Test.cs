using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.JsonRPC
{
	[TestFixture]
	class ResponderTest
	{
		[Test]
		public void Respond()
		{
			var responder = GetTestResponder();
			var response = responder.Respond(new Request("sayHello"));
			Assert.AreEqual("hello", response.Result.ToString());
		}
		public static Responder GetTestResponder()
		{
			return new Responder
			{
				MethodResponseFunctions = new Dictionary<string, Func<IDictionary, object>>
					{
						{"sayHello", SayHelloResponder }
					}
			};
		}
		public static object SayHelloResponder(IDictionary parameters)
		{
			return "hello";
		}
	}
	
}
