using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Node.Net.Service
{
	public sealed class WebResponder
	{
		public WebResponder()
		{
			MethodResponseFunctions["GET"] = RespondGET;
			MethodResponseFunctions["POST"] = RespondPOST;
		}

        public Dictionary<string, Action<HttpListenerContext>> MethodResponseFunctions { get; } = new Dictionary<string, Action<HttpListenerContext>>();

        public void Respond(HttpListenerContext context)
		{
			try
			{
				if (MethodResponseFunctions.ContainsKey(context.Request.HttpMethod))
				{
					try
					{
						MethodResponseFunctions[context.Request.HttpMethod](context);
					}
					catch (Exception e)
					{
						context.Response.StatusCode = 500;
						var bytes = Encoding.UTF8.GetBytes(e.ToString());
						context.Response.OutputStream.Write(bytes, 0, bytes.Length);
					}
				}
			}
			catch
			{
				context.Response.StatusCode = 500;
			}
			finally
			{
				context.Response.OutputStream.Close();
			}
		}

		public static void RespondGET(HttpListenerContext context)
		{
			var raw = context.Request.RawUrl;

			if (raw.Length == 0 || raw == "/")
			{
				using (var sw = new StreamWriter(context.Response.OutputStream))
				{
					sw.WriteLine("<html>");
					sw.WriteLine("<body>");
					sw.WriteLine($"<h1>hello anonymous</h1>");
					sw.WriteLine("</body>");
					sw.WriteLine("</html>");
				}
				context.Response.StatusCode = 200;
			}
			else
			{
				context.Response.StatusCode = 404;
			}
		}

		public static void RespondPOST(HttpListenerContext context)
		{
			var raw = context.Request.RawUrl;
			var input = new StreamReader(context.Request.InputStream).ReadToEnd();
			using (var sw = new StreamWriter(context.Response.OutputStream))
			{
				sw.WriteLine("<html>");
				sw.WriteLine("<body>");
				sw.WriteLine($"<h1>thanks for the post</h1>");
				sw.WriteLine("</body>");
				sw.WriteLine("</html>");
			}
			context.Response.StatusCode = 200;
		}
	}
}