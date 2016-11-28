using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public sealed class Writer
    {
        public static Writer Default { get; } = new Writer();
        public void Write(Stream stream, object value) { writer.Write(stream, value); }
        public void Write(string filename,object value){ writer.Write(filename, value); }
        private Node.Net.Writers.Writer writer = new Node.Net.Writers.Writer();
    }
}
