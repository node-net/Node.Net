namespace Node.Net.Model3D
{
    public class Resources : System.Windows.ResourceDictionary
    {
        public enum Format { Xaml, Json };
        public Resources() { }
        public Resources(System.IO.Stream stream) { Open(stream); }
        public void Open(System.IO.Stream stream)
        {
            Clear();
            try
            {
                Open(stream, Format.Xaml);
            }
            catch
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
                Open(stream, Format.Json);
            }
        }

        private void Open(System.IO.Stream stream,Format format)
        {
            Clear();
            if(format == Format.Json)
            {
                //SetBase64Dictionary(Node.Net.Json.Reader.ReadHash(stream));
            }
            else
            {
                System.Windows.ResourceDictionary dictionary
                    = (System.Windows.ResourceDictionary)System.Windows.Markup.XamlReader.Load(stream);
                foreach (object key in dictionary.Keys)
                {
                    object value = dictionary[key];

                    Add(key, value);
                }
            }
        }

        public void Save(System.IO.Stream stream,Format format = Format.Xaml)
        {
            if(format == Format.Json)
            {
                //Node.Net.Json.Writer.Write(GetBase64Dictionary(), stream);
            }
            else System.Windows.Markup.XamlWriter.Save(this, stream);
        }

        private System.Collections.IDictionary GetBase64Dictionary()
        {
            System.Collections.Generic.Dictionary<string, string> dictionary
                = new System.Collections.Generic.Dictionary<string, string>();
            foreach(string key in Keys)
            {
                System.IO.MemoryStream memory = new System.IO.MemoryStream();
                System.Windows.Markup.XamlWriter.Save(this[key], memory);
                memory.Flush();
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                System.IO.StreamReader reader = new System.IO.StreamReader(memory);
                dictionary[key] = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd()));
            }
            return dictionary;
        }

        private void SetBase64Dictionary(System.Collections.IDictionary dictionary)
        {
            Clear();
            foreach(string key in dictionary.Keys)
            {
                if (dictionary[key].GetType() == typeof(string))
                {
                    System.IO.MemoryStream memory = new System.IO.MemoryStream();
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(memory);
                    string xaml = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(dictionary[key].ToString()));
                    writer.Write(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(dictionary[key].ToString())));
                    writer.Flush();
                    memory.Flush();
                    memory.Seek(0, System.IO.SeekOrigin.Begin);
                    Add(key, System.Windows.Markup.XamlReader.Load(memory));
                }
            }
        }
    }
}
