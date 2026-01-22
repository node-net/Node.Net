using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
#if IS_WINDOWS
using System.Security.Principal;
#endif
using Node.Net.JsonRPC;

namespace Node.Net.JsonRPC
{
    internal class ServerTest
    {
		[Test, Explicit]
		public async Task Default_Usage()
		{
			Responder responder = ResponderTest.GetTestResponder();
			using Server server = new Server(responder.Respond);
			int port = server.Port;
			server.Start();

			Uri uri = server.Uri;
			await Assert.That(uri.ToString()).IsEqualTo($"http://localhost:{port}/");
			using (HttpClient client = new HttpClient())
			{
				Request request = new Request("say_hello");
				var content = new StringContent(request.ToJson(), Encoding.UTF8, "application/json");
				var httpResponse = client.PostAsync(uri.ToString(), content).Result;
				string json_response = httpResponse.Content.ReadAsStringAsync().Result;
				await Assert.That(json_response.Contains("hello")).IsTrue();
			}
			Client jsonRpcClient = new Client(server.Uri.ToString());
			Response response = jsonRpcClient.Respond(new Request("say_hello"));
			await Assert.That(response.Result.ToString()).IsEqualTo("hello");
			server.Stop();
		}

#if IS_WINDOWS
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
#else
        public static bool IsAdministrator()
        {
            return false; // Not applicable on non-Windows platforms
        }
#endif

        // https://stackoverflow.com/questions/11403333/httplistener-with-https-support
        //[Test]
        private async Task Default_Usage_Https()
        {
            if (IsAdministrator())
            {
#pragma warning disable RCS1163 // Unused parameter.
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
#pragma warning restore RCS1163 // Unused parameter.
                int port = Node.Net.Service.WebServer.GetNextAvailablePort(5000);
                HttpListener listener = new HttpListener();
                string uri = $"https://{Environment.MachineName}:{port}/";
                listener.Prefixes.Add(uri);

				Responder responder = ResponderTest.GetTestResponder();
				using Server server = new Server(listener, responder.Respond);
				//var port = server.Port;
				server.Start();
				await Assert.That(uri.ToString()).IsEqualTo($"https://{Environment.MachineName}:{port}/");
				using (HttpClient client = new HttpClient())
				{
					Request request = new Request("say_hello");
					var content = new StringContent(request.ToJson(), Encoding.UTF8, "application/json");
					var httpResponse = client.PostAsync(uri.ToString(), content).Result;
					string json_response = httpResponse.Content.ReadAsStringAsync().Result;
					await Assert.That(json_response.Contains("hello")).IsTrue();
				}
				Client jsonRpcClient = new Client(server.Uri.ToString());
				Response response = jsonRpcClient.Respond(new Request("say_hello"));
				await Assert.That(response.Result.ToString()).IsEqualTo("hello");
				server.Stop();
			}
            await Task.CompletedTask;
        }
    }
}