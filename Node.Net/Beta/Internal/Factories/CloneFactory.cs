﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Node.Net.Beta.Internal.Factories
{
    class CloneFactory : IFactory
    {
        public Func<Stream, object> ReadFunction { get; set; } = new Node.Net.Reader().Read;
        public Action<Stream, object> WriteFunction { get; set; } = Write;
        public object Create(Type target_type, object source)
        {
            if (source == null) return null;
            if (target_type.IsAssignableFrom(source.GetType()))
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    WriteFunction(memory, source);
                    memory.Flush();
                    memory.Seek(0, SeekOrigin.Begin);
                    var clone = ReadFunction(memory);
                    if (clone != null && target_type.IsAssignableFrom(clone.GetType())) return clone;
                }
            }
            return null;
        }

        private static void Write(Stream stream,object value)
        {
            if (typeof(IDictionary).IsAssignableFrom(value.GetType()))
            {
                JSONWriter.Default.Write(stream, value);
            }
            else
            {
                XamlWriter.Save(value, stream);
            }
        }
    }
}