using System.IO;
using System.Text;

namespace Node.Net.Deprecated.Data.Writers
{
    class PrimitiveWriter : IWrite
    {
        public void Write(Stream stream, object value)
        {
            if (value == null) return;
            using (StreamWriter writer = new StreamWriter(stream,Encoding.UTF8,1024,true))
            {
                writer.WriteLine(":Primitive:");
                writer.WriteLine(value.GetType().FullName);
                writer.WriteLine(value.ToString());
            }
        }
    }
}
