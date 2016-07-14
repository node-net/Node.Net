using System.Collections.Generic;
using System.Security;

namespace Node.Net.Data.Security
{
    public class Credential : Dictionary<string, string>, ICredential
    {
        public Credential() { }
        public Credential(IProtection protection) { _protection = protection; }
        private IProtection _protection = new CurrentUserProtection();
        public IProtection Protection
        {
            get
            {
                return _protection;
            }
        }
        public string Domain
        {
            get
            {
                if (ContainsKey(nameof(Domain))) return this[nameof(Domain)].ToString();
                return string.Empty;
            }
            set
            {
                this[nameof(Domain)] = value;
            }
        }

        public string UserName
        {
            get
            {
                if (ContainsKey(nameof(UserName))) return this[nameof(UserName)].ToString();
                return string.Empty;
            }
            set
            {
                this[nameof(UserName)] = value;
            }
        }

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
