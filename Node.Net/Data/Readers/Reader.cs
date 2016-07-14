using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Node.Net.Data.Readers
{
    public class Reader : IRead
    {
        private Dictionary<string, IRead> textSignatureReaders;
        public Dictionary<string, IRead> TextSignatureReaders
        {
            get
            {
                if (textSignatureReaders == null)
                {
                    textSignatureReaders = new Dictionary<string, IRead>();
                    var jsonReader = new JsonReader();
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
        public object Read(Stream stream_original)
        {
            var kvp = BytesReader.GetStreamSignature(stream_original);
            var stream = kvp.Key;
            var signature = kvp.Value;
            foreach (var binary_signature in BinarySignatureReaders.Keys)
            {
                if (SignatureMatches(signature, binary_signature))
                {
                    return BinarySignatureReaders[binary_signature].Read(stream);
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
                        return TextSignatureReaders[key].Read(stream);
                    }
                }
            }
            return new BytesReader().Read(stream);
        }

        private static bool SignatureMatches(byte[] test_signature, byte[] target_signature)
        {
            var hex_test_signature = BytesReader.ByteArrayToHexString(test_signature);
            var hex_target_signature = BytesReader.ByteArrayToHexString(target_signature);
            if (hex_test_signature.IndexOf(hex_target_signature) == 0) return true;
            return false;
            /*
            Assert.True(hex_signature.Contains(hex), hex_signature);

            if (signature.Length >= binary_signature.Length)
            {
                for(int i = 0; i < binary_signature.Length; ++i)
                {
                    if (binary_signature[i] != signature[i]) return false;
                }
                return true;
            }
            return false;
            */
        }
    }
}
