using System.IO;

namespace Node.Net.Deprecated.Data.Readers
{
    sealed class XamlReader : IRead
    {
        public static XamlReader Default { get; } = new XamlReader();
        public object Read(Stream stream) => System.Windows.Markup.XamlReader.Load(stream);
    }
}
