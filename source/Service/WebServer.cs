using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace Node.Net.Service
{
    public enum Protocol { HTTP, HTTPS };

    public sealed class WebServer : IDisposable
    {
        #region Construction

        public WebServer(HttpListener listener, Action<HttpListenerContext> action)
        {
            Listener = listener;
            _contextAction = action;
        }

        public WebServer(Protocol protocol, int port)
        {
            Port = GetNextAvailablePort(port);
            Protocol = Protocol.HTTP;
            Listener = new HttpListener();
            Listener.Prefixes.Add(Uri.ToString());
            _contextAction = new WebResponder().Respond;
        }

        public WebServer(Protocol protocol, int port, Action<HttpListenerContext> action)
            : this(protocol, port)
        {
            _contextAction = action;
        }

        #endregion Construction

        #region Destruction

        ~WebServer()
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
                Listener.Close();
            }
        }

        #endregion Destruction

        public Protocol Protocol { get; }
        public int Port { get; }

        public Uri Uri
        {
            get
            {
                switch (Protocol)
                {
                    case Protocol.HTTPS:
                        {
                            return new Uri($"https://localhost:{Port}/");
                        }
                }
                return new Uri($"http://localhost:{Port}/");
            }
        }

        public HttpListener Listener { get; }

        private readonly Action<HttpListenerContext> _contextAction;
        private readonly object _locker = new object();

        public bool Shutdown
        {
            get
            {
                lock (_locker)
                {
                    return _shutdown;
                }
            }
            set
            {
                lock (_locker)
                {
                    _shutdown = value;
                }
            }
        }

        private bool _shutdown = false;

        public void Start()
        {
            Listener.Start();
            ThreadPool.QueueUserWorkItem((_) =>
            {
                while (Listener.IsListening && !Shutdown)
                {
                    try
                    {
                        var context = Listener.GetContext();
#pragma warning disable CS8622 // Nullability of reference types in type of parameter 'context' doesn't match the target delegate
                        ThreadPool.QueueUserWorkItem(WorkItemCallback, context);
#pragma warning restore CS8622
                    }
                    catch { }
                }
            });
        }

        public void WorkItemCallback(object context)
        {
            if (context is HttpListenerContext listenerContext)
            {
                _contextAction(listenerContext);
            }
            //_contextAction(context as HttpListenerContext);
        }

        public void Stop()
        {
            Shutdown = true;
            Listener.Stop();
        }

        public static int GetNextAvailablePort(int starting_port)
        {
            int PortStartIndex = starting_port;
            int PortEndIndex = starting_port + 2000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort;
        }
    }
}