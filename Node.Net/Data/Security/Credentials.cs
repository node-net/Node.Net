using System.Collections.Generic;

namespace Node.Net.Data.Security
{
    public class Credentials : Dictionary<string, dynamic>, ICredentials,Model.IModel
    {
        public Credentials() { this.SetTypeName(); }

        private readonly IProtection _protection = new CurrentUserProtection();
        public IProtection Protection
        {
            get
            {
                return _protection;
            }
        }
        public ICredential Get(string domain, string userName)
        {
            if (ContainsKey($"{domain}|{userName}"))
            {
                return this[$"{domain}|{userName}"] as ICredential;
            }
            return null;
        }
        public void Set(string domain, string userName, System.Security.SecureString password)
        {

            this[$"{domain}|{userName}"] = new Credential(Protection)
            {
                Domain = domain,
                UserName = userName,
                Password = password
            };
        }
    }
}
