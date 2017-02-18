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
        public object Read(Stream original_stream)
        {
            using (var signatureReader = new Internal.Readers.SignatureReader(original_stream))
            {
                var stream = signatureReader.Stream;
                var signature = signatureReader.Signature;
                foreach (string siqnature_key in Keys)
                {
                    if (signature.Contains(siqnature_key)) return this[siqnature_key](stream);
                }
                //if (UnrecognizedSignatureReader != null) return UnrecognizedSignatureReader.Read(original_stream);
                throw new System.Exception($"unrecognized signature '{signature}'");
            }
        }

        public static object ReadXml(Stream original_stream)
        {
            using (var signatureReader = new Internal.Readers.SignatureReader(original_stream))
            {
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
            }
            return null;
        }
        public static object ReadJSON(Stream stream) => Internal.Readers.JSONReader.Default.Read(stream);
        public static object ReadImageSource(Stream stream) => Internal.Readers.ImageSourceReader.Default.Read(stream);
    }
}
