using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class StreamExtension
    {
        public static void CopyToFile(this Stream stream, string filename)
        {
            using (var filestream = new FileStream(filename, FileMode.Create))
            {
                int ibyte = stream.ReadByte();
                while(ibyte > -1)
                {
                    filestream.WriteByte((byte)ibyte);
                    ibyte = stream.ReadByte();
                }
            }
        }
    }
}
