using Node.Net.Service;
using System.Threading;
using System.Threading.Tasks;

namespace Node.Net.Service
{
    internal class WebServerTest
    {
        [Test]
        public async Task Default_Usage()
        {
			using WebServer server = new WebServer(Protocol.HTTP, 5000);
			int port = server.Port;
			server.Start();
			await Assert.That(server.Listener).IsNotNull();

			System.Uri uri = server.Uri;
			await Assert.That(uri.ToString()).IsEqualTo($"http://localhost:{port}/");
			using (WebClient client = new WebClient())
			{
				string response = client.DownloadString(uri.ToString());
				await Assert.That(response.Contains("html")).IsTrue();

				response = client.UploadString(uri.ToString(), "please post");
				await Assert.That(response.Contains("thanks")).IsTrue();
			}
			// Give background threads time to complete before stopping
			await Task.Delay(100);
			server.Stop();
			// Give time for cleanup
			await Task.Delay(100);
		}

        //[Test] - Commented out test
        public async Task Run_30_seconds()
        {
			using WebServer server = new WebServer(Protocol.HTTP, 5000);
			int port = server.Port;
			server.Start();

			System.Uri uri = server.Uri;
			await Assert.That( uri.ToString()).IsEqualTo($"http://localhost:{port}/");

			Thread.Sleep(30000);
			server.Stop();
		}
    }
}