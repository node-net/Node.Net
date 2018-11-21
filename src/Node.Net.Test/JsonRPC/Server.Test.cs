using NUnit.Framework;
using System.Net;

namespace Node.Net.JsonRPC
{
	[TestFixture]
	internal class ServerTest
	{
		[Test,Explicit]
		public void Default_Usage()
		{
			var responder = ResponderTest.GetTestResponder();
			using (var server = new Server(responder.Respond))
			{
				var port = server.Port;
				server.Start();

				var uri = server.Uri;
				Assert.AreEqual($"http://localhost:{port}/", uri.ToString());
				using (var client = new WebClient())
				{
					var request = new Request("say_hello");
					var json_response = client.UploadString(uri.ToString(), request.ToJson());
					Assert.NotNull(json_response, nameof(json_response));
					Assert.True(json_response.Contains("hello"));
				}
				var jsonRpcClient = new Client(server.Uri.ToString());
				var response = jsonRpcClient.Respond(new Request("say_hello"));
				Assert.NotNull(response, nameof(response));
				Assert.AreEqual("hello", response.Result.ToString());
				server.Stop();
			}
		}
	}
}