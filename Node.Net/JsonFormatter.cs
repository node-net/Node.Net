using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Node.Net
{
    public class JsonFormatter : IFormatter
    {
        public JsonFormatter()
        {
            Binder = null;
            Context = new StreamingContext();
            SurrogateSelector = null;
        }

        public object? Deserialize(Stream serializationStream) { return _jsonReader.Read(serializationStream); }

        public void Serialize(Stream serializationStream, object graph) { _jsonWriter.Write(serializationStream, graph); }

        public object Clone(object graph) => IFormatterExtension.Clone(this, graph);
        public T Clone<T>(object graph) => IFormatterExtension.Clone<T>(this, graph);

        private readonly Internal.JsonReader _jsonReader = new Internal.JsonReader();
        private readonly Internal.JsonWriter _jsonWriter = new Internal.JsonWriter();

        public SerializationBinder? Binder { get; set; }

        public StreamingContext Context { get; set; }

        public ISurrogateSelector? SurrogateSelector { get; set; }

        public string ToJson(object graph)
        {
            using var memory = new MemoryStream();
            _jsonWriter.Write(memory, graph);
            memory.Seek(0, SeekOrigin.Begin);
            return new StreamReader(memory).ReadToEnd();
        }
    }
}
