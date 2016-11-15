using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Readers
{
    public sealed class Reader : IRead
    {
        public static Reader Default { get; } = new Reader();
        public Reader()
        {
            readers.Add("Json", new JsonReader());
            signatureReaders.Add("{", "Json");
            signatureReaders.Add("[", "Json");
            readers.Add("Xml", new XmlReader());
            signatureReaders.Add("<", "Xml");
            readers.Add("ImageSource", new ImageSourceReader());
            signatureReaders.Add("42 4D", "ImageSource");                       // .bmp
            signatureReaders.Add("47 49 46 38 37 61", "ImageSource");           // .gif
            signatureReaders.Add("47 49 46 38 39 61", "ImageSource");           // .gif
            signatureReaders.Add("FF D8 FF E0", "ImageSource");                 // .jpg
            signatureReaders.Add("FF D8 FF E1", "ImageSource");                 // .jpg
            signatureReaders.Add("49 49 2A 00", "ImageSource");                 // .tif
            signatureReaders.Add("4D 4D 00 2A", "ImageSource");                 // .tif
            signatureReaders.Add("89 50 4E 47 0D 0A 1A 0A", "ImageSource");     // .png
        }
        public Dictionary<string, Type> Types { get; set; } = null;
        private SignatureReader signatureReader = new SignatureReader();
        private Dictionary<string, IRead> readers = new Dictionary<string, IRead>();
        private Dictionary<string, string> signatureReaders = new Dictionary<string, string>();
        public object Read(Stream original_stream)
        {
            var signature = signatureReader.Read(original_stream) as Signature;
            var stream = original_stream;
            if (!stream.CanSeek) stream = signatureReader.MemoryStream;
            foreach (string signature_key in signatureReaders.Keys)
            {
                if (signature.Text.IndexOf(signature_key) == 0 ||
                   signature.HexString.IndexOf(signature_key) == 0)
                {
                    return readers[signatureReaders[signature_key]].Read(stream);
                }
            }
            throw new System.Exception($"unrecognized signature '{signature.HexString}' {signature.Text}");
        }
    }
}
