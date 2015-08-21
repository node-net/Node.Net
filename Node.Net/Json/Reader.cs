namespace Node.Net.Json
{
    public class Reader
    {
        public Reader() { }
        public Reader(System.Type newDictionaryType) { dictionaryType = newDictionaryType; }
        public Reader(System.Collections.Generic.Dictionary<string,System.Type> typesDictionary)
        {
            types = typesDictionary;
        }
        public Reader(System.Reflection.Assembly assembly)
        {
            AddTypes(assembly);
        }
        public Reader(System.Reflection.Assembly[] assemblies)
        {
            foreach (System.Reflection.Assembly assembly in assemblies)
            {
                AddTypes(assembly);
            }
        }

        public static System.Collections.Generic.Dictionary<string, System.Type> GetTypeDictionary(System.Reflection.Assembly assembly)
        {
            System.Collections.Generic.Dictionary<string, System.Type> types
                = new System.Collections.Generic.Dictionary<string, System.Type>();
            foreach(System.Type type in GetTypes(assembly))
            {
                types[type.Name] = type;
            }
            return types;
        }
        
        public static System.Type[] GetTypes(System.Reflection.Assembly assembly)
        {
            System.Collections.Generic.List<System.Type> types
                = new System.Collections.Generic.List<System.Type>();
            foreach(System.Type type in assembly.GetTypes())
            {
                System.Reflection.ConstructorInfo ci = type.GetConstructor(System.Type.EmptyTypes);
                if(!object.ReferenceEquals(null,ci))
                {
                    types.Add(type);
                }
            }
            return types.ToArray();
        }
        public void AddTypes(System.Reflection.Assembly assembly)
        {
            foreach(System.Type type in GetTypes(assembly))
            {
                types[type.Name] = type;
            }
        }
        public void AddTypes(System.Collections.Generic.Dictionary<string,System.Type> typesDictionary)
        {
            foreach(string key in typesDictionary.Keys)
            {
                types[key] = typesDictionary[key];
            }
        }

        public static Hash ReadHash(System.IO.Stream stream)
        {
            Hash result = new Hash();
            Reader reader = new Reader();
            reader.Read(stream,result);
            return result;
        }

        public static Hash ReadHash(string value)
        {
            Hash result = new Hash();
            Reader reader = new Reader();
            reader.Read(value, result);
            return result;
        }
        private System.IO.StreamReader streamReader = null;
        private System.Type dictionaryType = typeof(Hash);
        private System.Type arrayType = typeof(Array);
        private System.Collections.Generic.Dictionary<string, System.Type> types = new System.Collections.Generic.Dictionary<string, System.Type>();

        public System.Collections.Generic.Dictionary<string, System.Type> Types { get { return types; } }

        public System.IO.StreamReader StreamReader
        {
            get { return streamReader; }
            set { streamReader = value; }
        }
        
        #if NET35
        public object Read(string value)
        #else
        public dynamic Read(string value)
        #endif
        {
            System.Collections.IDictionary dictionary = CreateHash();
            Read(value, dictionary);
            return dictionary;
        }

        public void Read(string value, System.Collections.IDictionary dictionary)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(value)))
            {
                Read(memory, dictionary);
            }
        }

        public void Read(string value, System.Collections.IList list)
        {
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(value)))
            {
                Read(memory, list);
            }
        }
        public System.Collections.IDictionary Read(System.IO.Stream stream, System.Collections.IDictionary dictionary)
        {
            streamReader = new System.IO.StreamReader(stream);
            Read(dictionary);
            return dictionary;
        }

        public System.Collections.IList Read(System.IO.Stream stream, System.Collections.IList list)
        {
            streamReader = new System.IO.StreamReader(stream);
            Read(list);
            return list;
        }

        private string lastKey = "";

        private void Read(System.Collections.IDictionary dictionary)
        {
            EatWhiteSpace();
            Read(); // consume the '{'
            EatWhiteSpace();
            bool done = false;
            if ((char)(Peek()) == '}')
            {
                done = true;
                Read(); // consume the '}'
            }
            while (!done)
            {
                EatWhiteSpace();
                string key = ReadString();
                lastKey = key;
                EatWhiteSpace();
                char ch = (char)Peek();
                if (ch != ':') throw new System.Exception("found character " + ch + " when a colon was expected. at stream position " + Position.ToString() + " last key " + key);
                Read(); //consume ':'
                dictionary[key] = ReadNext();
                EatWhiteSpace();
                ch = (char)Peek();
                if (ch == ',') Read(); // consume ','

                EatWhiteSpace();
                ch = (char)Peek();
                if (ch == '}')
                {
                    Read();
                    done = true;
                }
            }
        }

        private void Read(System.Collections.IList list)
        {
            char ch = ' ';
            EatWhiteSpace();
            Read(); // consume the '['
            EatWhiteSpace();
            bool done = false;
            ch = (char)Peek();
            if (ch == ']')
            {
                done = true;
                Read(); // consume the ']'
            }
            while (!done)
            {
                EatWhiteSpace();
                list.Add(ReadNext());
                EatWhiteSpace();

                ch = (char)Peek();
                if (ch == ',') Read(); // consume ','

                EatWhiteSpace();
                ch = (char)Peek();
                if (ch == ']')
                {
                    Read(); // consume ']'
                    done = true;
                }
            }
        }

        private System.Collections.IDictionary CreateHash()
        {
            return System.Activator.CreateInstance(dictionaryType) as System.Collections.IDictionary;
        }
        private System.Collections.IList CreateArray()
        {
            return System.Activator.CreateInstance(arrayType) as System.Collections.IList;
        }
        private object ReadNext()
        {
            EatWhiteSpace();
            char ch = (char)Peek();
            if (ch == '{')
            {
                System.Collections.IDictionary dictionary = CreateHash();
                Read(dictionary);
                if(dictionary.Contains("Type") && types.ContainsKey(dictionary["Type"].ToString()))
                {
                    string typeName = dictionary["Type"].ToString();
                    System.Type type = types[typeName];
                    try
                    {
                        System.Collections.IDictionary instance = System.Activator.CreateInstance(type) as System.Collections.IDictionary;
                        if (!object.ReferenceEquals(null, instance))
                        {
                            Json.Copier.Copy(dictionary, instance);
                            return instance;
                        }
                    }
                    catch(System.Exception ex)
                    {
                        throw new System.Exception("Exception raised while attempting to CreateInstance of Type " 
                            + typeName + 
                            " (" + type.FullName + ")");
                    }
                }
                return dictionary;
            }
            if (ch == '[')
            {
                System.Collections.IList array = CreateArray();
                Read(array);
                return array;
            }
            if (ch == '\'' || ch == '"')
            {
                string strValue = ReadString();
                if(strValue.IndexOf("base64:") == 0)
                {
                    return System.Convert.FromBase64String(strValue.Substring(7));
                }
                return strValue;
            }
            if (ch == 't' || ch == 'f') return ReadBool();
            if (System.Char.IsDigit(ch) || ch == '-') return ReadDouble();
            if (ch == 'n') { Read(); Read(); Read(); Read(); }// consume n,u,l,l
            return null;
        }

        private long Position
        {
            get
            {
                return streamReader.BaseStream.Position;
            }
        }
        private int Peek() { return streamReader.Peek(); }

        private string Buffer
        {
            get
            {
                return new System.String(buffer.ToArray());
            }
        }
        private System.Collections.Generic.Queue<char> buffer =
            new System.Collections.Generic.Queue<char>();
        private int Read()
        {
            int ichar = streamReader.Read();
            if (ichar == -1)
            {
                throw new System.Exception("end of file reached before json read was finished.");
            }
            buffer.Enqueue((char)ichar);
            if (buffer.Count > 255) buffer.Dequeue();
            return ichar;
        }
        private int EatWhiteSpace()
        {
            int ichar = -1;
            while (System.Char.IsWhiteSpace((char)(Peek())))
            {
                int tchar = Read();
            }
            return ichar;
        }
        private bool ReadBool()
        {
            EatWhiteSpace();
            char ch = (char)Peek();
            if (ch == 't')
            {
                Read(); Read(); Read(); Read(); // read chars t,r,u,e
                return true;
            }
            Read(); Read(); Read(); Read(); Read(); // read char f,a,l,s,e
            return false;
        }

        private double ReadDouble()
        {
            EatWhiteSpace();
            char[] endchars = { '}', ']', ',', ' ' };
            return System.Convert.ToDouble(Seek(endchars));
        }
        
        public string ReadString()
        {
            EatWhiteSpace();
            char ch = (char)Peek();
            if (ch != '\'' && ch != '"') throw new System.Exception("found character " + ch + " when a single or double quote was expected. at stream position " + Position.ToString() + " last Key " + lastKey + " buffer: " + Buffer);
            Read(); // consume single or double quote
            string result = Seek(ch);
            Read(); // consume escaped character
            return result;
        }

        private bool IsEscaped
        {
            get
            {
                if(buffer.Count> 0)
                {
                    int backSlashCount = 0;
                    char[] cbuf = buffer.ToArray();
                    if(cbuf[buffer.Count-1] == '\\')
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
        private string Seek(char value)
        {
            string result = "";
            bool escaped = IsEscaped;
            while (Peek() != (int)value ||
                   (Peek() == (int)value && escaped))
            {
                if (Peek() == -1) return result;
                if (Peek() == (int)value && escaped && result.Length > 0) result = result.Substring(0, result.Length - 1);
                int ichar = Read();
                if(escaped && ichar == '\\')
                {
                    // already added the first backslash to the result, skip this one
                }
                else
                {
                    result += (char)ichar;
                    if (ichar == value && !escaped) break;
                }
                escaped = IsEscaped;
            }
            return result;
        }

        private string Seek(char[] values)
        {
            string result = "";
            bool done = false;
            foreach (char ch in values) { if ((char)Peek() == ch) { done = true; } }
            while (!done)
            {
                int ichar = Read();
                result += (char)ichar;
                foreach (char ch in values) { if ((char)Peek() == ch) { done = true; } }
            }
            return result;
        }
    }
}
