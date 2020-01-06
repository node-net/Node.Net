using System;

namespace Node.Net.Service
{
    public class WebClient : System.Net.WebClient
    {
        public int Timeout { get; set; } = 30 * 1000;

        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            return request;
        }
    }
}