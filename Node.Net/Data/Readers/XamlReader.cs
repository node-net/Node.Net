using System.IO;

namespace Node.Net.Data.Readers
{
    sealed class XamlReader : IRead
    {
        public object Read(Stream stream) => System.Windows.Markup.XamlReader.Load(stream);
    }
}
