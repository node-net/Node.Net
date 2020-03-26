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
            using (Server server = new Server(responder.Respond))
            {
                int port = server.Port;
                server.Start();

                Uri uri = server.Uri;
                Assert.AreEqual($"http://localhost:{port}/", uri.ToString());
                using (WebClient client = new WebClient())
                {
                    Request request = new Request("say_hello");
                    string json_response = client.UploadString(uri.ToString(), request.ToJson());
                    Assert.NotNull(json_response, nameof(json_response));
                    Assert.True(json_response.Contains("hello"));
                }
                Client jsonRpcClient = new Client(server.Uri.ToString());
                Response response = jsonRpcClient.Respond(new Request("say_hello"));
                Assert.NotNull(response, nameof(response));
                Assert.AreEqual("hello", response.Result.ToString());
                server.Stop();
            }
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
                using (Server server = new Server(listener, responder.Respond))
                {
                    //var port = server.Port;
                    server.Start();
                    Assert.AreEqual($"https://{Environment.MachineName}:{port}/", uri.ToString());
                    using (WebClient client = new WebClient())
                    {
                        Request request = new Request("say_hello");
                        string json_response = client.UploadString(uri.ToString(), request.ToJson());
                        Assert.NotNull(json_response, nameof(json_response));
                        Assert.True(json_response.Contains("hello"));
                    }
                    Client jsonRpcClient = new Client(server.Uri.ToString());
                    Response response = jsonRpcClient.Respond(new Request("say_hello"));
                    Assert.NotNull(response, nameof(response));
                    Assert.AreEqual("hello", response.Result.ToString());
                    server.Stop();
                }
            }
        }
    }
}