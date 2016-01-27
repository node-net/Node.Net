using System;
using System.Collections.Generic;
namespace Node.Net.Json
{
    public enum JsonFormat { Compressed, Indented };
    public class Writer //: IDisposable
    {
        private Internal.JsonWriter writer = new Internal.JsonWriter();

        public bool IgnoreNullValues
        {
            get { return writer.IgnoreNullValues; }
            set { writer.IgnoreNullValues = value; }
        }
        public List<Type> IgnoreTypes
        {
            get { return writer.IgnoreTypes; }
            set { writer.IgnoreTypes = value; }
        }
        /*
        private bool ignoreNullValues = false;
        public bool IgnoreNullValues
        {
            get { return ignoreNullValues; }
            set { ignoreNullValues = value; }
        }*/
        /*
        private System.Collections.Generic.List<System.Type> ignoreTypes 
            = new System.Collections.Generic.List<System.Type>();

        public System.Collections.Generic.List<System.Type> IgnoreTypes => ignoreTypes;
        */
        /*
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (streamWriter != null) streamWriter.Dispose();
            }
        }*/

        public static void Write(System.Collections.IDictionary dictionary, System.IO.Stream stream,JsonFormat format = JsonFormat.Indented)
        {
            Internal.JsonWriter writer = new Internal.JsonWriter();
            if (format == JsonFormat.Indented) writer.Style = Internal.Style.Indented;
            else writer.Style = Internal.Style.Compact;
            writer.Write(stream, dictionary);

            /*
            Writer writer = new Writer();
            writer.Format = format;
            writer.Write(stream, dictionary);*/
        }
        public static void Write(System.Collections.IEnumerable enumerable, System.IO.Stream stream,JsonFormat format = JsonFormat.Indented)
        {
            Internal.JsonWriter writer = new Internal.JsonWriter();
            if (format == JsonFormat.Indented) writer.Style = Internal.Style.Indented;
            else writer.Style = Internal.Style.Compact;
            writer.Write(stream, enumerable);
            /*
            Writer writer = new Writer();
            writer.Format = format;
            writer.Write(stream, enumerable);*/
        }

        public static string ToString(System.Collections.IDictionary dictionary)
        {
            string result = "";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                Write(dictionary,memory);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        public static string ToString(System.Collections.IEnumerable enumerable)
        {
            string result = "";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                Write(enumerable, memory);
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(memory))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        private int level = 0;
        

        //private System.IO.StreamWriter streamWriter = null;
        public void Write(System.IO.Stream stream, System.Collections.IDictionary dictionary)
        {
            writer.Write(stream, dictionary);
            /*
            streamWriter = new System.IO.StreamWriter(stream);
            Write(dictionary);
            streamWriter.Flush();*/
        }
        public void Write(System.IO.Stream stream, System.Collections.IEnumerable enumerable)
        {
            writer.Write(stream, enumerable);
            /*
            streamWriter = new System.IO.StreamWriter(stream);
            Write(enumerable);
            streamWriter.Flush();*/
        }
        /*
        private void Write(System.Collections.IDictionary dictionary)
        {
            streamWriter.Write(GetIndent() + "{");
            ++level;
            int index = 0;
            foreach (object key in dictionary.Keys)
            {
                object item = dictionary[key];
                bool skipItem = false;
                if (!object.ReferenceEquals(null, item) && ignoreTypes.Contains(item.GetType())) skipItem = true;
                if (ignoreNullValues && object.ReferenceEquals(null, item)) skipItem = true;
                if (!skipItem)
                {
                    if (index > 0) streamWriter.Write(",");
                    streamWriter.Write(GetIndent());
                    Write(key.ToString());
                    streamWriter.Write(":");
                    Write(dictionary[key]);
                    ++index;
                }
            }
            --level;
            if (level == 0 && Format == JsonFormat.Indented) streamWriter.Write(System.Environment.NewLine);
            streamWriter.Write(GetIndent() + "}");
            return;
        }*/
        /*
        private void Write(System.Collections.IEnumerable enumerable)
        {
            
            streamWriter.Write(GetIndent() + "[");
            ++level;
            System.Collections.IEnumerator denum = enumerable.GetEnumerator();
            int index = 0;
            while (denum.MoveNext())
            {
                if (index > 0) streamWriter.Write("," + GetIndent());
                else streamWriter.Write(GetIndent());
                Write(denum.Current);
                ++index;
            }
            --level;
            if (level == 0 && Format == JsonFormat.Indented) streamWriter.Write(System.Environment.NewLine);
            streamWriter.Write(GetIndent() + "]");
        }*/
        /*
        private void Write(byte[] bytes)
        {
            Write("base64:" + System.Convert.ToBase64String(bytes));
        }

        private void Write(string svalue)
        {

            if (svalue.Contains("\\"))
            {
                svalue = EscapeBackslashes(svalue);
            }
            if (svalue.Contains("\""))
            {
                svalue = EscapeDoubleQuotes(svalue);
            }
            streamWriter.Write("\"" + svalue + "\"");
            return;
        }
        private void Write(object value)
        {
            if (object.ReferenceEquals(null, value)) { streamWriter.Write("null"); return; }
            if (value.GetType() == typeof(System.Boolean))
            {
                if (((bool)value)) { streamWriter.Write("true"); return; }
                else { streamWriter.Write("false"); return; }
            }
            if (value.GetType() == typeof(System.Double) ||
                value.GetType() == typeof(System.Single) ||
                value.GetType() == typeof(System.Byte) ||
                value.GetType() == typeof(System.Int16) ||
                value.GetType() == typeof(System.Int32) ||
                value.GetType() == typeof(System.Int64) ) { streamWriter.Write(value.ToString()); return; }
            if (value.GetType() == typeof(System.String))
            {
                Write((string)value);
                return;
            }
            if(value.GetType() == typeof(byte[]))
            {
                Write((byte[])value);
                return;
            }
            System.Collections.IDictionary idictionary = value as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, idictionary))
            {
                Write(idictionary);
                return;
            }
            System.Collections.IEnumerable ienumerable = value as System.Collections.IEnumerable;
            if (!object.ReferenceEquals(null, ienumerable))
            {
                Write(ienumerable);
                return;
            }
            throw new System.InvalidOperationException("attempted to write unsupported type " + value.GetType().FullName);
        }

        private static string EscapeDoubleQuotes(string input)
        {
            string result = input;
            if(input.Contains("\""))
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                char lastChar = 'a';
                for(int i = 0; i < input.Length; ++i)
                {
                    char ch = input[i];
                    if(ch == '"')
                    {
                        builder.Append('\\');
                        builder.Append(ch);
                    }
                        
                    else
                    {
                        builder.Append(ch);
                    }
                    lastChar = ch;
                }
                result = builder.ToString();
            }
            return result;
        }
        private static string EscapeBackslashes(string input) => input.Replace("\\", "\\\\");
        private JsonFormat format = JsonFormat.Indented;

        public JsonFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        private string GetIndent()
        {
            if (Format == JsonFormat.Compressed) return "";
            if (level == 0) return "";
            return System.Environment.NewLine + "".PadRight(level * 2);
        }
        */
    }
}
