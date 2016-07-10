using System.IO;

namespace Node.Net.Data.Readers
{
    public class XamlReader : IRead
    {
        public object Read(Stream stream) => System.Windows.Markup.XamlReader.Load(stream);
    }
}
