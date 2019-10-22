using System.IO;
using System.Windows.Media.Imaging;

namespace Node.Net.Deprecated.Data.Writers
{
    public class BitmapImageWriter : IWrite
    {
        public void Write(Stream stream, object value)
        {
            var bitmapSource = value as BitmapSource;
            if (bitmapSource != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
            }
        }
    }
}
