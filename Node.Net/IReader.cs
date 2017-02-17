using System.IO;

namespace Node.Net
{
    public interface IReader
    {
        object Load(Stream stream, string name);
    }
}
