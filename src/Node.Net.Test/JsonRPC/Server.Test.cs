using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Node.Net.JsonRPC
{
	[TestFixture]
	class ServerTest
	{

		[Test]
		public void ServerUsage()
		{
			var identity = WindowsIdentity.GetCurrent();
			var principal = new WindowsPrincipal(identity);
			if (principal.IsInRole(WindowsBuiltInRole.Administrator))
			{

				
				var machine = System.Environment.MachineName;
				var url = $"http://{machine}:9999/JsonRPC/";
				using (var server = new Server(ResponderTest.GetTestResponder().Respond, url))
				{
					server.Start();

					var client = new Client(url);
					var response = client.Respond(new Request("sayHello"));
					Assert.AreEqual("hello", response.Result.ToString());
				}
			}
		}
	}
}
