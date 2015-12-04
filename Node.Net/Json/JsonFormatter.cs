using System;
using System.IO;
using System.Runtime.Serialization;

namespace Node.Net.Json
{
    public class JsonFormatter : IFormatter
    {
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
            return Json.Internal.JsonReader.Load(serializationStream);
        }

        public void Serialize(Stream serializationStream, object graph)
        {
            Json.Internal.JsonWriter.Write(serializationStream, graph, Internal.Style.Compact);
        }
    }
}
