// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
using System;
using System.IO;

namespace Node.Net.Extensions
{
    public static class TextReaderExtension
    {
        public static void EatWhiteSpace(TextReader reader)
        {
            while (Char.IsWhiteSpace((char)(reader.Peek())))
            {
                reader.Read();
            }
        }
        public static string Seek(System.IO.TextReader reader, char value)
        {
            var buffer
                = new System.Collections.Generic.Queue<char>();
            var result = "";
            var ipeek = reader.Peek();
            var cpeek = (char)ipeek;
            var escaped = false;// IsEscaped;
            var builder = new System.Text.StringBuilder();
            builder.Append(result);
            while (reader.Peek() != (int)value ||
                   (reader.Peek() == (int)value && escaped))
            {
                if (reader.Peek() == -1) return result;
                if (reader.Peek() == (int)value && escaped && result.Length > 0) result = result.Substring(0, result.Length - 1);
                var ichar = reader.Read();
                buffer.Enqueue((char)ichar);
                if (escaped && ichar == '\\')
                {
                    // already added the first backslash to the result, skip this one
                }
                else
                {
                    builder.Append((char)ichar);
                    if (ichar == value && !escaped) break;
                }
                escaped = IsEscaped(buffer);
            }
            result = builder.ToString();
            return result;
        }
        public static string Seek(System.IO.TextReader reader, char[] values)
        {
            var result = "";
            var done = false;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            var builder = new System.Text.StringBuilder();
            builder.Append(result);
            while (!done)
            {
                var ichar = reader.Read();
                builder.Append((char)ichar);
                foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            }
            result = builder.ToString();
            return result;
        }

        public static bool IsEscaped(System.Collections.Generic.Queue<char> buffer)
        {
            if (buffer.Count > 0)
            {
                var backSlashCount = 0;
                var cbuf = buffer.ToArray();
                if (cbuf[buffer.Count - 1] == '\\')
                {

                    for (int i = buffer.Count - 1; i > -1; --i)
                    {
                        if (cbuf[i] == '\\') ++backSlashCount;
                        else break;
                    }
                }
                if (backSlashCount % 2 != 0) return true;
            }
            return false;
        }
    }
}
