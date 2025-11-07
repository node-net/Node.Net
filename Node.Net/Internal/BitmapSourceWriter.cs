#if IS_WINDOWS
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Node.Net.Internal
{
    internal class BitmapSourceWriter
    {
        public void Write(Stream stream, object value)
        {
            if (value == null)
            {
                return;
            }

            if (value is BitmapSource bitmapSource && BitmapEncoder != null && Activator.CreateInstance(BitmapEncoder.GetType()) is BitmapEncoder encoder)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
            }
        }

        public BitmapEncoder BitmapEncoder { get; set; } = new JpegBitmapEncoder();
    }
}
#endif