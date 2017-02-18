using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Deprecated.Data
{
    public sealed class Reader : IRead
    {
        public Reader() { }
        public Reader(Assembly assembly)
        {
            dictionaryTypeConverter = new Readers.DictionaryTypeConverter(assembly);
            reader = new Readers.Reader { IDictionaryTypeConversionFunction = dictionaryTypeConverter.Convert };
        }
        private Readers.DictionaryTypeConverter dictionaryTypeConverter;
        public object Read(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Read(fs);
            }
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

        public Func<IDictionary,IDictionary> IDictionaryTypeConversionFunction
        {
            get { return reader.IDictionaryTypeConversionFunction; }
            set { reader.IDictionaryTypeConversionFunction = value; }
        }
        public Dictionary<string, Func<Stream, object>> TextSignatureReadFunctions
        {
            get { return reader.TextSignatureReadFunctions; }
            set { reader.TextSignatureReadFunctions = value; }
        }
        public Dictionary<byte[], Func<Stream, object>> BinarySignatureReadFunctions
        {
            get { return reader.BinarySignatureReadFunctions; }
            set { reader.BinarySignatureReadFunctions = value; }
        }
        private Readers.Reader reader = new Readers.Reader();

        public static Reader Default { get; } = new Reader();
    }
}
