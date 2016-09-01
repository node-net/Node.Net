using System;
using System.Collections;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Node.Net.Data;
using System.Collections.Generic;
using System.Reflection;

namespace Node.Net
{
    public sealed class Reader : IRead
    {
        public static Reader Default { get; } = new Reader();

        public Reader() { }
        public Reader(Assembly assembly)
        {
            reader = new Data.Reader(assembly);
        }

        public object Read(Stream stream)
        {
            return reader.Read(stream);
        }
        public object Read(string value)
        {
            return reader.Read(value);
        }

        public Type DefaultArrayType
        {
            get { return reader.DefaultArrayType; }
            set { reader.DefaultArrayType = value; }
        }
        public Type DefaultObjectType
        {
            get { return reader.DefaultObjectType; }
            set { reader.DefaultObjectType = value; }
        }
        public Func<IDictionary, IDictionary> IDictionaryTypeConversionFunction
        {
            get { return reader.IDictionaryTypeConversionFunction; }
            set { reader.IDictionaryTypeConversionFunction = value; }
        }
        public Dictionary<string, IRead> TextSignatureReaders
        {
            get { return reader.TextSignatureReaders; }
            set { reader.TextSignatureReaders = value; }
        }
        public Dictionary<byte[], IRead> BinarySignatureReaders
        {
            get { return reader.BinarySignatureReaders; }
            set { reader.BinarySignatureReaders = value; }
        }

        private readonly Node.Net.Data.Reader reader = new Node.Net.Data.Reader();
    }
}
