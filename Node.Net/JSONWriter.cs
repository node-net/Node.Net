using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;

namespace Node.Net
{
    public enum JSONFormat { Compact, Indented };
    public sealed class JSONWriter : IWrite
    {
        public JSONFormat Format = JSONFormat.Indented;
        private int IndentLevel;
        public List<Type> IgnoreTypes = new List<Type>();
        private bool WritingArray { get; set; } = false;
        public void Write(Stream stream, object value)
        {
            using (StreamWriter writer = new StreamWriter(stream, Encoding.Default, 1024, true))
            {
                Write(writer, value);
            }
        }

        public string WriteToString(object value)
        {
            var result = "";
            MemoryStream memory = new MemoryStream();
            Write(memory, value);
            memory.Flush();
            memory.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(memory))
            {
                result = sr.ReadToEnd();
            }
            memory = null;
            return result;
        }

        public static JSONWriter Default { get; } = new JSONWriter();

        private void PushIndent()
        {
            IndentLevel++;
        }
        private void PopIndent()
        {
            IndentLevel--;
        }
        private string GetIndent()
        {
            if (Format == JSONFormat.Indented)
            {
                var sb = new StringBuilder();
                while (sb.Length < IndentLevel * 2) { sb.Append(" "); }
                return sb.ToString();
            }
            return string.Empty;
        }

        private string GetLineFeed()
        {
            if (Format == JSONFormat.Indented)
            {
                return NewLine;
            }
            return string.Empty;
        }
        private void Write(System.IO.TextWriter writer, object value)
        {
            if (ReferenceEquals(null, value)) WriteNull(writer);
            else if (typeof(byte[]).IsAssignableFrom(value.GetType())) WriteBytes(writer, (byte[])(value));
            else if (typeof(string).IsAssignableFrom(value.GetType())) WriteString(writer, value);
            else if (typeof(IDictionary).IsAssignableFrom(value.GetType())) WriteIDictionary(writer, value);
            else if (typeof(double[,]).IsAssignableFrom(value.GetType())) WriteDoubleArray2D(writer, value);
            else if (typeof(IEnumerable).IsAssignableFrom(value.GetType())) WriteIEnumerable(writer, value);
            else
            {
                WriteValueType(writer, value);
            }
        }

        private static void WriteNull(System.IO.TextWriter writer) { writer.Write("null"); }
        private void WriteBytes(System.IO.TextWriter writer, byte[] bytes)
        {
            WriteString(writer, $"base64:{Convert.ToBase64String(bytes)}");
        }
        private void WriteString(System.IO.TextWriter writer, object value)
        {
            var svalue = value.ToString();
            // Escape '\' first
            var escaped_value = svalue.Replace("\\", "\\u005c");
            // Escape '"'
            escaped_value = escaped_value.Replace("\"", "\\u0022");
            if (writingPrimitiveValue) writer.Write($"\"{escaped_value}\"");
            else writer.Write($"{GetIndent()}\"{escaped_value}\"");
        }
        private void WriteValueType(System.IO.TextWriter writer, object value)
        {
            if (value.GetType() == typeof(bool))
            {
                if (writingPrimitiveValue) writer.Write(value.ToString().ToLower());
                else
                {
                    writer.Write($"{GetIndent()}{value.ToString().ToLower()}");
                }
            }
            else
            {
                if (writingPrimitiveValue) writer.Write(value.ToString());
                else
                {
                    if (value.GetType() == typeof(float) || value.GetType() == typeof(double) ||
                       value.GetType() == typeof(int) || value.GetType() == typeof(long) ||
                       value.GetType() == typeof(string))
                    {
                        writer.Write(value.ToString());
                    }
                    else { writer.Write($"{GetIndent()}{value}"); }
                }
            }
        }
        private void WriteDoubleArray2D(System.IO.TextWriter writer, object value)
        {
            var array = value as double[,];
            var length0 = array.GetLength(0);
            var length1 = array.GetLength(1);
            var equivalentList = new List<List<double>>();
            for (int i = 0; i < length0; ++i)
            {
                equivalentList.Add(new List<double>());
                for (int j = 0; j < length1; ++j)
                {
                    equivalentList[i].Add(array[i, j]);
                }
            }
            WriteIEnumerable(writer, equivalentList);
        }
        private void WriteIEnumerable(System.IO.TextWriter writer, object value)
        {
            WritingArray = true;
            //writer.Write($"{GetIndent()}[{GetLineFeed()}");
            //writer.Write($"{GetIndent()}[");
            writer.Write("[");
            PushIndent();
            var enumerable = value as System.Collections.IEnumerable;
            var writeCount = 0;
            foreach (object item in enumerable)
            {
                var skip = false;
                if (!object.ReferenceEquals(null, item) && IgnoreTypes.Contains(item.GetType())) skip = true;
                if (!skip)
                {
                    if (object.ReferenceEquals(null, item) ||
                       item.GetType().IsValueType ||
                       typeof(System.Collections.IEnumerable).IsAssignableFrom(value.GetType()))
                    {
                        if (writeCount > 0)
                        {
                            if (!WritingArray)
                            {
                                writer.Write($",{GetLineFeed()}{GetIndent()}");
                            }
                            else { writer.Write(","); }
                        }
                        Write(writer, item);
                        ++writeCount;
                    }
                }
            }
            PopIndent();
            if (!WritingArray) writer.Write($"{GetLineFeed()}{GetIndent()}]{GetLineFeed()}");
            else writer.Write($"]");
            WritingArray = false;
        }

        private bool writingPrimitiveValue;

        private void WriteIDictionary(System.IO.TextWriter writer, object value)
        {
            WritingArray = false;
            var index = 0;
            writer.Write("{");
            PushIndent();

            var dictionary = value as System.Collections.IDictionary;

            foreach (object key in dictionary.Keys)
            {
                var item = dictionary[key];
                var skip = false;
                if (!object.ReferenceEquals(null, item) && IgnoreTypes.Contains(item.GetType())) skip = true;
                if (!skip)
                {
                    if (index > 0)
                    {
                        writer.Write($",{GetLineFeed()}{GetIndent()}");
                    }
                    else
                    {
                        writer.Write($"{GetLineFeed()}{GetIndent()}");
                    }
                    writingPrimitiveValue = true;
                    Write(writer, key.ToString());
                    writingPrimitiveValue = false;

                    var tmp = dictionary[key];
                    if (tmp == null || tmp.GetType().IsPrimitive || tmp.GetType() == typeof(string))
                    {
                        writer.Write(": ");
                        writingPrimitiveValue = true;
                        Write(writer, dictionary[key]);
                        writingPrimitiveValue = false;
                    }
                    else
                    {
                        writer.Write($": ");
                        Write(writer, dictionary[key]);

                    }

                    ++index;
                }
            }
            PopIndent();
            if (dictionary.Count > 0) writer.Write(GetLineFeed());
            writer.Write($"{GetIndent()}}}");
        }
    }
}
