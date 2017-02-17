using System;
using System.IO;
using System.Text;

namespace Node.Net.Factories.Prototype.Internal
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
        public static string Seek(this TextReader reader, char value)
        {
            char[] values = { value };
            return Seek(reader, values);
        }
        public static string Seek(this TextReader reader, char[] values)
        {
            var result = @"";
            var done = false;
            foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
            var builder = new StringBuilder();
            builder.Append(result);
            while (!done)
            {
                var ichar = reader.Read();
                if (ichar < 0)
                {
                    done = true;
                }
                else
                {
                    builder.Append((char)ichar);
                    foreach (char ch in values) { if ((char)reader.Peek() == ch) { done = true; } }
                }
            }
            result = builder.ToString();
            return result;
        }
    }
}
