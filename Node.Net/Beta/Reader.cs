using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Beta
{
    // Example signatures
    // "{"                           // ,json
    // "42 4D"                       // .bmp
    // "47 49 46 38 37 61"           // .gif
    public sealed class Reader : Dictionary<string,Func<Stream,object>>
    {
        public Reader()
        {
            Add("<", ReadXml);
            Add("[", ReadJSON);
            Add("{", ReadJSON);
            foreach(var signature in Internal.Readers.ImageSourceReader.Default.Signatures)
            {
                Add(signature, ReadImageSource);
            }
        }
        /*
         * Add("Json", new List<string> { "{", "[" }.ToArray(), JsonReader.Read);
            Add("Xml", new List<string> { "<" }.ToArray(), xmlReader.Read);
            readers.Add("ImageSource", imageSourceReader.Read);
            foreach(var signature in ImageSourceReader.Default.Signatures)
            {
                signatureReaders.Add(signature, "ImageSource");
            }
         */
        public object Read(Stream original_stream)
        {
            var signatureReader = new Internal.Readers.SignatureReader(original_stream);
            var stream = signatureReader.Stream;
            var signature = signatureReader.Signature;
            /*
            var signatureReader = new Internal.Readers.SignatureReader();
            var signature = signatureReader.Read(original_stream) as Internal.Readers.Signature;
            var stream = original_stream;
            if (!stream.CanSeek) stream = signatureReader.MemoryStream;
            */
            foreach(string siqnature_key in Keys)
            {
                if (signature.Contains(siqnature_key)) return this[siqnature_key](stream);
            }
            //if (UnrecognizedSignatureReader != null) return UnrecognizedSignatureReader.Read(original_stream);
            throw new System.Exception($"unrecognized signature '{signature}'");
            
            return null;
        }

        public static object ReadXml(Stream original_stream)
        {
            var signatureReader = new Internal.Readers.SignatureReader(original_stream);
            //var signature = signatureReader.Read(original_stream) as Internal.Readers.Signature;
            //var stream = original_stream;
            //if (!stream.CanSeek) stream = signatureReader.MemoryStream;

            //var signature_string = signature.ToString();
            var stream = signatureReader.Stream;
            var signature_string = signatureReader.Signature;
            if (signature_string.Contains("http://schemas.microsoft.com/winfx/2006/xaml/presentation"))
            {
                return System.Windows.Markup.XamlReader.Load(stream);
            }
            if (signature_string.IndexOf("<") == 0)
            {
                var xdoc = new System.Xml.XmlDocument();
                xdoc.Load(stream);
                return xdoc;
            }
            return null;
        }
        public static object ReadJSON(Stream stream) => Internal.Readers.JSONReader.Default.Read(stream);
        public static object ReadImageSource(Stream stream) => Internal.Readers.ImageSourceReader.Default.Read(stream);
    }
}
