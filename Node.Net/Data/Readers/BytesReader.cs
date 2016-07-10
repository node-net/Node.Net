using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Data.Readers
{
    public class BytesReader : IRead
    {
        public object Read(Stream stream)
        {
            var bytes = new List<byte>();
            var ibyte = stream.ReadByte();
            while (ibyte > -1)
            {
                bytes.Add((byte)ibyte);
                ibyte = stream.ReadByte();
            }
            return bytes.ToArray();
        }

        public static KeyValuePair<Stream,byte[]> GetStreamSignature(Stream stream,int maxBytes = 128)
        {
            MemoryStream memory = null;
            if (!stream.CanSeek) memory = new MemoryStream();

            var bytes = new List<byte>();
            var ibyte = stream.ReadByte();

            while (ibyte > -1)
            {
                if (bytes.Count < maxBytes) bytes.Add((byte)ibyte);
                if (bytes.Count == maxBytes && stream.CanSeek)
                {

                    break;
                }
                if (memory != null) memory.WriteByte((byte)ibyte);
                ibyte = stream.ReadByte();
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
                return new KeyValuePair<Stream, byte[]>(stream, bytes.ToArray());
            }
            return new KeyValuePair<Stream, byte[]>(memory, bytes.ToArray());
        }

        public static byte[] HexStringToByteArray(string hex_in)
        {
            var hex = hex_in.Replace(" ", "");
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString().ToUpper().Trim();
        }
    }
}
