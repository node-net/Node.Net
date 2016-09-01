using System;
using System.Collections;
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
            dictionaryTypeConverter = new Readers.DictionaryTypeConverter(assembly);
            reader = new Readers.Reader { IDictionaryTypeConversionFunction = dictionaryTypeConverter.Convert };
        }
        private readonly Readers.DictionaryTypeConverter dictionaryTypeConverter;

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

        public Func<IDictionary,IDictionary> IDictionaryTypeConversionFunction
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
        private readonly Readers.Reader reader = new Readers.Reader();

        public static Reader Default { get; } = new Reader();
    }
}
