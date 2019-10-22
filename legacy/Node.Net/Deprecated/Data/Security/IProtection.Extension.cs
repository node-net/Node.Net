using System;
using System.Net;
using System.Security;
using System.Text;

namespace Node.Net.Deprecated.Data.Security
{
    static class IProtectionExtension
    {
        public static string Protect(IProtection protection, SecureString secure_secret)
        {
            var credential = new NetworkCredential("temp", secure_secret);
            var bytes = Encoding.UTF8.GetBytes(credential.Password);
            var secret_bytes = protection.Protect(bytes);
            return Convert.ToBase64String(secret_bytes);

        }

        public static SecureString Unprotect(IProtection protection, string encrypted_secret)
        {
            var secret_bytes = Convert.FromBase64String(encrypted_secret);
            var bytes = protection.Unprotect(secret_bytes);
            return Credential.MakeSecureString(Encoding.UTF8.GetString(bytes));
        }
    }
}
