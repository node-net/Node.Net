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
        public static string Seek(this TextReader reader, char value)
        {
            char[] values = { value };
            return Seek(reader, values);
        }
        public static string Seek(this TextReader reader, char[] values,bool ignoreEscaped = false)
        {
            var result = @"";
            var done = false;
            //var lastChar = -1;
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
                    if(ignoreEscaped)
                    {
                        if(((char)ichar) == '\\') { }
                        else { builder.Append((char)ichar); }
                    }
                    else { builder.Append((char)ichar); }
                    //builder.Append((char)ichar);
                    foreach (char ch in values)
                    {
                        if ((char)reader.Peek() == ch)
                        {
                            if (ignoreEscaped && ((char)(ichar)) == '\\')
                            {
                                // ignore
                            }
                            else { done = true; }
                        }
                    }
                }
                //lastChar = ichar;
            }
            result = builder.ToString();
            return result;
        }
    }
}
