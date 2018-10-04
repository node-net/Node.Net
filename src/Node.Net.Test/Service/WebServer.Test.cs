using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.Service
{
	[TestFixture,Category(nameof(WebServer))]
	internal class WebServerTest
	{
		[Test]
		public void Default_Usage()
		{
			using (var server = new WebServer(Protocol.HTTP,5000))
			{
				var port = server.Port;
				server.Start();

				var uri = server.Uri;
				Assert.AreEqual($"http://localhost:{port}/", uri.ToString());
				using (var client = new WebClient())
				{
					var response = client.DownloadString(uri.ToString());
					Assert.True(response.Contains("html"));

					response = client.UploadString(uri.ToString(), "please post");
					Assert.True(response.Contains("thanks"));
				}
				server.Stop();
			}
		}
	}
}
