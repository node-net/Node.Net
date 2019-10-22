using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public class BitmapSourceWriter
    {
        public void Write(Stream stream, object value)
        {
            if (value == null) return;
            var bitmapSource = value as BitmapSource;
            if (bitmapSource != null && BitmapEncoder != null)
            {
                var encoder = Activator.CreateInstance(BitmapEncoder.GetType()) as BitmapEncoder;
                if (encoder != null)
                {
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                }
            }

        }

        public BitmapEncoder BitmapEncoder { get; set; } = new JpegBitmapEncoder();
    }
}
