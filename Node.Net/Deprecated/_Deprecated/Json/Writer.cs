using System;
using System.Collections.Generic;
namespace Node.Net.Deprecated.Json
{
    public enum JsonFormat { Compressed, Indented };
    public class Writer
    {
        private static readonly Writer _default = new Writer { Format = JsonFormat.Compressed };
        public static Writer Default { get { return _default; } }

        private readonly Internal.JsonWriter writer = new Internal.JsonWriter();

        public JsonFormat Format
        {
            get
            {
                return writer.Style == Internal.Style.Indented ? JsonFormat.Indented : JsonFormat.Compressed;
            }
            set
            {
                if (value == JsonFormat.Indented) writer.Style = Internal.Style.Indented;
                else writer.Style = Internal.Style.Compact;
            }
        }
        public bool IgnoreNullValues
        {
            get { return writer.IgnoreNullValues; }
            set { writer.IgnoreNullValues = value; }
        }
        public List<Type> IgnoreTypes
        {
            get { return writer.IgnoreTypes; }
            set { writer.IgnoreTypes = value; }
        }

        public static void Write(System.Collections.IDictionary dictionary, System.IO.Stream stream,JsonFormat format = JsonFormat.Indented)
        {
            var writer = new Internal.JsonWriter();
            if (format == JsonFormat.Indented) writer.Style = Internal.Style.Indented;
            else writer.Style = Internal.Style.Compact;
            writer.Write(stream, dictionary);
        }
        public static void Write(System.Collections.IEnumerable enumerable, System.IO.Stream stream,JsonFormat format = JsonFormat.Indented)
        {
            var writer = new Internal.JsonWriter();
            if (format == JsonFormat.Indented) writer.Style = Internal.Style.Indented;
            else writer.Style = Internal.Style.Compact;
            writer.Write(stream, enumerable);
        }

        public static string ToString(System.Collections.IDictionary dictionary,JsonFormat format = JsonFormat.Indented)
        {
            var result = "";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                Write(dictionary,memory,format);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        public static string ToString(System.Collections.IEnumerable enumerable,JsonFormat format = JsonFormat.Indented)
        {
            var result = "";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                Write(enumerable, memory,format);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        public void Write(System.IO.Stream stream, System.Collections.IDictionary dictionary)
        {
            writer.Write(stream, dictionary);
        }
        public void Write(System.IO.Stream stream, System.Collections.IEnumerable enumerable)
        {
            writer.Write(stream, enumerable);
        }
    }
}
