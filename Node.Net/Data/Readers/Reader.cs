using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Node.Net.Data.Readers
{
    sealed class Reader : IRead
    {
        public Reader() { }
        
        public Reader(Assembly assembly)
        {
            dictionaryTypeConverter = new DictionaryTypeConverter(assembly);
            IDictionaryTypeConversionFunction = dictionaryTypeConverter.Convert;
            //DictionaryTypeConverter = new DictionaryTypeConverter(assembly);
        }
        private readonly Readers.DictionaryTypeConverter dictionaryTypeConverter;

        //public static Reader Default { get; } = new Reader();
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
        private Dictionary<string, IRead> textSignatureReaders;
        public Dictionary<string, IRead> TextSignatureReaders
        {
            get
            {
                if (textSignatureReaders == null)
                {
                    textSignatureReaders = new Dictionary<string, IRead>();
                    textSignatureReaders.Add("{", jsonReader);
                    textSignatureReaders.Add("[", jsonReader);
                    var xmlReader = new XmlReader();
                    textSignatureReaders.Add("<", xmlReader);
                    textSignatureReaders.Add(":Primitive:", new PrimitiveReader());
                }
                return textSignatureReaders;
            }
            set { textSignatureReaders = value; }
        }

        private Dictionary<byte[], IRead> binarySignatureReaders;
        public Dictionary<byte[], IRead> BinarySignatureReaders
        {
            get
            {
                if (binarySignatureReaders == null)
                {
                    binarySignatureReaders = new Dictionary<byte[], IRead>();
                    var imageSourceReader = new ImageSourceReader();
                    foreach(var signature in ImageSourceReader.BinarySignatures.Keys)
                    {
                        binarySignatureReaders.Add(signature, imageSourceReader);
                    }
                }
                return binarySignatureReaders;
            }
            set { binarySignatureReaders = value; }
        }

        public Func<IDictionary,IDictionary> IDictionaryTypeConversionFunction { get; set; }
        /*
        private IDictionaryTypeConverter dictionaryTypeConverter = new DictionaryTypeConverter(typeof(DictionaryTypeConverter).Assembly);
        public IDictionaryTypeConverter DictionaryTypeConverter
        {
            get { return dictionaryTypeConverter; }
            set {
                if(value == null)
                {
                    throw new System.Exception("setting DictionaryTypeConverter to null");

                }
                dictionaryTypeConverter = value;
            }
        }*/

        public object Read(Stream stream_original)
        {
            var kvp = BytesReader.GetStreamSignature(stream_original);
            var stream = kvp.Key;
            var signature = kvp.Value;
            foreach (var binary_signature in BinarySignatureReaders.Keys)
            {
                if (SignatureMatches(signature, binary_signature))
                {
                    return Convert(BinarySignatureReaders[binary_signature].Read(stream));
                }                
            }
            var text_signature = Encoding.UTF8.GetString(kvp.Value).Trim();
            if (text_signature.Length > 0)
            {
                foreach (var key in TextSignatureReaders.Keys)
                {
                    var index = text_signature.IndexOf(key);
                    if (index == 0 || index == 1)
                    {
                        return Convert(TextSignatureReaders[key].Read(stream));
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
            /*
            if(dictionary != null && DictionaryTypeConverter != null)
            {
                return DictionaryTypeConverter.Convert(dictionary);
            }*/
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
