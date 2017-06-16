using System;
using System.IO;
using System.Text;

namespace Node.Net.Beta.Internal
{
    static class TextReaderExtension
    {
        public static void EatWhiteSpace(this TextReader reader)
        {
            while (Char.IsWhiteSpace((char)(reader.Peek())))
            {
                reader.Read();
            }
        }
        public static void FastSeek(this TextReader reader,char value, bool ignoreEscaped = false)
        {
            //if (reader.Peek() == -1) return;
            if ((char)reader.Peek() == value) { return; } 
            while (true)
            {
                var ichar = reader.Read();
                if (ichar < 0) { return; }
                else
                {
                    if ((char)reader.Peek() == value)
                    {
                        if (ignoreEscaped && ((char)(ichar)) == '\\') { }// ignore
                        else { return; }
                    }
                }
            }
        }
        public static void FastSeek(this TextReader reader, char[] values, bool ignoreEscaped = false)
        {
            //if (reader.Peek() == -1) return;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { return; } }
            while (true)
            {
                var ichar = reader.Read();
                if (ichar < 0) { return; }
                else
                {
                    foreach (char ch in values)
                    {
                        if ((char)reader.Peek() == ch)
                        {
                            if (ignoreEscaped && ((char)(ichar)) == '\\') { }// ignore
                            else { return; }
                        }
                    }
                }
            }
        }
        private static StringBuilder builder = new StringBuilder();
        public static string Seek(this TextReader reader, char value, bool ignoreEscaped = false)
        {
            var done = false;
            if ((char)reader.Peek() == value) { done = true; return ""; }
            builder.Clear();
            while (!done)
            {
                var ichar = reader.Read();
                if (ichar < 0) { done = true; }
                else
                {
                    if (ignoreEscaped)
                    {
                        if (((char)ichar) == '\\') { }
                        else { builder.Append((char)ichar); }
                    }
                    else { builder.Append((char)ichar); }
                    if ((char)reader.Peek() == value)
                    {
                        if (ignoreEscaped && ((char)(ichar)) == '\\') { } // ignore
                        else { done = true; }
                    }
                }
            }
            if (builder.Length == 0) return "";
            return builder.ToString();
        }
        public static string Seek(this TextReader reader, char[] values, bool ignoreEscaped = false)
        {
            var done = false;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            builder.Clear();
            while (!done)
            {
                var ichar = reader.Read();
                if (ichar < 0) { done = true; }
                else
                {
                    if (ignoreEscaped)
                    {
                        if (((char)ichar) == '\\') { }
                        else { builder.Append((char)ichar); }
                    }
                    else { builder.Append((char)ichar); }
                    foreach (char ch in values)
                    {
                        if ((char)reader.Peek() == ch)
                        {
                            if (ignoreEscaped && ((char)(ichar)) == '\\') { } // ignore
                            else { done = true; }
                        }
                    }
                }
            }
            return builder.ToString();
        }
    }
}
