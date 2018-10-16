using NUnit.Framework;
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
				Methods = new Dictionary<string, IResponder>
				{
					{"sayHello", new JsonRPC.Function<string>(SayHello) }
				}
			};
		}

		public static string SayHello()
		{
			return "hello";
		}

		/*
		public static object SayHelloResponder(IDictionary parameters)
		{
			return "hello";
		}*/
	}
}