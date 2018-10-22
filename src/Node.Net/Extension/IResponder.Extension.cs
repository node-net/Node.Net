﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
	public static class IResponderExtension
	{
		public static Stream Respond(this JsonRPC.IResponder responder,Stream request)
		{
			var _request = new JsonRPC.Request(new Reader().Read<IDictionary>(request));
			var response = responder.Respond(_request);
			byte[] response_bytes = response.GetBytes();
			return new MemoryStream(response_bytes);
		}

		public static string Respond(this JsonRPC.IResponder responder, string request)
		{
			using (var memory = new MemoryStream(Encoding.UTF8.GetBytes(request)))
			{
				return new StreamReader(responder.Respond(memory)).ReadToEnd().Trim();
			}
		}
	}
}