using System.IO;
using System.Text;

namespace Node.Net.Deprecated.Data.Readers
{
    static class IReadExtension
    {
        public static object Read(IRead read, string value)
        {
            if (File.Exists(value))
            {
                using (FileStream stream = new FileStream(value, FileMode.Open))
                {
                    var result = read.Read(stream);
                    stream.Close();
                    return result;
                }
            }
            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                return read.Read(memory);
            }
        }
    }
}

