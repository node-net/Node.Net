namespace Node.Net.Data.Deprecated.Security
{
    public interface IProtection
    {
        byte[] Protect(byte[] secret);
        byte[] Unprotect(byte[] encrypted);
    }
}
