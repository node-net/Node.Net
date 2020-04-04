﻿using Node.Net.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Media;

namespace Node.Net
{
    public class Formatter : IFormatter
    {
        public Formatter()
        {
            Binder = null;
            Context = new StreamingContext();
            SurrogateSelector = null;

            Readers.Add("json", DeserializeJson);
            Readers.Add("png", DeserializeImage);
            Readers.Add("tif", DeserializeImage);
            Readers.Add("jpg", DeserializeImage);
            Readers.Add("gif", DeserializeImage);
            Readers.Add("bmp", DeserializeImage);
            SignatureReaders.Add("42 4D", "bmp");                       // .bmp
            SignatureReaders.Add("47 49 46 38 37 61", "gif");           // .gif
            SignatureReaders.Add("47 49 46 38 39 61", "gif");           // .gif
            SignatureReaders.Add("FF D8 FF E0", "jpg");                 // .jpg
            SignatureReaders.Add("FF D8 FF E1", "jpg");                 // .jpg
            SignatureReaders.Add("49 49 2A 00", "tif");                 // .tif
            SignatureReaders.Add("4D 4D 00 2A", "tif");                 // .tif
            SignatureReaders.Add("89 50 4E 47 0D 0A 1A 0A", "png");     // .png
            SignatureReaders.Add("{", "json");
            SignatureReaders.Add("[", "json");
        }

        public Dictionary<string, string> SignatureReaders { get; } = new Dictionary<string, string>();
        public Dictionary<string, Func<Stream, object?>> Readers { get; } = new Dictionary<string, Func<Stream, object?>>();

        public virtual object? Deserialize(Stream serializationStream)
        {
            using (SignatureReader? signatureReader = new Internal.SignatureReader(serializationStream))
            {
                Stream? stream = signatureReader.Stream;
                string? signature = signatureReader.Signature;
                foreach (string signature_key in SignatureReaders.Keys)
                {
                    if (signature.IndexOf(signature_key) == 0)
                    {
                        object? instance = Readers[SignatureReaders[signature_key]](stream);
                        return instance;
                    }
                }
                throw new System.InvalidOperationException($"unrecognized signature '{signature}'");
            }
        }

        private object? DeserializeJson(Stream stream) => _jsonFormatter.Deserialize(stream);
        private object? DeserializeImage(Stream stream) => _imageReader.Read(stream);

        public virtual void Serialize(Stream serializationStream, object graph)
        {
            _jsonFormatter.Serialize(serializationStream, graph);
        }

        public object Clone(object graph)
        {
            return IFormatterExtension.Clone(this, graph);
        }

        public T Clone<T>(object graph)
        {
            return IFormatterExtension.Clone<T>(this, graph);
        }

        private readonly JsonFormatter _jsonFormatter = new JsonFormatter();
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private readonly ImageSourceReader _imageReader = new ImageSourceReader();

        public SerializationBinder? Binder { get; set; }

        public StreamingContext Context { get; set; }

        public ISurrogateSelector? SurrogateSelector { get; set; }

        public string ToJson(object graph) => _jsonFormatter.ToJson(graph);
    }
}
