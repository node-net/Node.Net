﻿using System.Collections;
using System.IO;
using System.Net;

namespace Node.Net.JsonRPC
{
	public sealed class Client
	{
		public Client(string url)
		{
			_url = url;
		}

		private string _url;

		public Stream Respond(Stream request)
		{
			var _request = new Request(new Reader().Read<IDictionary>(request));
			var response = Respond(_request);
			byte[] response_bytes = response.GetBytes();
			return new MemoryStream(response_bytes);
		}

		public Response Respond(Request request)
		{
			var webClient = new WebClient();
			return new Response(webClient.UploadData(_url, request.GetBytes()));
		}
	}
}