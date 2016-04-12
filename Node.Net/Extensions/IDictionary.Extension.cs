using System.Collections;
using System.IO;

namespace Node.Net.Extensions
{
    public class IDictionaryExtension
    {
        public static IDictionary Find(IDictionary source, string key, string value)
        {
            if (object.ReferenceEquals(null, source)) return null;
            if (source.Contains(key) && source[key].ToString() == value)
            {
                return source;
            }
            foreach (string child_key in source.Keys)
            {
                IDictionary childDictionary = source[child_key] as IDictionary;
                if (!object.ReferenceEquals(null, childDictionary))
                {
                    IDictionary result = Find(childDictionary, key, value);
                    if (!object.ReferenceEquals(null, result)) return result;
                }
            }
            return null;
        }

        public static void Save(IDictionary dictionary, Stream stream)
        {
            Node.Net.Json.JsonFormatter formatter = new Node.Net.Json.JsonFormatter()
            {
                Style = Node.Net.Json.JsonStyle.Indented
            };
            formatter.Serialize(stream, dictionary);
        }

        public static void Save(IDictionary dictionary, string filename)
        {
            using (FileStream stream = File.OpenWrite(filename))
            {
                Save(dictionary, stream);
            }
        }
    }
}
