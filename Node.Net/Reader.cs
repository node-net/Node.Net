using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Node.Net
{
    /// <summary>
    /// Reader
    /// </summary>
    [Serializable]
    public sealed class Reader : Dictionary<string, Func<Stream, object>>, IRead
    {
        public Reader()
        {
            Add("<", ReadXml);
            Add("[", ReadJSON);
            Add("{", ReadJSON);
            foreach (string? signature in new Internal.ImageSourceReader().Signatures)
            {
                Add(signature, ReadImageSource);
            }
        }

        ~Reader()
        {
            jsonReader = null;
            Clear();
        }

        public object Read(Stream stream)
        {
            using (Internal.SignatureReader? signatureReader = new Internal.SignatureReader(stream))
            {
                Stream? stream2 = signatureReader.Stream;
                string? signature = signatureReader.Signature;
                foreach (string signature_key in Keys)
                {
                    if (signature.IndexOf(signature_key) == 0)
                    {
                        object? item = this[signature_key](stream2);
                        //stream.Close();
                        return item;
                    }
                }
                throw new UnrecognizedSignatureException($"unrecognized signature '{signature.Substring(0, 24)}'");
            }
        }

#pragma warning disable CS8603 // Possible null reference return.

        public object Read(string filename) => IReadExtension.Read(this, filename);

#pragma warning restore CS8603 // Possible null reference return.

        private T Convert<T>(object instance)
        {
            if (instance == null)
            {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
#pragma warning disable CS8603 // Possible null reference return.
                return default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
            }

            if (instance is T t)
            {
                return t;
            }
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
#pragma warning disable CS8603 // Possible null reference return.
            return default;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
        }

        public T Read<T>(Stream stream) => Convert<T>(Read(stream));

        public T Read<T>(string filename) => Convert<T>(Read(filename));

        private readonly List<string> xaml_markers = new List<string>
        {
            "http://schemas.microsoft.com/winfx/2006/xaml/presentation",
            "<MeshGeometry3D",
        };

        public object? ReadXml(Stream original_stream)
        {
            using (Internal.SignatureReader? signatureReader = new Internal.SignatureReader(original_stream))
            {
                string? filename = original_stream.GetFileName();
                Stream? stream = signatureReader.Stream;
                string? signature_string = signatureReader.Signature;
                foreach (string? marker in xaml_markers)
                {
                    if (signature_string.Contains(marker))
                    {
                        object? item = System.Windows.Markup.XamlReader.Load(stream);
                        item.SetFileName(filename);
                        return item;
                    }
                }
                if (signature_string.IndexOf("<") == 0)
                {
                    System.Xml.XmlDocument? xdoc = new System.Xml.XmlDocument();
                    xdoc.Load(stream);
                    xdoc.SetFileName(filename);
                    return xdoc;
                }
            }
            return null;
        }

        public object ReadJSON(Stream stream)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            object? i = jsonReader.Read(stream);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (i is IDictionary dictionary)
            {
                dictionary.DeepUpdateParents();
            }

            return i;
        }

        public static object ReadImageSource(Stream stream) => new Internal.ImageSourceReader().Read(stream);

        public Type DefaultDocumentType
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            get { return jsonReader.DefaultDocumentType; }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            set { jsonReader.DefaultDocumentType = value; }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public Type DefaultObjectType
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            get { return jsonReader.DefaultObjectType; }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            set { jsonReader.DefaultObjectType = value; }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public Dictionary<string, Type> ConversionTypeNames
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            get { return jsonReader.ConversionTypeNames; }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            set { jsonReader.ConversionTypeNames = value; }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private Internal.JsonReader? jsonReader = new Internal.JsonReader();

        public object? Open(string name)
        {
            if (name.IsFileDialogFilter())
            {
                string? openFileDialogFilter = name;
                Microsoft.Win32.OpenFileDialog? ofd = new Microsoft.Win32.OpenFileDialog { Filter = openFileDialogFilter };
                bool? result = ofd.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        return this.Read(ofd.FileName);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Unable to open file {ofd.FileName}", ex);
                    }
                }
            }
            else
            {
                if (name.IsValidFileName())
                {
                    return this.Read(name);
                }
            }
            return null;
        }

        private Reader(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}