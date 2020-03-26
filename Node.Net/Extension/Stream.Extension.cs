using System;
using System.IO;
using System.Security.Cryptography;

namespace Node.Net
{
    public static class StreamExtension
    {
        public static void CopyToFile(this Stream stream, string filename)
        {
            using (FileStream? filestream = new FileStream(filename, FileMode.Create))
            {
                int ibyte = stream.ReadByte();
                while (ibyte > -1)
                {
                    filestream.WriteByte((byte)ibyte);
                    ibyte = stream.ReadByte();
                }
            }
        }

        public static string GetMD5String(this Stream stream)
        {
            using MD5? md5 = MD5.Create();
            byte[]? hash = md5.ComputeHash(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public static string GetBase64String(this Stream stream)
        {
            MemoryStream? memory = new MemoryStream();
            stream.CopyTo(memory);
            return Convert.ToBase64String(memory.ToArray());
        }
    }
}