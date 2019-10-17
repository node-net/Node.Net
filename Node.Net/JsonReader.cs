using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net
{
    public sealed class JsonReader
    {
        // public Dictionary<string, Type> ConversionTypeNames { get; set; } = new Dictionary<string, Type>();
        public Func<IDictionary, object> ConvertFunction { get; set; } = DefaultConvertFunction;
        private static object DefaultConvertFunction(IDictionary dictionary) { return dictionary; }
        public int ObjectCount { get; set; }
        public Type DefaultObjectType { get; set; } = typeof(Dictionary<string, dynamic>);
        public Type DefaultDocumentType { get; set; } = typeof(Dictionary<string, dynamic>);
        public Type DefaultArrayType { get; set; } = typeof(List<dynamic>);

        public object? Read(Stream stream)
        {
            using System.IO.TextReader reader = new StreamReader(stream, Encoding.Default, true, 1024, true);
            try
            {
                ObjectCount = 0;
                var item = Read(reader);
                return item;
            }
            catch (Exception e)
            {
                var exception_info = $"JsonRead.Load raised an exception at stream position {stream.Position}";
                throw new InvalidOperationException(exception_info, e);
            }
        }

        private object? Read(System.IO.TextReader reader)
        {
            const char objectOpenCharacter = '{';
            const char arrayOpenCharacter = '[';
            const char doubleQuote = '"';
            const char singleQuote = '\'';
            reader.EatWhiteSpace();
            var ichar = reader.Peek();
            if (ichar < 0)
            {
                throw new InvalidDataException("end of stream reached");
            }

            var c = (char)ichar;
            // char type
            //  'n'  null
            //  '\d' number
            //  '"' or '\'' string
            //  'f' or 't' bool
            //  '{' object (hash)
            //  '[' array
            if (c == doubleQuote || c == singleQuote)
            {
                return ReadString(reader);
            }

            if (c == objectOpenCharacter)
            {
                return ReadObject(reader);
            }

            if (c == arrayOpenCharacter)
            {
                return ReadArray(reader);
            }

            if (c == 'f' || c == 't')
            {
                return ReadBool(reader);
            }

            if (c == 'n')
            {
                return ReadNull(reader);
            }

            return ReadNumber(reader);
        }

        private object? ReadNull(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            var ch = (char)reader.Peek();
            if (ch == 'n')
            {
                reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars n,u,l,l
            }
            return null;
        }

        private object ReadBool(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            var ch = (char)reader.Peek();
            if (ch == 't')
            {
                reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars t,r,u,e
                return true;
            }
            reader.Read(); reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read char f,a,l,s,e
            return false;
        }

        private object ReadNumber(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            char[] endchars = { '}', ']', ',', ' ' };
            var nstr = reader.Seek(endchars);
            if (nstr.Contains("."))
            {
                var value = Convert.ToDouble(nstr);
                if (value <= Single.MaxValue)
                {
                    return Convert.ToSingle(nstr);
                }

                return value;
            }
            else
            {
                var value = Convert.ToInt64(nstr);
                if (value <= Int32.MaxValue)
                {
                    return Convert.ToInt32(nstr);
                }

                return value;
            }
        }

        private object ReadString(System.IO.TextReader reader)
        {
            const string unicodeDoubleQuotes = @"\u0022";
            const string doubleQuotes = @"""";
            const string unicodeBackslash = @"\u005c";
            const string backslash = @"\";
            reader.EatWhiteSpace();
            string stringResult = reader.SeekIgnoreEscaped((char)reader.Read());
            reader.Read(); // consume escaped character
            return stringResult.Replace(unicodeDoubleQuotes, doubleQuotes)
                .Replace(unicodeBackslash, backslash)
                .Replace("u005c", backslash)
                .Replace("\\\\", "\\");
        }

        private object ReadArray(System.IO.TextReader reader)
        {
            var list = (Activator.CreateInstance(DefaultArrayType) as IList)!;
            reader.FastSeek('[');
            reader.Read(); // consume the '['
            reader.EatWhiteSpace();
            var done = false;
            var ch = (char)reader.Peek();
            if (ch == ']')
            {
                done = true;
                reader.Read(); // consume the ']'
            }
            else
            {
                if (ch != 't' && ch != 'f' && ch != 'n' && Char.IsLetter(ch))
                {
                    throw new InvalidDataException($"LoadArray char {ch} is not allowed after [");
                }
            }

            while (!done)
            {
                reader.EatWhiteSpace();
                list.Add(Read(reader));
                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == ',')
                {
                    reader.Read(); // consume ','
                }

                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == ']')
                {
                    reader.Read(); // consume ']'
                    done = true;
                }
            }
            return list;//.Simplify();
        }

        private IDictionary GetDictionary()
        {
            if (ObjectCount == 0)
            {
                return (Activator.CreateInstance(DefaultDocumentType) as IDictionary)!;
            }
            else
            {
                return (Activator.CreateInstance(DefaultObjectType) as IDictionary)!;
            }
        }

        private object ReadObject(System.IO.TextReader reader)
        {
            const char objectOpenCharacter = '{';
            const char objectCloseCharacter = '}';
            const char comma = ',';
            IDictionary dictionary = GetDictionary();

            ObjectCount++;
            reader.FastSeek(objectOpenCharacter);
            reader.Read(); // consume the '{'
            reader.EatWhiteSpace();
            var done = false;
            if ((char)(reader.Peek()) == objectCloseCharacter)
            {
                done = true;
                reader.Read(); // consume the '}'
            }
            while (!done)
            {
                reader.EatWhiteSpace();
                var key = ReadString(reader) as string;
                reader.EatWhiteSpace();
                reader.Read(); //consume ':'
                dictionary[key] = Read(reader);
                reader.EatWhiteSpace();
                var ch = (char)reader.Peek();
                if (ch == comma)
                {
                    reader.Read(); // consume ','
                }

                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == objectCloseCharacter)
                {
                    reader.Read();
                    done = true;
                }
            }

            return ConvertFunction(dictionary);
        }
    }
}
