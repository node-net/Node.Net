using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Writers
{
    public class Writer
    {
        public static Writer Default { get; } = new Writer();
        public void Write(Stream stream, object value)
        {

        }
    }
}
