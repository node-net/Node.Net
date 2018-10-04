using NUnit.Framework;
using System.Net;
using System.Security.Principal;

namespace Node.Net.JsonRPC
{
	[TestFixture]
	internal class ServerTest
	{
		public Response Respond(Request request)
		{
			if(request.Method == "sayHello")
			{
				return new Response("hello", request.Id);
			}
			return new Response(new Error(-32601,"Method not found"),request.Id);
		}

		[Test]
		public void Default_Usage()
		{
			using (var server = new Server(Respond))
			{
				var port = server.Port;
				server.Start();

				var uri = server.Uri;
				Assert.AreEqual($"http://localhost:{port}/", uri.ToString());
				using (var client = new WebClient())
				{
					var request = new Request("sayHello");
					var json_response = client.UploadString(uri.ToString(), request.ToJson());
					Assert.NotNull(json_response, nameof(json_response));
					Assert.True(json_response.Contains("hello"));

				}
				var jsonRpcClient = new Client(server.Uri.ToString());
				var response = jsonRpcClient.Respond(new Request("sayHello"));
				Assert.NotNull(response, nameof(response));
				Assert.AreEqual("hello", response.Result.ToString());
				server.Stop();
			}
		}
	}
}