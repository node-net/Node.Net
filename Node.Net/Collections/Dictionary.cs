using System.Collections;
using System.IO;

namespace Node.Net.Collections
{
    public class Dictionary : System.Collections.Generic.Dictionary<string,dynamic>, 
        Framework.IDocument
    {
        private bool readOnly = true;
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = true; }
        }

        public void Open(Stream stream)
        {
            Clear();
            Json.Hash hash = Json.Reader.ReadHash(stream);
            Json.Copier.Copy(hash, this);
        }
    }
}
