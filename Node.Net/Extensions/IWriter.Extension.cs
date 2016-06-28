using System.IO;

namespace Node.Net.Extensions
{
    static class IWriterExtension
    {
        public static void Save(IWriter writer, string filename, object value)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                writer.Save(stream, value);
            }
        }
    }
}
