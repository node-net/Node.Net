namespace Node.Net.Data.Security
{
    public interface IProtection
    {
        byte[] Protect(byte[] secret);
        byte[] Unprotect(byte[] encrypted);
    }
}
