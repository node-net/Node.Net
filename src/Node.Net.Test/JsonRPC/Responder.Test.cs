using NUnit.Framework;
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
			var response = responder.Respond(new Request("say_hello"));
			Assert.AreEqual("hello", response.Result.ToString());

			var stream = typeof(ResponderTest).Assembly.GetManifestResourceStream(
				"Node.Net.Test.JsonRPC.Responder.Test.Data.json");
			var test_data = new Node.Net.Reader().Read<IDictionary>(stream);
			Assert.NotNull(test_data, nameof(test_data));

			foreach (string key in test_data.Keys)
			{
				if (key.Contains("_request"))
				{
					try
					{
						var request_data = (test_data[key] as IDictionary)?.ToJson();
						var response_text = responder.Respond(request_data);
						if (test_data.Contains(key.Replace("_request", "_response")))
						{
							var response_json = (test_data[key.Replace("_request", "_response")] as IDictionary)?.ToJson();
							Assert.AreEqual(response_json, response_text, key);
						}
					}
					catch (System.Exception e)
					{
						throw new System.InvalidOperationException(key, e);
					}
				}
			}
		}

		public static Responder GetTestResponder()
		{
			return new Responder
			{
				Methods = new Dictionary<string, IResponder>
				{
					{"say_hello", new JsonRPC.Function<string>(SayHello) },
					{"action3",new JsonRPC.Action<string,string,string>(Action3) },
					{"bad_action",new JsonRPC.Action(BadAction) },
					{ "add_multiply",new JsonRPC.Function<int,int,int,int>(AddMultiply,"a","b","c") }
				}
			};
		}

		public static string SayHello()
		{
			return "hello";
		}

		public static void Action3(string a, string b, string c)
		{
		}
		public static void BadAction()
		{
			throw new System.InvalidOperationException("BadAction");
		}
		public static int AddMultiply(int a,int b,int c) { return (a + b) * c; }
	}
}