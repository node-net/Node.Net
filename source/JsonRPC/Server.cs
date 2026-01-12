using System;
using System.Collections;
using System.IO;
using System.Net;

namespace Node.Net.JsonRPC
{
    internal class StreamResponder
    {
        public StreamResponder(Func<Stream, Stream> responder)
        {
            _responder = responder;
        }

        public Response Respond(Request request)
        {
            Stream? responseStream = _responder(request.ToStream());
            return new Response(responseStream);
        }

        private readonly Func<Stream, Stream> _responder;
    }

    public sealed class Server : IDisposable
    {
        public Server(Func<string, string> responder)
        {
            _webServer = new Service.WebServer(Service.Protocol.HTTP, 5000, ContextAction);
            _responder = responder;
        }

        public Server(HttpListener listener, Func<string, string> responder)
        {
            _webServer = new Service.WebServer(listener, ContextAction);
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
                _webServer?.Dispose();
            }
        }

        #endregion Dispose

        private void ContextAction(HttpListenerContext context)
        {
            try
            {
                IDictionary? request_dictionary = new Internal.JsonReader().Read(context.Request.InputStream) as IDictionary;
#pragma warning disable CS8604 // Possible null reference argument.
                Request? request = new Request(request_dictionary);
#pragma warning restore CS8604 // Possible null reference argument.
                try
                {
					using StreamWriter? sw = new StreamWriter(context.Response.OutputStream);
					sw.Write(_responder(request.ToJson()));
				}
                catch (Exception e)
                {
					using StreamWriter? sw = new StreamWriter(context.Response.OutputStream);
					sw.Write(new Response(new Error(-32000, e.ToString()), 0).ToJson());
				}
            }
            catch (Exception e)
            {
				using StreamWriter? sw = new StreamWriter(context.Response.OutputStream);
				sw.Write(new Response(new Error(-32600, e.ToString()), 0).ToJson());
			}
        }

        private readonly Service.WebServer _webServer;
        private readonly Func<string, string> _responder;
        public int Port { get { return _webServer.Port; } }

        public Uri Uri { get { return _webServer.Uri; } }

        public void Start()
        {
            _webServer.Start();
        }

        public void Stop()
        {
            _webServer.Stop();
        }
    }
}