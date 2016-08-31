using System;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public sealed class Reader : IRead
    {
        public static Reader Default { get; } = new Reader();
        public object Read(Stream stream)
        {
            return Node.Net.Data.Readers.Reader.Default.Read(stream);
        }
    }
}
