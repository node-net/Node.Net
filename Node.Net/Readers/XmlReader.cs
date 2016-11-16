﻿using System.IO;

namespace Node.Net.Readers
{
    public sealed class XmlReader : IRead
    {
        private SignatureReader signatureReader = new SignatureReader();
        public object Read(Stream original_stream)
        {
            var signature = signatureReader.Read(original_stream) as Signature;
            var stream = original_stream;
            if (!stream.CanSeek) stream = signatureReader.MemoryStream;

            if (signature.Text.Contains("http://schemas.microsoft.com/winfx/2006/xaml/presentation"))
            {
                return System.Windows.Markup.XamlReader.Load(stream);
            }
            var xdoc = new System.Xml.XmlDocument();
            xdoc.Load(stream);
            return xdoc;
        }
    }
}