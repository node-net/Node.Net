using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Readers
{
    public sealed class SignatureReader : IRead
    {
        public int MinimumBytes = 1024;

        private MemoryStream memoryStream = null;
        public MemoryStream MemoryStream { get { return memoryStream; } }
        public object Read(Stream original_stream)
        {
            memoryStream = null;
            if (!original_stream.CanSeek)
            {
                memoryStream = new MemoryStream();
                var iByte = original_stream.ReadByte();
                while (iByte > -1) memoryStream.WriteByte((byte)iByte);
                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            var stream = original_stream;
            if (memoryStream != null) stream = memoryStream;
            var bytes = new List<byte>();
            var textSignature = new StringBuilder();
            var ibyte = stream.ReadByte();
            while (ibyte > -1)
            {
                bytes.Add((byte)ibyte);
                if (bytes.Count >= MinimumBytes)
                {
                    break;
                }
                ibyte = stream.ReadByte();
            }

            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            return new Signature(bytes.ToArray());
        }
        /*
        public static KeyValuePair<Stream, byte[]> GetStreamSignature(Stream stream, int maxBytes = 128)
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
            memory.Seek(0, SeekOrigin.Begin);
            return new KeyValuePair<Stream, byte[]>(memory, bytes.ToArray());
        }*/
    }
}
