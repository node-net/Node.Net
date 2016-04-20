using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Json.Internal
{
    class JsonReader
    {
        private static JsonReader _default = new JsonReader() { AutoResolveTypes = true };
        public static JsonReader Default { get { return _default; } }

        public static object Load(string value)
        {
            return Default.Read(value);
        }
        public static object Load(string value, IDictionary dictionary)
        {
            IDictionary d = Load(value) as IDictionary;
            dictionary.Clear();
            Collections.Copier.Copy(d, dictionary);
            return dictionary;
        }
        public static object Load(Stream stream)
        {
            return Default.Read(stream);
        }
        public static object Load(Stream stream, IDictionary dictionary)
        {
            IDictionary d = Load(stream) as IDictionary;
            dictionary.Clear();
            Collections.Copier.Copy(d, dictionary);
            return dictionary;
        }

        public Type DefaultListType = typeof(List<object>);
        public Type DefaultDictionaryType = typeof(Dictionary<string, dynamic>);
        public Dictionary<string, Type> Types = new Dictionary<string, Type>();
        public void AddTypes(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!Types.ContainsKey(type.FullName))
                {
                    Types.Add(type.FullName, type);
                }
            }
        }
        public bool AutoResolveTypes = false;
        public object Read(string value)
        {
            if (File.Exists(value))
            {
                using (StreamReader reader = new StreamReader(value))
                {
                    return Read(reader);
                }
            }
            else
            {
                MemoryStream memory = new MemoryStream();
                StreamWriter writer = new StreamWriter(memory);
                writer.Write(value);
                writer.Flush();
                memory.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(memory);
                return Read(reader);
            }
        }

        public object Read(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return Read(reader);
        }


        public object Read(TextReader reader)
        {
            reader.EatWhiteSpace();
            char ch = (char)reader.Peek();
            if (ch == '{')
            {
                IDictionary dictionary = ReadDictionary(reader);
                dictionary = ConvertDictionaryType(dictionary);
                return dictionary;
            }
            if (ch == '[')
            {
                IList list = ReadList(reader);
                list = ConvertListType(list);
                return list;
            }
            if (ch == '\'' || ch == '"') return ReadString(reader);
            if (ch == 't' || ch == 'f') return ReadBool(reader);
            if (ch == 'n') return ReadNull(reader);
            if (Char.IsDigit(ch)) return ReadNumber(reader);
            return null;
        }

        private object ReadNull(TextReader reader)
        {
            for (int i = 0; i < 4; ++i) { reader.Read(); };
            return null;
        }
        private object ReadBool(TextReader reader)
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
        private object ReadNumber(TextReader reader)
        {
            reader.EatWhiteSpace();
            char[] endchars = { '}', ']', ',', ' ' };
            string nstr = reader.Seek(endchars);
            if (nstr.Contains("."))
            {
                return System.Convert.ToDouble(nstr);
            }
            try
            {
                int value = System.Convert.ToInt32(nstr);
                return value;
            }
            catch
            {
                long value = System.Convert.ToInt64(nstr);
                return value;
            }
        }
        private object ReadString(TextReader reader)
        {
            reader.EatWhiteSpace();
            char ch = (char)reader.Peek();
            reader.Read(); // consume single or double quote
            string result = reader.Seek(ch);


            result = result.Replace("\\u0022", "\"");
            result = result.Replace("\\u005c", "\\");

            reader.Read(); // consume escaped character
            return result;
        }
        private IList ReadList(TextReader reader)
        {
            IList list = Activator.CreateInstance(DefaultListType) as IList;
            reader.Seek('[');
            char ch = ' ';
            reader.Read();// Read(reader); // consume the '['
            reader.EatWhiteSpace();
            bool done = false;
            ch = (char)reader.Peek();
            if (ch == ']')
            {
                done = true;
                reader.Read();// Read(reader); // consume the ']'
            }
            while (!done)
            {
                reader.EatWhiteSpace();
                list.Add(Read(reader));
                reader.EatWhiteSpace();

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
            return list;
        }
        private IList ConvertListType(IList source)
        {
            int nullCount = 0;
            Dictionary<Type, int> typeCounts = new Dictionary<Type, int>();
            typeCounts[typeof(string)] = 0;
            typeCounts[typeof(int)] = 0;
            typeCounts[typeof(double)] = 0;
            foreach (object item in source)
            {
                if (ReferenceEquals(null, item)) ++nullCount;
                else
                {
                    if (item.GetType() == typeof(string)) typeCounts[typeof(string)] = typeCounts[typeof(string)] + 1;
                    if (item.GetType() == typeof(int)) typeCounts[typeof(int)] = typeCounts[typeof(int)] + 1;
                    if (item.GetType() == typeof(double)) typeCounts[typeof(double)] = typeCounts[typeof(double)] + 1;
                }
            }
            if (nullCount == 0)
            {
                if (typeCounts[typeof(int)] == source.Count)
                {
                    List<int> new_list = new List<int>();
                    foreach (object i in source) { new_list.Add((int)i); }
                    return new_list;
                }
                if (typeCounts[typeof(string)] == source.Count)
                {
                    List<string> new_list = new List<string>();
                    foreach (object i in source) { new_list.Add((string)i); }
                    return new_list;
                }
                if (typeCounts[typeof(double)] == source.Count)
                {
                    List<double> new_list = new List<double>();
                    foreach (object i in source) { new_list.Add((double)i); }
                    return new_list;
                }
            }
            return source;
        }
        private IDictionary ReadDictionary(TextReader reader)
        {
            IDictionary dictionary = Activator.CreateInstance(DefaultDictionaryType) as IDictionary;
            reader.Seek('{');
            reader.Read(); // consume the '{'
            reader.EatWhiteSpace();
            bool done = false;
            if ((char)(reader.Peek()) == '}')
            {
                done = true;
                reader.Read(); // consume the '}'
            }
            while (!done)
            {
                reader.EatWhiteSpace();
                string key = ReadString(reader) as string;
                string lastKey = key;
                reader.EatWhiteSpace();
                char ch = (char)reader.Peek();
                reader.Read(); //consume ':'

                object value = Read(reader);
                if (!object.ReferenceEquals(null,value) &&
                     value.GetType() == typeof(string) &&
                     value.ToString().IndexOf("base64:") == 0)
                {
                    value = System.Convert.FromBase64String(value.ToString().Substring(7));
                }
                dictionary[key] = value;// Read(reader);
                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == ',') reader.Read(); // consume ','

                reader.EatWhiteSpace();
                ch = (char)reader.Peek();
                if (ch == '}')
                {
                    reader.Read();
                    done = true;
                }
            }
            return dictionary;
        }
        private Type GetIDictionaryType(IDictionary source)
        {
            Type result = source.GetType();

            string stype = "";
            if (source.Contains("Type")) stype = source["Type"].ToString();
            if (stype.Length > 0)
            {
                if (Types.ContainsKey(stype)) return Types[stype];
                //return JsonSerializationBinder.GetType(stype);
            }
            return result;
        }
        private IDictionary ConvertDictionaryType(IDictionary source)
        {
            IDictionary result = source;
            Type targetType = GetIDictionaryType(source);
            if (!ReferenceEquals(null, targetType) && targetType != source.GetType())
            {
                IDictionary new_dictionary = Activator.CreateInstance(targetType) as IDictionary;
                if (!ReferenceEquals(null, new_dictionary))
                {
                    Collections.Copier.Copy(source, new_dictionary);
                    result = new_dictionary;
                }
            }
            return result;
        }
    }
}