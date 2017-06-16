using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;

namespace Node.Net.Beta.Internal.Readers
{
    sealed class JSONReader
    {
        public static JSONReader Default { get; } = new JSONReader();
        public Type DefaultArrayType = typeof(List<dynamic>);
        public Type DefaultObjectType = typeof(Dictionary<string, dynamic>);
        public Type DefaultDocumentType = typeof(Dictionary<string, dynamic>);

        public Func<IDictionary> CreateDefaultObject { get; set; } = CreateDefaultObjectInstance;
        public static IDictionary CreateDefaultObjectInstance() { return new Dictionary<string,dynamic>();}
        public Dictionary<string, Type> ConversionTypeNames { get; } = new Dictionary<string, Type>();
        public int ObjectCount { get; set; }
        public object Read(Stream stream)
        {
            using (System.IO.TextReader reader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            {
                try
                {
                    ObjectCount = 0;
                    var item = Read(reader);
                    return item;
                }
                catch (Exception e)
                {
                    var exception_info = $"JsonRead.Load raised an exception at stream position {stream.Position}";
                    throw new Exception(exception_info, e);
                }
            }
        }

        private static char doubleQuote = '"';
        private static char singleQuote = '\'';
        private object Read(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            var ichar = reader.Peek();
            if (ichar < 0) throw new InvalidDataException(@"end of stream reached");
            var c = (char)ichar;
            // char type
            //  'n'  null
            //  '\d' number
            //  '"' or '\'' string
            //  'f' or 't' bool
            //  '{' object (hash)
            //  '[' array
            if (c == doubleQuote || c == singleQuote) return ReadString(reader);
            if (c == objectOpenCharacter) return ReadObject(reader);
            if (c == arrayOpenCharacter) return ReadArray(reader);
            if (c == 'f' || c == 't') return ReadBool(reader);
            if (c == 'n') return ReadNull(reader);
            return ReadNumber(reader);
        }

        private static object ReadNull(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            var ch = (char)reader.Peek();
            if (ch == 'n')
            {
                reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars n,u,l,l
            }
            return null;
        }
        private static object ReadBool(System.IO.TextReader reader)
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
        private static object ReadNumber(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            char[] endchars = { '}', ']', ',', ' ' };
            var nstr = reader.Seek(endchars);
            if (nstr.Contains(@"."))
            {
                var value = Convert.ToDouble(nstr);
                if (value <= Single.MaxValue) return Convert.ToSingle(nstr);
                return value;
            }
            else
            {
                var value = Convert.ToInt64(nstr);
                if (value <= Int32.MaxValue) return Convert.ToInt32(nstr);
                return value;
            }
        }

        private static string unicodeDoubleQuotes = @"\u0022";
        private static string doubleQuotes = @"""";
        private static string unicodeBackslash = @"\u005c";
        private static string backslash = @"\";
        private static object ReadString(System.IO.TextReader reader)
        {
            reader.EatWhiteSpace();
            var ch = (char)reader.Read(); // consume single or double quote
            var result_raw = reader.SeekIgnoreEscaped(ch);// reader.Seek(ch, true);
            reader.Read(); // consume escaped character

            return result_raw.Replace(unicodeDoubleQuotes, doubleQuotes).Replace(unicodeBackslash, backslash);
            //return result_raw.Replace(@"\u0022", @"""").Replace(@"\u005c", @"\");




            //if(result_raw.Contains(@"\u0022") || result_raw.Contains(@"\u005c")) return result_raw.Replace(@"\u0022", @"""").Replace(@"\u005c", @"\");
            //return result_raw;
            /*
            var result = result_raw.Replace(@"\u0022", @"""");
            result = result.Replace(@"\u005c", @"\");
            return result;
            */
            
        }
        private static char arrayOpenCharacter = '[';
        private object ReadArray(System.IO.TextReader reader)
        {
            var list = Activator.CreateInstance(DefaultArrayType) as IList;
            reader.FastSeek('[');
            var ch = ' ';
            reader.Read(); // consume the '['
            reader.EatWhiteSpace();
            var done = false;
            ch = (char)reader.Peek();
            if (ch == ']')
            {
                done = true;
                reader.Read(); // consume the ']'
            }
            else
            {
                if (ch != 't' && ch != 'f' && ch != 'n')
                {
                    if (Char.IsLetter(ch))
                    {
                        throw new InvalidDataException($"LoadArray char {ch} is not allowed after [");
                    }
                }
            }

            while (!done)
            {
                reader.EatWhiteSpace();
                list.Add(Read(reader));
                reader.EatWhiteSpace();
                var ichar = reader.Peek();
                ch = (char)reader.Peek();
                if (ch == ',') reader.Read(); // consume ','

                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == ']')
                {
                    reader.Read(); // consume ']'
                    done = true;
                }
            }
            return list.Simplify();
        }


        private static char objectOpenCharacter = '{';
        private static char objectCloseCharacter = '}';
        private static char comma = ',';

        private object ReadObject(System.IO.TextReader reader)
        {
            //var dictionary = (ObjectCount == 0) ? Activator.CreateInstance(DefaultDocumentType) as IDictionary :
            //                                              Activator.CreateInstance(DefaultObjectType) as IDictionary;
            IDictionary dictionary = null;
            if(ObjectCount == 0)
            {
                dictionary = Activator.CreateInstance(DefaultDocumentType) as IDictionary;
            }
            else
            {
                if (CreateDefaultObject != null) dictionary = CreateDefaultObject();
                else dictionary = Activator.CreateInstance(DefaultObjectType) as IDictionary;
            }
         
            ObjectCount++;
            reader.FastSeek(objectOpenCharacter);// '{');
            reader.Read(); // consume the '{'
            reader.EatWhiteSpace();
            var done = false;
            if ((char)(reader.Peek()) == objectCloseCharacter)// '}')
            {
                done = true;
                reader.Read(); // consume the '}'
            }
            while (!done)
            {
                reader.EatWhiteSpace();
                var key = ReadString(reader) as string;
                var lastKey = key;
                reader.EatWhiteSpace();
                var ch = (char)reader.Peek();
                reader.Read(); //consume ':'
                dictionary[key] = Read(reader);
                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch ==comma) reader.Read(); // consume ','

                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == objectCloseCharacter)//'}')
                {
                    reader.Read();
                    done = true;
                }
            }
            return dictionary;
        }
    }
}
