using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Data.Readers
{
    sealed class JsonReader : IRead
    {
        public static JsonReader Default { get; } = new JsonReader();
        public Type DefaultArrayType = typeof(List<dynamic>);
        public Type DefaultObjectType = typeof(Dictionary<string, dynamic>);
        public object Read(Stream stream)
        {
            using (System.IO.TextReader reader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            {
                try
                {
                    return Read(reader);
                }
                catch (Exception e)
                {
                    var exception_info = $"JsonRead.Load raised an exception at stream position {stream.Position}";
                    throw new Exception(exception_info, e);
                }
            }
        }

        private object Read(System.IO.TextReader reader)
        {
            EatWhiteSpace(reader);
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
            if (c == 'n') return ReadNull(reader);
            if (c == 'f' || c == 't') return ReadBool(reader);
            if (c == '"' || c == '\'') return ReadString(reader);
            if (c == '[') return ReadArray(reader);
            return (c == '{') ? ReadObject(reader) : ReadNumber(reader);
        }

        private static object ReadNull(System.IO.TextReader reader)
        {
            EatWhiteSpace(reader);
            var ch = (char)reader.Peek();
            if (ch == 'n')
            {
                reader.Read(); reader.Read(); reader.Read(); reader.Read(); // read chars n,u,l,l
            }
            return null;
        }
        private static object ReadBool(System.IO.TextReader reader)
        {
            EatWhiteSpace(reader);
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
            EatWhiteSpace(reader);
            char[] endchars = { '}', ']', ',', ' ' };
            var nstr = Seek(reader, endchars);
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

        private static object ReadString(System.IO.TextReader reader)
        {
            EatWhiteSpace(reader);
            var ch = (char)reader.Peek();
            reader.Read(); // consume single or double quote
            char[] chars = { ch };
            var result_raw = Seek(reader, chars);

            var result = result_raw.Replace(@"\u0022", @"""");
            result = result.Replace(@"\u005c", @"\");
            reader.Read(); // consume escaped character
            return result;
        }
        private object ReadArray(System.IO.TextReader reader)
        {
            var list = Activator.CreateInstance(DefaultArrayType) as IList;
            Seek(reader, '[');
            var ch = ' ';
            reader.Read();// Read(reader); // consume the '['
            EatWhiteSpace(reader);
            var done = false;
            ch = (char)reader.Peek();
            if (ch == ']')
            {
                done = true;
                reader.Read();// Read(reader); // consume the ']'
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
                EatWhiteSpace(reader);
                list.Add(Read(reader));
                EatWhiteSpace(reader);
                var ichar = reader.Peek();
                //if (ichar < 0) throw new InvalidDataException($"Load Array end of stream reached before array close was found");
                ch = (char)reader.Peek();
                if (ch == ',') reader.Read(); // consume ','

                EatWhiteSpace(reader);
                ch = (char)reader.Peek();
                if (ch == ']')
                {
                    reader.Read(); // consume ']'
                    done = true;
                }
            }
            return ConvertArray(list);
        }

        public static object ConvertArray(IList list)
        {
            if (list.Count == 0) return list;
            var hasNull = false;
            var allTypesConvertToDouble = true;
            var types = new List<Type>();
            foreach (var item in list)
            {
                if (item == null) hasNull = true;
                else
                {
                    if (!types.Contains(item.GetType())) types.Add(item.GetType());
                    if (!item.GetType().IsPrimitive) allTypesConvertToDouble = false;
                }
            }
            if (types.Count == 1 && !hasNull)
            {
                if (types[0] == typeof(string))
                {
                    var strings = new List<string>();
                    foreach (string value in list) { strings.Add(value); }
                    return strings.ToArray();
                }
                if (types[0] == typeof(double[]))
                {
                    var length = 0;
                    foreach (double[] dar in list)
                    {
                        if (length == 0) length = dar.Length;
                        if (length != dar.Length) length = -1;
                    }
                    if (length > -1)
                    {
                        double[,] array = new double[list.Count, length];
                        for (int i = 0; i < list.Count; ++i)
                        {
                            for (int j = 0; j < length; ++j)
                            {
                                array[i, j] = ((double[])list[i])[j];
                            }
                        }
                        return array;
                    }
                }
            }
            if(allTypesConvertToDouble)
            { 
                if (types[0] == typeof(double) || types[0] == typeof(int) || types[0] == typeof(float))
                {
                    var doubles = new List<double>();
                    foreach (var value in list) { doubles.Add(Convert.ToDouble(value)); }
                    return doubles.ToArray();
                }
                
            }

            return list;
        }
        private IDictionary ReadObject(System.IO.TextReader reader)
        {
            var dictionary = Activator.CreateInstance(DefaultObjectType) as IDictionary;
            Seek(reader, '{');
            reader.Read(); // consume the '{'
            EatWhiteSpace(reader);
            var done = false;
            if ((char)(reader.Peek()) == '}')
            {
                done = true;
                reader.Read(); // consume the '}'
            }
            while (!done)
            {
                EatWhiteSpace(reader);
                var key = ReadString(reader) as string;
                var lastKey = key;
                EatWhiteSpace(reader);
                var ch = (char)reader.Peek();
                reader.Read(); //consume ':'
                dictionary[key] = Read(reader);
                EatWhiteSpace(reader);
                ch = (char)reader.Peek();
                if (ch == ',') reader.Read(); // consume ','

                EatWhiteSpace(reader);
                ch = (char)reader.Peek();
                if (ch == '}')
                {
                    reader.Read();
                    done = true;
                }
            }
            return dictionary;
        }
        #region TextReader Helper Methods
        private static void EatWhiteSpace(System.IO.TextReader reader)
        {
            while (Char.IsWhiteSpace((char)(reader.Peek())))
            {
                reader.Read();
            }
        }

        private static string Seek(System.IO.TextReader reader, char value)
        {
            char[] values = { value };
            return Seek(reader, values);
        }
        private static string Seek(System.IO.TextReader reader, char[] values)
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

        #endregion
    }
}
