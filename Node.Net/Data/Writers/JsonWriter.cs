using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Data.Writers
{
    class JsonWriter : IWrite
    {
        //public bool IgnoreNullValues;
        public List<Type> IgnoreTypes = new List<Type>();
        public void Write(Stream stream, object value)
        {
            using (StreamWriter writer = new StreamWriter(stream, Encoding.Default, 1024, true))
            {
                Write(writer, value);
            }
        }

        private static JsonWriter _default;
        public static JsonWriter Default
        {
            get
            {
                if (object.ReferenceEquals(null, _default))
                {
                    _default = new JsonWriter();
                }
                return _default;
            }
        }

        private void Write(System.IO.TextWriter writer, object value)
        {
            if (ReferenceEquals(null, value)) WriteNull(writer);
            else if (typeof(byte[]).IsAssignableFrom(value.GetType())) WriteBytes(writer, (byte[])(value));
            else if (typeof(string).IsAssignableFrom(value.GetType())) WriteString(writer, value);
            else if (typeof(IDictionary).IsAssignableFrom(value.GetType())) WriteIDictionary(writer, value);
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType())) WriteIEnumerable(writer, value);
            else
            {
                WriteValueType(writer, value);
            }
        }

        private static void WriteNull(System.IO.TextWriter writer) { writer.Write("null"); }
        private static void WriteBytes(System.IO.TextWriter writer, byte[] bytes)
        {
            WriteString(writer, $"base64:{Convert.ToBase64String(bytes)}");
        }
        private static void WriteString(System.IO.TextWriter writer, object value)
        {
            var svalue = value.ToString();
            // Escape '\' first
            var escaped_value = svalue.Replace("\\", "\\u005c");
            // Escape '"'
            escaped_value = escaped_value.Replace("\"", "\\u0022");
            writer.Write($"\"{escaped_value}\"");
        }
        private static void WriteValueType(System.IO.TextWriter writer, object value)
        {
            if (value.GetType() == typeof(bool)) writer.Write(value.ToString().ToLower());
            else writer.Write(value.ToString());
        }
        private void WriteIEnumerable(System.IO.TextWriter writer, object value)
        {
            writer.Write("[");
            var enumerable = value as System.Collections.IEnumerable;
            var writeCount = 0;
            foreach (object item in enumerable)
            {
                var skip = false;
                //if (object.ReferenceEquals(null, item) && IgnoreNullValues) skip = true;
                if (!object.ReferenceEquals(null, item) && IgnoreTypes.Contains(item.GetType())) skip = true;
                if (!skip)
                {
                    if (object.ReferenceEquals(null, item) ||
                       item.GetType().IsValueType ||
                       typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                    {
                        if (writeCount > 0) writer.Write(",");
                        Write(writer, item);
                        ++writeCount;
                    }
                }
            }
            writer.Write("]");
        }
        private void WriteIDictionary(System.IO.TextWriter writer, object value)
        {
            var index = 0;
            writer.Write("{");

            var dictionary = value as System.Collections.IDictionary;

            foreach (object key in dictionary.Keys)
            {
                var item = dictionary[key];
                var skip = false;
                //if (object.ReferenceEquals(null, item) && IgnoreNullValues) skip = true;
                if (!object.ReferenceEquals(null, item) && IgnoreTypes.Contains(item.GetType())) skip = true;
                if (!skip)
                {
                    if (index > 0)
                    {
                        writer.Write(",");
                    }
                    Write(writer, key.ToString());
                    writer.Write(":");
                    Write(writer, dictionary[key]);

                    ++index;
                }
            }
            writer.Write("}");
        }
    }
}
