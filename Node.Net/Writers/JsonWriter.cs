using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Environment;

namespace Node.Net.Writers
{
    public enum JsonFormat { Compact, Indented };
    public class JsonWriter : IWrite
    {
        public JsonFormat Format = JsonFormat.Indented;
        private int IndentLevel;
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
            if (Format == JsonFormat.Indented)
            {
                var sb = new StringBuilder();
                while (sb.Length < IndentLevel * 2) { sb.Append(" "); }
                return sb.ToString();
            }
            return string.Empty;
        }

        private string GetLineFeed()
        {
            if (Format == JsonFormat.Indented)
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
            else if (typeof(Node.Net.Writers.IElement).IsAssignableFrom(value.GetType())) WriteIElement(writer, value);
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
                else writer.Write($"{GetIndent()}{value.ToString().ToLower()}");
            }
            else
            {
                if (writingPrimitiveValue) writer.Write(value.ToString());
                else writer.Write($"{GetIndent()}{value}");
            }
        }
        private void WriteDoubleArray2D(System.IO.TextWriter writer, object value)
        {
            double[,] array = value as double[,];
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
            writer.Write($"{GetIndent()}[{GetLineFeed()}");
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
                        if (writeCount > 0) writer.Write($",{GetLineFeed()}{GetIndent()}");
                        Write(writer, item);
                        ++writeCount;
                    }
                }
            }
            PopIndent();
            writer.Write($"{GetLineFeed()}{GetIndent()}]{GetLineFeed()}");

        }

        private bool writingPrimitiveValue = false;

        private void WriteIElement(System.IO.TextWriter writer, object value)
        {
            var index = 0;
            writer.Write("{");
            PushIndent();

            var element = value as Node.Net.Writers.IElement;

            foreach (string key in element.Keys)
            {
                var item = element.Get(key);
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

                    var tmp = element.Get(key);
                    if (tmp == null || tmp.GetType().IsPrimitive || tmp.GetType() == typeof(string))
                    {
                        writer.Write(": ");
                        writingPrimitiveValue = true;
                        Write(writer, element.Get(key));
                        writingPrimitiveValue = false;
                    }
                    else
                    {
                        writer.Write($": ");
                        Write(writer, element.Get(key));
                    }
                    ++index;
                }
            }
            PopIndent();
            if (element.Count > 0) writer.Write(GetLineFeed());
            writer.Write($"{GetIndent()}}}");
        }
        private void WriteIDictionary(System.IO.TextWriter writer, object value)
        {
            var index = 0;
            //writer.Write($"{GetIndent()}{{{GetLineFeed()}");
            //writer.Write($"{GetIndent()}{{");
            writer.Write("{");
            PushIndent();

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
                        writingPrimitiveValue =false;
                    }
                    else
                    {
                        //writer.Write($":{GetLineFeed()}");
                        writer.Write($": ");
                        Write(writer, dictionary[key]);

                    }

                    ++index;
                }
            }
            PopIndent();
            if (dictionary.Count > 0) writer.Write(GetLineFeed());
            //if (IndentLevel == 0) writer.Write($"{GetIndent()}}}");
            //else writer.Write($"{GetIndent()}}}{GetLineFeed()}");
            writer.Write($"{GetIndent()}}}");
        }
    }
}
