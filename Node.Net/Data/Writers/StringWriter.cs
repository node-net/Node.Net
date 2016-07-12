using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data.Writers
{
    public class StringWriter : IWrite
    {
        public void Write(Stream stream,object value)
        {
            if(value != null)
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(value.ToString());
                }
            }
        }
    }
}
