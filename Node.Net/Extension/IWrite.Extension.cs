using System;
using System.IO;

namespace Node.Net
{
    public static class IWriteExtension
    {
        public static string WriteToBase64String(this IWrite writer, object item)
        {
            return WriteToBase64String(writer.Write, item);
        }
        public static string WriteToBase64String(Action<Stream, object> writeFunction, object item)
        {
            using (var memory = new MemoryStream())
            {
                writeFunction(memory, item);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                return Convert.ToBase64String(memory.GetBuffer());
            }
        }
        public static string WriteToString(this IWrite writer, object item)
        {
            using (var memory = new MemoryStream())
            {
                writer.Write(memory, item);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(memory))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public static void Write(this IWrite writer, string filename, object item)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                writer.Write(fs, item);
            }
        }
    }
}
