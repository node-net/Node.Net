using System.Collections;
using System.IO;
using System.Text;

namespace Node.Net
{
    public static class IResponderExtension
    {
        public static Stream Respond(this JsonRPC.IResponder responder, Stream request)
        {
            JsonRPC.Request? _request = new JsonRPC.Request(new Reader().Read<IDictionary>(request));
            JsonRPC.Response? response = responder.Respond(_request);
            byte[] response_bytes = response.GetBytes();
            return new MemoryStream(response_bytes);
        }

        public static string Respond(this JsonRPC.IResponder responder, string request)
        {
            using (MemoryStream? memory = new MemoryStream(Encoding.UTF8.GetBytes(request)))
            {
                return new StreamReader(responder.Respond(memory)).ReadToEnd().Trim();
            }
        }
    }
}