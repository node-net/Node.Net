using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Data
{
    public sealed class Reader : IRead
    {
        public Reader() { }
        public Reader(Assembly assembly)
        {
            reader = new Readers.Reader(assembly);
        }

        public object Read(Stream stream)
        {
            return reader.Read(stream);
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
        public IDictionaryTypeConverter DictionaryTypeConverter
        {
            get { return reader.DictionaryTypeConverter; }
            set { reader.DictionaryTypeConverter = value; }
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
        private Readers.Reader reader = new Readers.Reader();

        public static Reader Default { get; } = new Reader();
    }
}
