using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Beta.Internal.Readers
{
    class SignatureReader : IDisposable
    {
        private SignatureReader() { }
        public SignatureReader(Stream original_stream)
        {
            Signature = (Read(original_stream) as Signature).ToString();
            if (!original_stream.CanSeek) Stream = MemoryStream;
            else Stream = original_stream;
        }
        public string Signature { get; set; }
        public Stream Stream { get; set; }
        public int MinimumBytes = 1024;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~SignatureReader()
        {
            Dispose(false);
        }
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }
        private MemoryStream memoryStream = null;
        public MemoryStream MemoryStream { get { return memoryStream; } }

        public static Signature GetSignature(Stream stream)
        {
            using (var sr = new SignatureReader())
            {
                return sr.Read(stream) as Signature;
            }
        }
        private object Read(Stream original_stream)
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
    }
}
