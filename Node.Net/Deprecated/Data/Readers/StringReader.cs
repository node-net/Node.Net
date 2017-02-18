using System.IO;
using System.Text;

namespace Node.Net.Deprecated.Data.Readers
{
    public class StringReader : IRead
    {
        public object Read(Stream stream)
        {
            using (TextReader reader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
