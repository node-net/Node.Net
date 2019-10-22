using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Extensions
{
    public class StreamExtension
    {
        public static void CopyToFile(Stream source, string fileName)
        {
            if (System.IO.File.Exists(fileName)) System.IO.File.Delete(fileName);
            using (System.IO.FileStream dest = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
            {
                int ibyte = source.ReadByte();
                while (ibyte > -1)
                {
                    dest.WriteByte((byte)ibyte);
                    ibyte = source.ReadByte();
                }
                dest.Flush();
            }
        }
    }
}
