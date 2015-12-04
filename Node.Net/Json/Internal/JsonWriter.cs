using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Json.Internal
{
    public enum Style { Compact, Indented };

    class JsonWriter
    {
        public Style Style = Style.Compact;
        public bool AddTypeInfo = true;

        public static string Write(object value, Style style = Style.Compact)
        {
            JsonWriter writer = new JsonWriter() { Style = style };
            return writer.Write(value);
        }

        public static void Write(string filename, object value, Style style = Style.Compact)
        {
            using (FileStream stream = File.OpenWrite(filename))
            {
                Write(stream, value, style);
            }
        }
        public static void Write(Stream stream, object value, Style style = Style.Compact)
        {
            JsonWriter writer = new JsonWriter() { Style = style };
            writer.Write(stream, value);
        }

        public string Write(object value)
        {
            MemoryStream memory = new MemoryStream();
            StreamWriter writer = new StreamWriter(memory);
            Write(writer, value);
            writer.Flush();
            memory.Seek(0, SeekOrigin.Begin);
            StreamReader reader = new StreamReader(memory);
            return reader.ReadToEnd();
        }

#if NET40
        private static StreamWriter streamWriter = null;
#endif
        public void Write(Stream stream, object value)
        {
#if NET40
            streamWriter = new StreamWriter(stream);
            streamWriter.Write(value);
            streamWriter.Flush();
#else
            using (StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.Default, 1024, true))
            {
                Write(writer, value);
            }
#endif
        }
        public void Write(TextWriter writer, object value)
        {
            if (ReferenceEquals(null, value)) WriteNull(writer);
            else if (typeof(string).IsAssignableFrom(value.GetType())) WriteString(writer, value);
            else if (typeof(IDictionary).IsAssignableFrom(value.GetType())) WriteIDictionary(writer, value);
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType())) WriteIEnumerable(writer, value);
            else
            {
                WriteValueType(writer, value);
            }
        }


        private void WriteNull(TextWriter writer)
        {
            writer.Write("null");
        }
        private void WriteString(TextWriter writer, object value)
        {
            writer.Write($"\"{value.ToString()}\"");
        }
        private void WriteValueType(TextWriter writer, object value)
        {
            if (value.GetType() == typeof(bool)) writer.Write(value.ToString().ToLower());
            else writer.Write(value.ToString());
        }
        private void WriteIEnumerable(TextWriter writer, object value)
        {
            if (Style == Style.Indented)
            {
                writer.Write(GetIndent());
            }
            writer.Write("[");
            System.Collections.IEnumerable enumerable = value as System.Collections.IEnumerable;
            int writeCount = 0;
            foreach (object item in enumerable)
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
            writer.Write("]");
        }
        private void UpdateTypeInfo(IDictionary idictionary)
        {
            if (idictionary.GetType() == typeof(Dictionary<string, dynamic>)) return;
            if (!AddTypeInfo) return;
            if (idictionary.GetType().IsGenericType)
            {
                Type[] generic_types = idictionary.GetType().GetGenericArguments();
                if (generic_types.Length == 2)
                {
                    if (!generic_types[0].IsAssignableFrom(typeof(string))) return;
                    if (!generic_types[1].IsAssignableFrom(typeof(string))) return;
                }
            }

            idictionary["Type"] = idictionary.GetType().FullName;
            idictionary["Assembly"] = idictionary.GetType().Assembly.GetName().Name;
        }
        private void WriteIDictionary(TextWriter writer, object value)
        {
            int index = 0;
            if (Style == Style.Indented) writer.Write(GetIndent());
            writer.Write("{");
            if (IndentString.Length > 0) writer.Write(System.Environment.NewLine);
            IndentLevel++;

            System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
            UpdateTypeInfo(dictionary);
            foreach (object key in dictionary.Keys)
            {
                object item = dictionary[key];
                if (index > 0)
                {
                    if (Style == Style.Indented) writer.Write($",{System.Environment.NewLine}");
                    else writer.Write(",");
                }
                if (Style == Style.Indented) writer.Write(GetIndent());
                Write(writer, key.ToString());
                writer.Write(":");
                Write(writer, dictionary[key]);

                ++index;
            }
            IndentLevel--;
            if (dictionary.Keys.Count > 0)
            {
                if (Style == Style.Indented)
                {
                    writer.Write(System.Environment.NewLine);
                    writer.Write(GetIndent());
                }
            }
            writer.Write("}");
            if (Style == Style.Indented) writer.Write(System.Environment.NewLine);
        }

        private uint IndentLevel = 0;
        private string IndentString = "";
        private string GetIndent()
        {
            string indent = "";
            for (uint i = 0; i < IndentLevel; ++i)
            {
                indent = indent + IndentString;
            }
            return indent;
        }
    }
}