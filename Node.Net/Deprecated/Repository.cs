﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Deprecated
{
    public class Repository
    {
        public static Repository Default { get; } = new Repository();
        public object Get(string name) => repository.Get(name);
        public void Set(string name, object value) => repository.Set(name, value);
        public Func<Stream,object> ReadFunction
        {
            get { return repository.ReadFunction; }
            set { repository.ReadFunction = value; }
        }
        public Action<Stream,object> WriteFunction
        {
            get { return repository.WriteFunction; }
            set { repository.WriteFunction = value; }
        }
        private readonly global::Node.Net.Deprecated.Repositories.MemoryRepository repository
            = new Repositories.MemoryRepository
            {
                ReadFunction = new Reader
                {
                    DefaultDocumentType = typeof(Dictionary<string, dynamic>),
                    DefaultObjectType = typeof(Dictionary<string, dynamic>)
                }.Read,
                WriteFunction = Deprecated.Writer.Default.Write
            };
    }
}