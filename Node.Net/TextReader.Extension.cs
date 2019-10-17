using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net
{
    public static class TextReaderExtension
    {
        public static void EatWhiteSpace(this TextReader reader)
        {
            while (Char.IsWhiteSpace((char)(reader.Peek())))
            {
                reader.Read();
            }
        }

        public static string Seek(this TextReader reader, char[] values, bool ignoreEscaped = false)
        {
            var done = false;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            var builder = new StringBuilder();
            while (!done)
            {
                var ichar = reader.Read();
                if (ichar < 0) { done = true; }
                else
                {
                    if (ignoreEscaped)
                    {
                        if (((char)ichar) != '\\') { builder.Append((char)ichar); }
                    }
                    else { builder.Append((char)ichar); }
                    foreach (char ch in values)
                    {
                        if ((char)reader.Peek() == ch)
                        {
                            if (!ignoreEscaped || ((char)(ichar)) != '\\') { done = true; }
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static void FastSeek(this TextReader reader, char value, bool ignoreEscaped = false)
        {
            if ((char)reader.Peek() == value) { return; }
            while (true)
            {
                var ichar = reader.Read();
                if (ichar < 0) { return; }
                else
                {
                    if ((char)reader.Peek() == value)
                    {
                        if (!ignoreEscaped || ((char)(ichar)) != '\\') { return; }
                    }
                }
            }
        }

        public static string SeekIgnoreEscaped(this TextReader reader, char value)
        {
            const int iBackslash = '\\';
            int iPeek = reader.Peek();
            if ((char)iPeek == value) { return string.Empty; }
            var done = false;
            var builder = new StringBuilder();
            while (!done)
            {
                int iChar = reader.Read();
                if (iChar < 0) { done = true; }
                else
                {
                    if (iChar != iBackslash)
                    {
                        builder.Append((char)iChar);
                    }

                    iPeek = reader.Peek();
                    if ((char)iPeek == value && iChar != iBackslash)
                    {
                        done = true;
                    }
                    // Added 3/21/2019 to correct parsing of \\ embedded in a json string
                    else
                    {
                        if (iChar == iBackslash)
                        {
                            builder.Append((char)iChar);
                        }
                    }
                }
            }
            if (builder.Length == 0)
            {
                return string.Empty;
            }

            return builder.ToString();
        }
    }
}
