using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public class Document : Element
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

        public static Document Open(string name = "")
        {
            using (var reader = new Node.Net.Reader { DefaultDocumentType = typeof(Document) })
            {
                return reader.Open(name) as Document;
            }
        }
    }
}
