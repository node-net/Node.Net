using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Internal
{
    public sealed class JsonReader : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~JsonReader()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                DefaultArrayType = null;
                //DefaultObjectType = null;
                //DefaultDocumentType = null;
                CreateDefaultObject = null;
                //ConversionTypeNames = null;
                ObjectCount = 0;
            }
        }

        public Func<IDictionary>? CreateDefaultObject { get; set; } = null;
        public Dictionary<string, Type> ConversionTypeNames { get; set; } = new Dictionary<string, Type>();
        public int ObjectCount { get; set; }
        public Type DefaultObjectType { get; set; } = typeof(Collections.Spatial);// typeof(Dictionary<string, dynamic>);
        public Type DefaultDocumentType { get; set; } = typeof(Collections.Spatial);//typeof(Dictionary<string, dynamic>);
        public Type? DefaultArrayType { get; set; } = typeof(Collections.List);// typeof(List<dynamic>);

        public object Read(Stream stream)
        {
            using System.IO.TextReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            try
            {
                ObjectCount = 0;
                object? item = Read(reader);
#pragma warning disable CS8603 // Possible null reference return.
                return item;
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception e)
            {
                string? exception_info = $"JsonRead.Load raised an exception at stream position {stream.Position}";
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
            int ichar = reader.Peek();
            if (ichar < 0)
            {
                throw new InvalidDataException("end of stream reached");
            }

            char c = (char)ichar;
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

        private static object? ReadNull(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            char ch = (char)reader.Peek();
            if (ch == 'n')
            {
                reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars n,u,l,l
            }
            return null;
        }

        private static object ReadBool(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            char ch = (char)reader.Peek();
            if (ch == 't')
            {
                reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars t,r,u,e
                return true;
            }
            reader.Read(); reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read char f,a,l,s,e
            return false;
        }

        private static object ReadNumber(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            char[] endchars = { '}', ']', ',', ' ' };
            string? nstr = reader.Seek(endchars);
            if (nstr.Contains("."))
            {
                double value = Convert.ToDouble(nstr);
                if (value <= Single.MaxValue)
                {
                    return Convert.ToSingle(nstr);
                }

                return value;
            }
            else
            {
                long value = Convert.ToInt64(nstr);
                if (value <= Int32.MaxValue)
                {
                    return Convert.ToInt32(nstr);
                }

                return value;
            }
        }

        private static object ReadString(System.IO.TextReader reader)
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
#pragma warning disable CS8604 // Possible null reference argument.
            IList? list = Activator.CreateInstance(DefaultArrayType) as IList;
#pragma warning restore CS8604 // Possible null reference argument.
            reader.FastSeek('[');
            reader.Read(); // consume the '['
            reader.EatWhiteSpace();
            bool done = false;
            char ch = (char)reader.Peek();
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
                list?.Add(Read(reader));
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
#pragma warning disable CS8604 // Possible null reference argument.
            return list.Simplify();
#pragma warning restore CS8604 // Possible null reference argument.
        }

        private object ReadObject(System.IO.TextReader reader)
        {
            const char objectOpenCharacter = '{';
            const char objectCloseCharacter = '}';
            const char comma = ',';
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IDictionary dictionary = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            if (ObjectCount == 0)
            {
                if (DefaultDocumentType == null)
                {
                    throw new InvalidOperationException("DefaultDocumentType is null");
                }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                dictionary = Activator.CreateInstance(DefaultDocumentType) as IDictionary;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (dictionary == null)
                {
                    throw new InvalidOperationException($"Unable to create instance of {DefaultDocumentType.FullName}");
                }
            }
            else
            {
                if (CreateDefaultObject != null)
                {
                    dictionary = CreateDefaultObject();
                    if (dictionary == null) { throw new InvalidOperationException("CreateDefaultObject returned null"); }
                }
                else
                {
                    if (DefaultObjectType == null)
                    {
                        throw new InvalidOperationException("DefaultObjectType is null");
                    }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    dictionary = Activator.CreateInstance(DefaultObjectType) as IDictionary;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    if (dictionary == null)
                    {
                        throw new InvalidOperationException($"Unable to create isntance of {DefaultObjectType.FullName}");
                    }
                }
            }

            ObjectCount++;
            reader.FastSeek(objectOpenCharacter);
            reader.Read(); // consume the '{'
            reader.EatWhiteSpace();
            bool done = false;
            if ((char)(reader.Peek()) == objectCloseCharacter)// '}')
            {
                done = true;
                reader.Read(); // consume the '}'
            }
            while (!done)
            {
                reader.EatWhiteSpace();
                string? key = ReadString(reader) as string;
                /*
#if DEBUG
                if (key == "string_symbol")
                {
                    int x = 0;
                }
#endif*/
                reader.EatWhiteSpace();
                reader.Read(); //consume ':'
#pragma warning disable CS8604 // Possible null reference argument.
                dictionary[key] = Read(reader);
#pragma warning restore CS8604 // Possible null reference argument.
                reader.EatWhiteSpace();
                char ch = (char)reader.Peek();
                if (ch == comma)
                {
                    reader.Read(); // consume ','
                }

                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == objectCloseCharacter)//'}')
                {
                    reader.Read();
                    done = true;
                }
            }

            string? type = dictionary.Get<string>("Type", "");
            if (type.Length > 0 && ConversionTypeNames.ContainsKey(type) && !ConversionTypeNames[type].IsInstanceOfType(dictionary))
            {
                if (!(Activator.CreateInstance(ConversionTypeNames[type]) is IDictionary converted))
                {
                    throw new InvalidOperationException($"Unable to create instance of {ConversionTypeNames[type].FullName}");
                }
                foreach (object? key in dictionary.Keys)
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    if (!converted.Contains(key))
#pragma warning restore CS8604 // Possible null reference argument.
                    {
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
                        converted.Add(key, dictionary[key]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8604 // Possible null reference argument.
                    }
                }
                return converted;
            }
            return dictionary;
        }
    }
}