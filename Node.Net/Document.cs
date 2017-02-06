using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Document : Element, IDocument//, Node.Net.Readers.IElement
    {
        public string FileName
        {
            get { return fileName; }
            set
            {
                SetField<string>(ref fileName, value);

            }
        }
        private string fileName;
        /*
        public static Document Open(string name = "")
        {
            using (var reader = new Node.Net.Reader { DefaultDocumentType = typeof(Document) })
            {
                var doc = reader.Open(name) as Document;
                if (doc != null) doc.DeepUpdateParents();
                return doc;
            }
        }*/

        public IDocument Load(string name)
        {
            var doc = reader.Read(name) as IDocument;
            Clear();
            FileName = doc.FileName;
            foreach(var key in doc.Keys)
            {
                this[key] = doc[key];
                //Set(key, doc.Get(key));
            }
            this.DeepUpdateParents();
            return this;
        }
        public IDocument Load(Stream stream)
        {
            var doc = reader.Read(stream) as IDocument;
            Clear();
            FileName = doc.FileName;
            foreach (var key in doc.Keys)
            {
                this[key] = doc[key];
                //Set(key, doc.Get(key));
            }
            this.DeepUpdateParents();
            return this;
        }
        public Dictionary<string, Type> ConversionTypeNames { get { return reader.Types; } }
        private readonly Reader reader = new Reader
        {
            DefaultDocumentType = typeof(Document),
            DefaultObjectType = typeof(Element)
        };
    }
}
