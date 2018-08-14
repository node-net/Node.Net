using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Node.Net.JsonRPC
{
	[TestFixture]
	internal class ResponderTest
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