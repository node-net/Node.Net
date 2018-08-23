using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Node.Net.JsonRPC
{
	public sealed class Server : IDisposable
	{
		public Server(Func<Stream, Stream> responder, string prefix)
		{
			_responderFunction = responder;
			_listener = new HttpListener();
			_listener.Prefixes.Add(prefix);
		}

		~Server()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_listener.Close();
			}
		}

		private readonly Func<Stream, Stream> _responderFunction;
		//private readonly Func<Request, Response> _responseFunction;
		private readonly HttpListener _listener;

		private Request GetRequest(HttpListenerRequest httpRequest)
		{
			if (httpRequest.HttpMethod == "POST")
			{
				var data = new Reader().Read<IDictionary>(httpRequest.InputStream);
				var request = new Request(data);
				return request;
			}
			return new Request("Http.Get", 0);
		}

		public void Start()
		{
			_listener.Start();
			ThreadPool.QueueUserWorkItem((o) =>
			{
				Console.WriteLine("Webserver running...");
				try
				{
					while (_listener.IsListening)
					{
						ThreadPool.QueueUserWorkItem((c) =>
						{
							var context = c as HttpListenerContext;
							var id = 0;
							try
							{
								Stream requestStream = null;
								if (context.Request.HttpMethod == "POST")
								{
									requestStream = context.Request.InputStream;
									var responseStream = _responderFunction(requestStream);
									using (var sr = new StreamReader(responseStream))
									{
										var text = sr.ReadToEnd();
										byte[] response_bytes = Encoding.UTF8.GetBytes(text);
										context.Response.ContentLength64 = response_bytes.Length;
										context.Response.OutputStream.Write(response_bytes, 0, response_bytes.Length);
									}
								}
							}
							catch (Exception ex)
							{
								var response = new Response(new Error(
									(int)(ErrorCode.InternalError),
									$"exception of type {ex.GetType().FullName} occurred.",
									ex.ToString()), id);
								byte[] response_bytes = response.GetBytes();
								context.Response.ContentLength64 = response_bytes.Length;
								context.Response.OutputStream.Write(response_bytes, 0, response_bytes.Length);
							} // suppress any exceptions
							finally
							{
								// always close the stream
								context.Response.OutputStream.Close();
							}
						}, _listener.GetContext());
					}
				}
				catch { } // suppress any exceptions
			});
		}

		public void Stop()
		{
			_listener.Stop();
		}
	}
}