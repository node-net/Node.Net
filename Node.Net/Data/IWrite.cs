using System.IO;

namespace Node.Net.Data
{
    public interface IWrite
    {
        void Write(Stream stream, object value);
    }
}
