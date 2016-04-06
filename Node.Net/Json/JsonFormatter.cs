using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Node.Net.Json
{
    public enum JsonStyle { Compact,Indented};
    public class JsonFormatter : IFormatter
    {

        private Json.Internal.JsonReader reader = new Internal.JsonReader();
        public Type DefaultListType
        {
            get { return reader.DefaultListType; }
            set { reader.DefaultListType = value; }
        }
        public Type DefaultDictionaryType
        {
            get { return reader.DefaultDictionaryType; }
            set { reader.DefaultDictionaryType = value; }
        }
        public Dictionary<string, Type> Types
        {
            get { return reader.Types; }
            set { reader.Types = value; }
        }

        public JsonStyle Style = JsonStyle.Compact;
        public static void Save(Stream stream, object graph)
        {
            JsonFormatter formatter = new JsonFormatter();
            formatter.Serialize(stream, graph);
        }
        public static void Load(Stream stream, IDictionary dictionary)
        {
            JsonFormatter formatter = new JsonFormatter();
            IDictionary d = (IDictionary)formatter.Deserialize(stream);
            Collections.Copier.Copy(d, dictionary);
        }

        public static void Load(Stream stream, IDictionary dictionary,Type dictionaryType)
        {
            JsonFormatter formatter = new JsonFormatter();
            IDictionary d = (IDictionary)formatter.Deserialize(stream);
            Collections.Copier.Copy(d, dictionary);
        }

        private SerializationBinder binder = new JsonSerializationBinder();
        public SerializationBinder Binder
        {
            get { return binder; }
            set { binder = value; }
        }

        private StreamingContext context = new StreamingContext();
        public StreamingContext Context
        {
            get { return context; }
            set { context = value; }
        }

        private ISurrogateSelector surrogateSelector = new SurrogateSelector();
        public ISurrogateSelector SurrogateSelector
        {
            get { return surrogateSelector; }
            set { surrogateSelector = value; }
        }

        public object Deserialize(Stream serializationStream)
        {
            return reader.Read(serializationStream);
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            if (Style == JsonStyle.Compact)
            {
                Json.Internal.JsonWriter.Write(serializationStream, graph, Internal.Style.Compact);
            }
            else
            {
                Json.Internal.JsonWriter.Write(serializationStream, graph, Internal.Style.Indented);
            }
        }
    }
}
