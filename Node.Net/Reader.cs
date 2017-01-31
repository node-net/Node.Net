//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using Node.Net.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    public sealed class Reader : IRead, IDisposable
    {
        public static Reader Default { get; } = new Reader();

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                reader.Dispose();
            }
        }
        private Readers.Reader reader = new Readers.Reader();
        //{
        //    DefaultObjectType = typeof(Element)//,
            //DefaultDocumentType = typeof(Document)
        //};
        
        public Dictionary<string, Type> Types
        {
            get { return reader.Types; }
            set { reader.Types = value; }
        }

        public Type DefaultObjectType
        {
            get { return reader.DefaultObjectType; }
            set { reader.DefaultObjectType = value; }
        }
        public Type DefaultDocumentType
        {
            get { return reader.DefaultDocumentType; }
            set { reader.DefaultDocumentType = value; }
        }
        public object Read(Stream stream)
        {
            var instance = reader.Read(stream);
            var dictionary = instance as IDictionary;
            if (dictionary != null)
            {
                global::Node.Net.IDictionaryExtension.DeepUpdateParents(dictionary);
            }
            return instance;
        }
        public object Read(Assembly assembly, string name) => Read(AssemblyExtension.GetStream(assembly, name));
        public object Read(Type type, string name) => Read(AssemblyExtension.GetStream(type.Assembly, name));
        public object Read(string name)
        {
            var item = reader.Read(name);
            var dictionary = item as IDictionary;
            if (dictionary != null) dictionary.DeepUpdateParents();
            return item;
        }
        public string[] Signatures { get { return reader.Signatures; } }
        public void Add(string name, string[] signatures, Func<Stream, object> readFunction) => reader.Add(name, signatures, readFunction);
        public void Clear() => reader.Clear();
        public void SetReader(string name, Func<Stream, object> readFunction) => reader.SetReader(name, readFunction);
        public object Open(string name = "JSON Files (.json)|*.json|All Files (*.*)|*.*")
        {
            var item = reader.Open(name);
            var dictionary = item as IDictionary;
            if (dictionary != null) dictionary.DeepUpdateParents();
            return item;
        }

    }
}
