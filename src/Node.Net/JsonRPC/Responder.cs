using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.JsonRPC
{
	public sealed class Responder : IResponder
	{
		public string Respond(string request) { return IResponderExtension.Respond(this, request); }
		public Stream Respond(Stream request) { return IResponderExtension.Respond(this, request); }
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
				new Error(1, $"unrecognized method '{request.Method}'"), request.Id
			);
		}

		public Dictionary<string, IResponder> Methods { get; set; } = new Dictionary<string, IResponder>();
	}
}