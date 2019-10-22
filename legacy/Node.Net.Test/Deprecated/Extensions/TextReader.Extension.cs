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
            System.Collections.Generic.Queue<char> buffer
                = new System.Collections.Generic.Queue<char>();
            string result = "";
            int ipeek = reader.Peek();
            char cpeek = (char)ipeek;
            bool escaped = false;// IsEscaped;
            while (reader.Peek() != (int)value ||
                   (reader.Peek() == (int)value && escaped))
            {
                if (reader.Peek() == -1) return result;
                if (reader.Peek() == (int)value && escaped && result.Length > 0) result = result.Substring(0, result.Length - 1);
                int ichar = reader.Read();
                buffer.Enqueue((char)ichar);
                if (escaped && ichar == '\\')
                {
                    // already added the first backslash to the result, skip this one
                }
                else
                {
                    result += (char)ichar;
                    if (ichar == value && !escaped) break;
                }
                escaped = IsEscaped(buffer);
            }
            return result;
        }
        public static string Seek(System.IO.TextReader reader, char[] values)
        {
            string result = "";
            bool done = false;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            while (!done)
            {
                int ichar = reader.Read();
                result += (char)ichar;
                foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            }
            return result;
        }

        public static bool IsEscaped(System.Collections.Generic.Queue<char> buffer)
        {
            if (buffer.Count > 0)
            {
                int backSlashCount = 0;
                char[] cbuf = buffer.ToArray();
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
