﻿using Node.Net.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    public sealed class Reader : IRead
    {
        public static Reader Default { get; } = new Reader();
        private Readers.Reader reader = new Readers.Reader();
        public object Read(Stream stream) => reader.Read(stream);
        public void Add(string name, string[] signatures, Func<Stream, object> readFunction) => reader.Add(name, signatures, readFunction);
        public void Clear() => reader.Clear();
        public void SetReader(string name, Func<Stream, object> readFunction) => reader.SetReader(name, readFunction);
    }
    /*
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

        private readonly Node.Net.Data.Reader reader = new Node.Net.Data.Reader();
    }*/
}
