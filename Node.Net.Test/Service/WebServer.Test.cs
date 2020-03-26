using NUnit.Framework;
using System.Threading;

namespace Node.Net.Service
{
    [TestFixture, Category(nameof(WebServer))]
    internal class WebServerTest
    {
        [Test, Explicit]
        public void Default_Usage()
        {
            using (WebServer server = new WebServer(Protocol.HTTP, 5000))
            {
                int port = server.Port;
                server.Start();
                Assert.NotNull(server.Listener, "server.Listener");

                System.Uri uri = server.Uri;
                Assert.AreEqual($"http://localhost:{port}/", uri.ToString());
                using (WebClient client = new WebClient())
                {
                    string response = client.DownloadString(uri.ToString());
                    Assert.True(response.Contains("html"));

                    response = client.UploadString(uri.ToString(), "please post");
                    Assert.True(response.Contains("thanks"));
                }
                server.Stop();
            }
        }

        //[Test, Explicit]
        public void Run_30_seconds()
        {
            using (WebServer server = new WebServer(Protocol.HTTP, 5000))
            {
                int port = server.Port;
                server.Start();

                System.Uri uri = server.Uri;
                Assert.AreEqual($"http://localhost:{port}/", uri.ToString());

                Thread.Sleep(30000);
                server.Stop();
            }
        }
    }
}