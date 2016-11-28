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
        public static void Write(IWrite write,string filename,object value)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                write.Write(fs, value);
            }
        }
    }
}
