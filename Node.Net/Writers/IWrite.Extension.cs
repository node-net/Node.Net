using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Writers
{
    public static class IWriteExtension
    {
        public static void Write(this IWrite write,string filename,object value)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                write.Write(fs, value);
            }
        }

        public static string WriteToString(this IWrite write, object value)
        {
            var memory = new MemoryStream();
            write.Write(memory, value);
            memory.Flush();
            memory.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(memory))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
