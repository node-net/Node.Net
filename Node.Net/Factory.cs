﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net
{
    public sealed class Factory : IFactory
    {
        public Factory() { }
        public Factory(Assembly assembly)
        {
            factory.ResourceAssemblies.Add(assembly);
        }

        public Func<string, object> GetFunction
        {
            get { return factory.GetFunction; }
            set { factory.GetFunction = value; }
        }

        public Func<Stream, object> ReadFunction
        {
            get { return factory.ReadFunction; }
            set { factory.ReadFunction = value; }
        }

        public List<Assembly> ResourceAssemblies
        {
            get { return factory.ResourceAssemblies; }
            set { factory.ResourceAssemblies = value; }
        }


        public object Create(Type targetType, object source)
        {
            return factory.Create(targetType, source);
        }

        private Node.Net.Factories.Factory factory = new Factories.Factory { ReadFunction = Node.Net.Data.Readers.Reader.Default.Read };
    }
}
