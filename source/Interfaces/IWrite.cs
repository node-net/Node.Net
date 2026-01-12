using System.IO;

namespace Node.Net
{
    public interface IWrite
    {
        void Write(Stream stream, object value);
    }
}