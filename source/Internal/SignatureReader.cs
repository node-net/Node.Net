using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Internal
{
    internal sealed class SignatureReader : IDisposable
    {
        private SignatureReader()
        {
        }

        public SignatureReader(Stream original_stream)
        {
            Signature = (Read(original_stream) as Signature)?.ToString() ?? string.Empty;
            if (!original_stream.CanSeek)
            {
                Stream = MemoryStream ?? original_stream;
            }
            else
            {
                Stream = original_stream;
            }
        }

        public string Signature { get; set; } = string.Empty;
        public Stream Stream { get; set; } = null!;
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

        private void Dispose(bool disposing)
        {
            if (disposing && memoryStream != null)
            {
                memoryStream.Dispose();
                memoryStream = null;
            }
        }

        private MemoryStream? memoryStream = null;
#pragma warning disable CS8603 // Possible null reference return.
        public MemoryStream MemoryStream { get { return memoryStream; } }
#pragma warning restore CS8603 // Possible null reference return.

        public static Signature GetSignature(Stream stream)
        {
			using SignatureReader? sr = new SignatureReader();
#pragma warning disable CS8603 // Possible null reference return.
			return sr.Read(stream) as Signature;
#pragma warning restore CS8603 // Possible null reference return.
		}

        private object Read(Stream original_stream)
        {
            memoryStream = null;
            if (!original_stream.CanSeek)
            {
                memoryStream = new MemoryStream();
                int iByte = original_stream.ReadByte();
                while (iByte > -1)
                {
                    memoryStream.WriteByte((byte)iByte);
                    iByte = original_stream.ReadByte();
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
            }

            Stream? stream = original_stream;
            if (memoryStream != null)
            {
                stream = memoryStream;
            }

            List<byte>? bytes = new List<byte>();
            int ibyte = stream.ReadByte();
            while (ibyte > -1)
            {
                bytes.Add((byte)ibyte);
                if (bytes.Count >= MinimumBytes)
                {
                    break;
                }
                ibyte = stream.ReadByte();
            }

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return new Signature(bytes.ToArray());
        }
    }
}