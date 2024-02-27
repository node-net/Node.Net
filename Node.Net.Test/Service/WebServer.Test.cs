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
			using WebServer server = new WebServer(Protocol.HTTP, 5000);
			int port = server.Port;
			server.Start();
			Assert.That(server.Listener,Is.Not.Null, "server.Listener");

			System.Uri uri = server.Uri;
			Assert.That(uri.ToString(),Is.EqualTo($"http://localhost:{port}/"));
			using (WebClient client = new WebClient())
			{
				string response = client.DownloadString(uri.ToString());
				Assert.That(response.Contains("html"),Is.True);

				response = client.UploadString(uri.ToString(), "please post");
				Assert.That(response.Contains("thanks"), Is.True);
			}
			server.Stop();
		}

        //[Test, Explicit]
        public void Run_30_seconds()
        {
			using WebServer server = new WebServer(Protocol.HTTP, 5000);
			int port = server.Port;
			server.Start();

			System.Uri uri = server.Uri;
			Assert.That( uri.ToString(),Is.EqualTo($"http://localhost:{port}/"));

			Thread.Sleep(30000);
			server.Stop();
		}
    }
}