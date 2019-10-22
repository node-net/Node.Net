using System.IO;

namespace Node.Net.Readers
{
    public interface IRead
    {
        object Read(Stream stream);
    }
}
