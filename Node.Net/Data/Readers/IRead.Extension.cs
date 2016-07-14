﻿using System.IO;
using System.Text;

namespace Node.Net.Data.Readers
{
    public static class IReadExtension
    {
        public static object Read(IRead read, string value)
        {
            if (File.Exists(value))
            {
                using (FileStream stream = new FileStream(value, FileMode.Open))
                {
                    return read.Read(stream);
                }
            }
            using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                return read.Read(memory);
            }
        }
    }
}
