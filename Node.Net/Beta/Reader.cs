using System.Collections;
using System.IO;

namespace Node.Net.Beta
{
    public sealed class Reader
    {
        public object Read(Stream original_stream)
        {
            /*
            var signatureReader = new Internal.Readers.SignatureReader();
            var signature = signatureReader.Read(original_stream) as Internal.Readers.Signature;
            var stream = original_stream;
            if (!stream.CanSeek) stream = signatureReader.MemoryStream;
            foreach (string signature_key in signatureReaders.Keys)
            {
                if (signature.Text.IndexOf(signature_key) == 0 ||
                   signature.HexString.IndexOf(signature_key) == 0)
                {
                    var instance = readers[signatureReaders[signature_key]](stream);
                    if (instance != null && Types != null && typeof(IDictionary).IsAssignableFrom(instance.GetType()))
                    {
                        instance = IDictionaryExtension.ConvertTypes(instance as IDictionary, Types, TypeKey);
                    }
                    return instance;
                }
            }
            //if (UnrecognizedSignatureReader != null) return UnrecognizedSignatureReader.Read(original_stream);
            throw new System.Exception($"unrecognized signature '{signature.HexString}' {signature.Text}");
            */
            return null;
        }
    }
}
