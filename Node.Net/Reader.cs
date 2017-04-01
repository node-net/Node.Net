using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net
{
    // Example signatures
    // "{"                           // ,json
    // "42 4D"                       // .bmp
    // "47 49 46 38 37 61"           // .gif
    public sealed class Reader : Dictionary<string, Func<Stream, object>>
    {
        public Reader()
        {
            Add("<", ReadXml);
            Add("[", ReadJSON);
            Add("{", ReadJSON);
            foreach (var signature in Beta.Internal.Readers.ImageSourceReader.Default.Signatures)
            {
                Add(signature, ReadImageSource);
            }
        }
        public object Read(Stream original_stream)
        {
            using (var signatureReader = new Beta.Internal.Readers.SignatureReader(original_stream))
            {
                var stream = signatureReader.Stream;
                var signature = signatureReader.Signature;
                foreach (string signature_key in Keys)
                {
                    //if (signature.Contains(siqnature_key)) return this[siqnature_key](stream);
                    if (signature.IndexOf(signature_key) == 0) return this[signature_key](stream);
                }
                //if (UnrecognizedSignatureReader != null) return UnrecognizedSignatureReader.Read(original_stream);
                throw new System.Exception($"unrecognized signature '{signature}'");
            }
        }

        public static object ReadXml(Stream original_stream)
        {
            using (var signatureReader = new Beta.Internal.Readers.SignatureReader(original_stream))
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
        public object ReadJSON(Stream stream) => jsonReader.Read(stream);
        public static object ReadImageSource(Stream stream) => Beta.Internal.Readers.ImageSourceReader.Default.Read(stream);
        public Type DefaultDocumentType
        {
            get { return jsonReader.DefaultDocumentType; }
            set { jsonReader.DefaultDocumentType = value; }
        }
        private Beta.Internal.Readers.JSONReader jsonReader = new Beta.Internal.Readers.JSONReader();
    }
}
