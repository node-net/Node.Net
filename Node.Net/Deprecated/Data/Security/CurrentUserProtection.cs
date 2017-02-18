using System.Text;

namespace Node.Net.Data.Deprecated.Security
{
    public class CurrentUserProtection : IProtection
    {
        private byte[] _additionalEntropy;
        public byte[] AdditionalEntropy
        {
            get
            {
                if (_additionalEntropy == null)
                {
                    _additionalEntropy = Encoding.UTF8.GetBytes("DoNotChangeThisValue");
                }
                return _additionalEntropy;
            }
            set
            {
                _additionalEntropy = value;
            }
        }
        public byte[] Protect(byte[] secret)
        {
            return System.Security.Cryptography.ProtectedData.Protect(
                secret, AdditionalEntropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
        }
        public byte[] Unprotect(byte[] encrypted)
        {
            return System.Security.Cryptography.ProtectedData.Unprotect(
                encrypted, AdditionalEntropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
        }
    }
}
