using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Readers
{
    public class TextReader : IRead
    {
        public static TextReader Default { get; } = new TextReader();
        public object Read(Stream stream)
        {
            var text = new List<string>();
            string line;
            using (StreamReader reader = new StreamReader(stream))
            {
                while((line = reader.ReadLine()) != null)
                {
                    text.Add(line);
                }
            }
            return text;
        }
    }
}
