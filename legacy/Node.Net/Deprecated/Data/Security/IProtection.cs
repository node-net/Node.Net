namespace Node.Net.Deprecated.Data.Security
{
    public interface IProtection
    {
        byte[] Protect(byte[] secret);
        byte[] Unprotect(byte[] encrypted);
    }
}
