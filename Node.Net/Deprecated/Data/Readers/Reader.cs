using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Node.Net.Deprecated.Data.Readers
{
    sealed class Reader : IRead
    {
        public Reader() { }
        
        public Reader(Assembly assembly)
        {
            dictionaryTypeConverter = new DictionaryTypeConverter(assembly);
            IDictionaryTypeConversionFunction = dictionaryTypeConverter.Convert;
        }
        private Readers.DictionaryTypeConverter dictionaryTypeConverter;

        private readonly JsonReader jsonReader = new JsonReader();
        public Type DefaultArrayType
        {
            get { return jsonReader.DefaultArrayType; }
            set { jsonReader.DefaultArrayType = value; }
        }
        public Type DefaultObjectType
        {
            get { return jsonReader.DefaultObjectType; }
            set { jsonReader.DefaultObjectType = value; }
        }

        private Dictionary<string, Func<Stream, object>> textSignatureReadFunctions;
        public Dictionary<string,Func<Stream,object>> TextSignatureReadFunctions
        {
            get
            {
                if(textSignatureReadFunctions == null)
                {
                    textSignatureReadFunctions = new Dictionary<string, Func<Stream, object>>();
                    textSignatureReadFunctions.Add("{", jsonReader.Read);
                    textSignatureReadFunctions.Add("[", jsonReader.Read);
                    textSignatureReadFunctions.Add("<", XmlReader.Default.Read);
                    textSignatureReadFunctions.Add(":Primitive:", PrimitiveReader.Default.Read);
                }
                return textSignatureReadFunctions;
            }
            set { textSignatureReadFunctions = value; }
        }

        private Dictionary<byte[], Func<Stream, object>> binarySignatureReadFunctions;
        public Dictionary<byte[],Func<Stream,object>> BinarySignatureReadFunctions
        {
            get
            {
                if (binarySignatureReadFunctions == null)
                {
                    binarySignatureReadFunctions = new Dictionary<byte[], Func<Stream, object>>();
                    foreach (var signature in ImageSourceReader.BinarySignatures.Keys)
                    {
                        binarySignatureReadFunctions.Add(signature, ImageSourceReader.Default.Read);
                    }
                }
                return binarySignatureReadFunctions;
            }
            set
            {
                binarySignatureReadFunctions = value;
            }
        }

        public Func<IDictionary,IDictionary> IDictionaryTypeConversionFunction { get; set; }

        public object Read(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                return Read(fs);
            }
        }
        public object Read(Stream stream_original)
        {
            var kvp = BytesReader.GetStreamSignature(stream_original);
            var stream = kvp.Key;
            var signature = kvp.Value;
            foreach(var binary_signature in BinarySignatureReadFunctions.Keys)
            {
                if(SignatureMatches(signature,binary_signature))
                {
                    return BinarySignatureReadFunctions[binary_signature](stream);
                }
            }

            var text_signature = Encoding.UTF8.GetString(kvp.Value).Trim();
            if (text_signature.Length > 0)
            {
                foreach(var key in TextSignatureReadFunctions.Keys)
                {
                    var index = text_signature.IndexOf(key);
                    if(index == 0 || index == 1)
                    {
                        return Convert(TextSignatureReadFunctions[key](stream));
                    }
                }
            }
            return new BytesReader().Read(stream);
        }

        private object Convert(object source)
        {
            var dictionary = source as IDictionary;
            if(IDictionaryTypeConversionFunction != null && dictionary !=null)
            {
                return IDictionaryTypeConversionFunction(dictionary);
            }
            return source;
        }

        private static bool SignatureMatches(byte[] test_signature, byte[] target_signature)
        {
            var hex_test_signature = BytesReader.ByteArrayToHexString(test_signature);
            var hex_target_signature = BytesReader.ByteArrayToHexString(target_signature);
            if (hex_test_signature.IndexOf(hex_target_signature) == 0) return true;
            return false;
        }
    }
}
