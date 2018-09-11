﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Node.Net.Service
{
	public enum Protocol { HTTP, HTTPS };

	public sealed class WebServer : IDisposable
	{
		#region Construction
		/*
		public WebServer()
		{
			_port = GetNextAvailablePort(5000);
			_protocol = Protocol.HTTP;
			_listener = new HttpListener();
			_listener.Prefixes.Add(Uri.ToString());
			_contextAction = new WebResponder().Respond;
		}*/
		/*
		public WebServer(Protocol protocol) : this()
		{
			//_port = GetNextAvailablePort(5000);
			_protocol = protocol;
			//_listener = new HttpListener();
			//_listener.Prefixes.Add(Uri.ToString());
			//_contextAction = new WebResponder().Respond;
		}*/
		public WebServer(Protocol protocol,int port)
		{
			_port = GetNextAvailablePort(port);
			_protocol = Protocol.HTTP;
			_listener = new HttpListener();
			_listener.Prefixes.Add(Uri.ToString());
			_contextAction = new WebResponder().Respond;
		}
		public WebServer(Protocol protocol,int port,Action<HttpListenerContext> action)
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
				_listener.Close();
			}
		}

		#endregion Destruction

		public Protocol Protocol { get { return _protocol; } }
		private readonly Protocol _protocol;
		public int Port { get { return _port; } }
		private readonly int _port;

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

		private readonly Action<HttpListenerContext> _contextAction;
		private readonly HttpListener _listener;

		public void Start()
		{
			_listener.Start();
			ThreadPool.QueueUserWorkItem((o) =>
			{
				while (_listener.IsListening)
				{
					ThreadPool.QueueUserWorkItem((c) =>
					{
						_contextAction(c as HttpListenerContext);
					}, _listener.GetContext());
				}
			});
		}

		public void Stop()
		{
			_listener.Stop();
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