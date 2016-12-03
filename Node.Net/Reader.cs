﻿//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using Node.Net.Data;
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
        public Dictionary<string, Type> Types
        {
            get { return reader.Types; }
            set { reader.Types = value; }
        }
        public object Read(Stream stream)
        {
            var instance = reader.Read(stream);
            var dictionary = instance as IDictionary;
            if (dictionary != null)
            {
                Node.Net.IDictionaryExtension.DeepUpdateParents(dictionary);
            }
            return instance;
        }
        public object Read(Assembly assembly, string name) => Read(Extensions.AssemblyExtension.GetStream(assembly, name));
        public object Read(Type type, string name) => Read(Extensions.AssemblyExtension.GetStream(type.Assembly, name));
        public object Read(string name) => reader.Read(name);
        public string[] Signatures { get { return reader.Signatures; } }
        public void Add(string name, string[] signatures, Func<Stream, object> readFunction) => reader.Add(name, signatures, readFunction);
        public void Clear() => reader.Clear();
        public void SetReader(string name, Func<Stream, object> readFunction) => reader.SetReader(name, readFunction);
        public object Open(string openFileDialogFilter = "JSON Files (.json)|*.json|All Files (*.*)|*.*") => reader.Open(openFileDialogFilter);

    }
}
