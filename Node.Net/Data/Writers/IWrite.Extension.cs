using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data.Writers
{
    public static class IWriteExtension
    {
        public static void Write(IWrite write,string filename,object value)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                write.Write(stream, value);
            }
        }
    }
}
