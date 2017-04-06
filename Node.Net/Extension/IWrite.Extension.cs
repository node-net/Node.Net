using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class IWriteExtension
    {
        public static string WriteToBase64String(this IWrite writer,object item)
        {
            using (var memory = new MemoryStream())
            {
                writer.Write(memory, item);
                memory.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                return Convert.ToBase64String(memory.GetBuffer());
            }

        }
    }
}
