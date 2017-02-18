namespace Node.Net.Data.Deprecated.Security
{
    public interface ICredentials
    {
        ICredential Get(string domain, string userName);
        void Set(string domain, string userName, System.Security.SecureString password);
    }
}
