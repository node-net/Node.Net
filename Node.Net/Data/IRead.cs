using System.IO;

namespace Node.Net.Data
{
    public interface IRead
    {
        object Read(Stream stream);
    }
}
