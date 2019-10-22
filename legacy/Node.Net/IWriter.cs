using System.IO;

namespace Node.Net
{
    public interface IWriter
    {
        void Save(Stream destination, object value);
    }
}
