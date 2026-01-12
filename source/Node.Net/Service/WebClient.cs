using System;

namespace Node.Net.Service
{
#pragma warning disable SYSLIB0014 // WebClient is obsolete, but this is a wrapper class for backward compatibility
    public class WebClient : System.Net.WebClient
#pragma warning restore SYSLIB0014
    {
        public int Timeout { get; set; } = 30 * 1000;

        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            System.Net.WebRequest? request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            return request;
        }
    }
}