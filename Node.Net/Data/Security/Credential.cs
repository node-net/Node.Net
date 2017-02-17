using System;
using System.Collections.Generic;
using System.Security;

namespace Node.Net.Data.Security
{
    public class Credential : Dictionary<string, string>, ICredential//, Model.IModel
    {
        public Credential() { this["Type"] = "Credentail"; }// this.Set(nameof(Type),nameof(Credential)); }
        public Credential(IProtection protection) { this["Type"] = "Credential";  _protection = protection; }
        private readonly IProtection _protection = new CurrentUserProtection();
        public IProtection Protection
        {
            get
            {
                return _protection;
            }
        }

        public string Domain { get { return this.Get<string>(nameof(Domain)); }set { this.Set(nameof(Domain), value); } }
        public string UserName { get { return this.Get<string>(nameof(UserName)); } set { this.Set(nameof(UserName),value); } }

        public SecureString Password
        {
            get
            {
                if (ContainsKey(nameof(Password)))
                {
                    return Protection.Unprotect(this[nameof(Password)].ToString());
                }

                return new SecureString();
            }
            set
            {
                this[nameof(Password)] = Protection.Protect(value);
            }
        }

        public static System.Security.SecureString MakeSecureString(string secret)
        {
            var secure = new SecureString();
            System.Array.ForEach(secret.ToCharArray(), secure.AppendChar);
            secure.MakeReadOnly();
            return secure;
        }

    }
}
