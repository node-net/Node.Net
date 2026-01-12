using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Node.Net.Security
{
    public class ServerCertificateValidation
    {
        public Uri Url { get; set; } = new Uri("https://www.nuget.org/");

        public bool IgnoreServerCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (sender is HttpWebRequest request)
            {
                if (request.Address.Host.Equals(Url.Host, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                if (certificate is null) return false;
                if (chain is null) return false;
                if (errors.ToString().Length < 1) return false;
            }

            return false;
        }
    }
}