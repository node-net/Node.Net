using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.JsonRPC
{
	public sealed class Responder
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
			if(MethodResponseFunctions.ContainsKey(request.Method))
			{
				try
				{
					var result = MethodResponseFunctions[request.Method](request.Parameters);
					return new Response(result, request.Id);
				}
				catch(Exception e)
				{
					return new Response(
						new Error(11, e.ToString()));
				}
			}
			return new Response(
				new Error(1,"unrecognized method"),request.Id
			);
		}

		private Dictionary<string, Func<IDictionary, object>> methodResponseFunctions
			= new Dictionary<string, Func<IDictionary, object>>();

		public Dictionary<string, Func<IDictionary, object>> MethodResponseFunctions { get => methodResponseFunctions; set => methodResponseFunctions = value; }
	}
}
