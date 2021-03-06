﻿using System.Collections;
using System.IO;
using System.Net;
using System.Text;

namespace Node.Net.JsonRPC
{
    public sealed class Client
    {
        public Client(string url)
        {
            _url = url;
        }

        private readonly string _url;

        public string Respond(string request)
        {
            MemoryStream? srequest = new MemoryStream(Encoding.UTF8.GetBytes(request));
            return new StreamReader(Respond(srequest)).ReadToEnd();
        }

        public Stream Respond(Stream request)
        {
            Request? _request = new Request(new Reader().Read<IDictionary>(request));
            Response? response = Respond(_request);
            byte[] response_bytes = response.GetBytes();
            return new MemoryStream(response_bytes);
        }

        public Response Respond(Request request)
        {
			using WebClient? webClient = new WebClient();
			return new Response(webClient.UploadData(_url, request.GetBytes()));
		}
    }
}