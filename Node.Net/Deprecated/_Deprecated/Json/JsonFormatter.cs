﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Node.Net.Deprecated.Json
{
    public enum JsonStyle { Compact,Indented};
    public class JsonFormatter : IFormatter
    {

        private readonly Json.Internal.JsonReader reader = new Internal.JsonReader();
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
            var formatter = new JsonFormatter();
            formatter.Serialize(stream, graph);
        }
        public static void Load(Stream stream, IDictionary dictionary)
        {
            var formatter = new JsonFormatter();
            var d = (IDictionary)formatter.Deserialize(stream);
            Deprecated.Collections.Copier.Copy(d, dictionary);
        }

        public static void Load(Stream stream, IDictionary dictionary,Type dictionaryType)
        {
            var formatter = new JsonFormatter();
            var d = (IDictionary)formatter.Deserialize(stream);
            Deprecated.Collections.Copier.Copy(d, dictionary);
        }
        public SerializationBinder Binder { get; set; } = new JsonSerializationBinder();
        public StreamingContext Context { get; set; } = new StreamingContext();
        public ISurrogateSelector SurrogateSelector { get; set; } = new SurrogateSelector();

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