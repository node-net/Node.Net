using System.IO;

namespace Node.Net.Deprecated.Readers
{
    public interface IRead
    {
        object Read(Stream stream);
    }
}
