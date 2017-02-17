using System.IO;
using System.Xml;

namespace Node.Net.Data.Readers
{
    sealed class XmlReader : IRead
    {
        public static XmlReader Default { get; } = new XmlReader();
        public object Read(Stream stream)
        {
            var xdoc = new XmlDocument();
            xdoc.Load(stream);
            if (xdoc.DocumentElement.HasAttribute("xmlns"))
            {
                var value = xdoc.DocumentElement.GetAttribute("xmlns");
                if (value.Contains("http://schemas.microsoft.com/winfx/2006/xaml/presentation"))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return System.Windows.Markup.XamlReader.Load(stream);
                }
            }
            var ns = xdoc.GetNamespaceOfPrefix("xmlns");
            return xdoc;
        }
    }
}
