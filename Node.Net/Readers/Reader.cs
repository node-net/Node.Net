using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Node.Net.Readers
{
    public sealed class Reader : IRead, IDisposable
    {
        public static Reader Default { get; } = new Reader();
        public Reader()
        {
            Add("Json", new List<string> { "{", "[" }.ToArray(), JsonReader.Read);
            Add("Xml", new List<string> { "<" }.ToArray(), new XmlReader().Read);
            readers.Add("ImageSource", new ImageSourceReader().Read);
            foreach(var signature in ImageSourceReader.Default.Signatures)
            {
                signatureReaders.Add(signature, "ImageSource");
            }
        }
        public JsonReader JsonReader { get; } = new JsonReader();
        public Type DefaultObjectType
        {
            get { return JsonReader.DefaultObjectType; }
            set { JsonReader.DefaultObjectType = value; }
        }
        public Type DefaultDocumentType
        {
            get { return JsonReader.DefaultDocumentType; }
            set { JsonReader.DefaultDocumentType = value; }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Reader()
        {
            Dispose(false);
        }
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (signatureReader != null)
                {
                    signatureReader.Dispose();
                    signatureReader = null;
                }
            }
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
        public object Read(Type type,string name) { return IReadExtension.Read(this, type, name); }
        public object Read(Assembly assembly,string name) { return IReadExtension.Read(this, assembly, name); }
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

        public object Open(string openFileDialogFilter = "JSON Files (.json)|*.json|All Files (*.*)|*.*")
        {
            var ofd = new Microsoft.Win32.OpenFileDialog { Filter = openFileDialogFilter };
            var result = ofd.ShowDialog();
            if (result == true)
            {
                try
                {
                    return Read(ofd.FileName);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unable to open file {ofd.FileName}", ex);
                }
            }
            return null;
        }
    }
}
