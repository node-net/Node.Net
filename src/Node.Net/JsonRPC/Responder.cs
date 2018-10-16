using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.JsonRPC
{
	public sealed class Responder : IResponder
	{
		public Stream Respond(Stream request)
		{
			var _request = new Request(new Reader().Read<IDictionary>(request));
			var response = Respond(_request);
			byte[] response_bytes = response.GetBytes();
			return new MemoryStream(response_bytes);
		}

		public Response Respond(Request request)
		{
			if (Methods.ContainsKey(request.Method))
			{
				try
				{
					var method_responder = Methods[request.Method];
					return method_responder.Respond(request);
				}
				catch (Exception e)
				{
					return new Response(
						new Error(11, e.ToString()));
				}
			}
			return new Response(
				new Error(1, "unrecognized method"), request.Id
			);
		}

		public Dictionary<string, IResponder> Methods { get; set; } = new Dictionary<string, IResponder>();
	}
}