using System.Security;
namespace Node.Net.Deprecated.Data.Security
{
    public interface ICredential
    {
        string Domain { get; }
        string UserName { get; }
        SecureString Password { get; }
    }
}
