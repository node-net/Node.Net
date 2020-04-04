using System;
using System.Collections;
using System.IO;
using System.Windows.Markup;

namespace Node.Net.Internal
{
    internal class CloneFactory : IFactory
    {
        public Func<Stream, object> ReadFunction { get; set; } = new Node.Net.Reader().Read;
        public Action<Stream, object> WriteFunction { get; set; } = Write;

        public object Create(Type targetType, object source)
        {
            if (source == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            if (targetType.IsInstanceOfType(source))
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    WriteFunction(memory, source);
                    memory.Flush();
                    memory.Seek(0, SeekOrigin.Begin);
                    object? clone = ReadFunction(memory);
                    if (clone != null && targetType.IsInstanceOfType(clone))
                    {
                        return clone;
                    }
                }
            }
#pragma warning disable CS8603 // Possible null reference return.
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }

        private static void Write(Stream stream, object value)
        {
            if (value is IDictionary)
            {
                new JsonWriter().Write(stream, value);
            }
            else
            {
                XamlWriter.Save(value, stream);
            }
        }
    }
}