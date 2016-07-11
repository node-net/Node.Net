using System;
using System.IO;
using System.Text;

namespace Node.Net.Data.Readers
{
    public class LinesReader : IRead
    {
        public object Read(Stream stream)
        {
            using (TextReader reader = new StreamReader(stream, Encoding.Default, true, 1024, true))
            {
                return reader.ReadToEnd().Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            }
        }
    }
}
