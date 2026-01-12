extern alias NodeNet;
using NUnit.Framework;
using NodeNet::Node.Net.JsonRPC;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
#if IS_WINDOWS
using System.Security.Principal;
#endif

namespace Node.Net.JsonRPC
{
    [TestFixture]
    internal class ServerTest
    {
		[Test, Explicit]
		public void Default_Usage()
		{
			Responder responder = ResponderTest.GetTestResponder();
			using Server server = new Server(responder.Respond);
			int port = server.Port;
			server.Start();

			Uri uri = server.Uri;
			Assert.That(uri.ToString(), Is.EqualTo($"http://localhost:{port}/"));
			using (HttpClient client = new HttpClient())
			{
				Request request = new Request("say_hello");
				var content = new StringContent(request.ToJson(), Encoding.UTF8, "application/json");
				var httpResponse = client.PostAsync(uri.ToString(), content).Result;
				string json_response = httpResponse.Content.ReadAsStringAsync().Result;
				Assert.That(json_response.Contains("hello"),Is.True);
			}
			Client jsonRpcClient = new Client(server.Uri.ToString());
			Response response = jsonRpcClient.Respond(new Request("say_hello"));
			Assert.That(response.Result.ToString(), Is.EqualTo("hello"));
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
        public void Default_Usage_Https()
        {
            if (IsAdministrator())
            {
#pragma warning disable RCS1163 // Unused parameter.
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
#pragma warning restore RCS1163 // Unused parameter.
                int port = NodeNet::Node.Net.Service.WebServer.GetNextAvailablePort(5000);
                HttpListener listener = new HttpListener();
                string uri = $"https://{Environment.MachineName}:{port}/";
                listener.Prefixes.Add(uri);

				Responder responder = ResponderTest.GetTestResponder();
				using Server server = new Server(listener, responder.Respond);
				//var port = server.Port;
				server.Start();
				Assert.That(uri.ToString(), Is.EqualTo($"https://{Environment.MachineName}:{port}/"));
				using (HttpClient client = new HttpClient())
				{
					Request request = new Request("say_hello");
					var content = new StringContent(request.ToJson(), Encoding.UTF8, "application/json");
					var httpResponse = client.PostAsync(uri.ToString(), content).Result;
					string json_response = httpResponse.Content.ReadAsStringAsync().Result;
					Assert.That(json_response.Contains("hello"),Is.True);
				}
				Client jsonRpcClient = new Client(server.Uri.ToString());
				Response response = jsonRpcClient.Respond(new Request("say_hello"));
				Assert.That(response.Result.ToString(), Is.EqualTo("hello"));
				server.Stop();
			}
        }
    }
}