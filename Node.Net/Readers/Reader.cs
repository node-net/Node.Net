using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Readers
{
    public sealed class Reader : IRead
    {
        public static Reader Default { get; } = new Reader();
        public Reader()
        {
            Add("Json", new List<string> { "{", "[" }.ToArray(), new JsonReader().Read);
            //readers.Add("Json", new JsonReader().Read);
            //signatureReaders.Add("{", "Json");
            //signatureReaders.Add("[", "Json");
            Add("Xml", new List<string> { "<" }.ToArray(), new XmlReader().Read);
            //readers.Add("Xml", new XmlReader().Read);
            //signatureReaders.Add("<", "Xml");
            readers.Add("ImageSource", new ImageSourceReader().Read);
            signatureReaders.Add("42 4D", "ImageSource");                       // .bmp
            signatureReaders.Add("47 49 46 38 37 61", "ImageSource");           // .gif
            signatureReaders.Add("47 49 46 38 39 61", "ImageSource");           // .gif
            signatureReaders.Add("FF D8 FF E0", "ImageSource");                 // .jpg
            signatureReaders.Add("FF D8 FF E1", "ImageSource");                 // .jpg
            signatureReaders.Add("49 49 2A 00", "ImageSource");                 // .tif
            signatureReaders.Add("4D 4D 00 2A", "ImageSource");                 // .tif
            signatureReaders.Add("89 50 4E 47 0D 0A 1A 0A", "ImageSource");     // .png
        }
        public string TypeKey { get; set; } = "Type";
        public string[] Signatures
        {
            get
            {
                var signatures = new List<string>();
                foreach(var signature in signatureReaders.Keys)
                {
                    if (!signatures.Contains(signature)) signatures.Add(signature);
                }
                return signatures.ToArray();
            }
        }
        public Dictionary<string, Type> Types { get; set; } = null;
        private SignatureReader signatureReader = new SignatureReader();
        private Dictionary<string, Func<Stream, object>> readers = new Dictionary<string, Func<Stream, object>>();
        private Dictionary<string, string> signatureReaders = new Dictionary<string, string>();
        public void Add(string name,string[] signatures,Func<Stream,object> readFunction)
        {
            readers.Add(name, readFunction);
            foreach(var signature in signatures)
            {
                if (!signatureReaders.ContainsKey(signature))
                {
                    signatureReaders.Add(signature, name);
                }
            }
        }
        public void SetReader(string name,Func<Stream,object> readFunction)
        {
            readers[name] = readFunction;
        }
        public void Clear()
        {
            readers.Clear();
            signatureReaders.Clear();
        }
        public object Read(string filename) { return IReadExtension.Read(this, filename); }
        public object Read(Stream original_stream)
        {
            var signature = signatureReader.Read(original_stream) as Signature;
            var stream = original_stream;
            if (!stream.CanSeek) stream = signatureReader.MemoryStream;
            foreach (string signature_key in signatureReaders.Keys)
            {
                if (signature.Text.IndexOf(signature_key) == 0 ||
                   signature.HexString.IndexOf(signature_key) == 0)
                {
                    var instance = readers[signatureReaders[signature_key]](stream);
                    if(instance != null && Types != null && typeof(IDictionary).IsAssignableFrom(instance.GetType()))
                    {
                        instance = IDictionaryExtension.ConvertTypes(instance as IDictionary, Types, TypeKey);
                    }
                    return instance;
                }
            }
            throw new System.Exception($"unrecognized signature '{signature.HexString}' {signature.Text}");
        }
    }
}
