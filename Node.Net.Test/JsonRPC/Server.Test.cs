using NUnit.Framework;
using System;
using System.Net;
using System.Security.Principal;

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
			using (WebClient client = new WebClient())
			{
				Request request = new Request("say_hello");
				string json_response = client.UploadString(uri.ToString(), request.ToJson());
				Assert.That(json_response.Contains("hello"),Is.True);
			}
			Client jsonRpcClient = new Client(server.Uri.ToString());
			Response response = jsonRpcClient.Respond(new Request("say_hello"));
			Assert.That(response.Result.ToString(), Is.EqualTo("hello"));
			server.Stop();
		}

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // https://stackoverflow.com/questions/11403333/httplistener-with-https-support
        //[Test]
        public void Default_Usage_Https()
        {
            if (IsAdministrator())
            {
#pragma warning disable RCS1163 // Unused parameter.
                ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
#pragma warning restore RCS1163 // Unused parameter.
                int port = Service.WebServer.GetNextAvailablePort(5000);
                HttpListener listener = new HttpListener();
                string uri = $"https://{Environment.MachineName}:{port}/";
                listener.Prefixes.Add(uri);

                Responder responder = ResponderTest.GetTestResponder();
				using Server server = new Server(listener, responder.Respond);
				//var port = server.Port;
				server.Start();
				Assert.That(uri.ToString(), Is.EqualTo($"https://{Environment.MachineName}:{port}/"));
				using (WebClient client = new WebClient())
				{
					Request request = new Request("say_hello");
					string json_response = client.UploadString(uri.ToString(), request.ToJson());
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