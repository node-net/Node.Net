using System;
using System.Collections;
using System.IO;
using System.Net;

namespace Node.Net.JsonRPC
{
	public sealed class Server : IDisposable
	{
		public Server(Func<Request, Response> responder)
		{
			_webServer = new Service.WebServer(Service.Protocol.HTTP,5000,ContextAction);
			_responder = responder;
		}

		~Server()
		{
			Dispose(false);
		}
		#region Dispose
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_webServer != null)
				{
					_webServer.Dispose();
				}
			}
		}
		#endregion

		private void ContextAction(HttpListenerContext context)
		{
			try
			{
				var request_dictionary = new Internal.JsonReader().Read(context.Request.InputStream) as IDictionary;
				var request = new Request(request_dictionary);
				try
				{
					using (var sw = new StreamWriter(context.Response.OutputStream))
					{
						sw.Write(_responder(request).ToJson());
					}
				}
				catch (Exception e)
				{
					using (var sw = new StreamWriter(context.Response.OutputStream))
					{
						sw.Write(new Response(new Error(-32000, e.ToString()), 0).ToJson());
					}
				}
			}
			catch (Exception e)
			{
				using (var sw = new StreamWriter(context.Response.OutputStream))
				{
					sw.Write(new Response(new Error(-32600, e.ToString()), 0).ToJson());
				}
			}
		}

		private Service.WebServer _webServer;
		private readonly Func<Request, Response> _responder;
		public int Port { get { return _webServer.Port; } }
		public Uri Uri { get { return _webServer.Uri; } }
		public void Start() { _webServer.Start(); }
		public void Stop() { _webServer.Stop(); }
	}
}