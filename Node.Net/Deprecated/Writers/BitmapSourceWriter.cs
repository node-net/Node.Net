using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Deprecated.Writers
{
    public class BitmapSourceWriter : IWrite
    {
        public void Write(Stream stream, object value)
        {
            if (value == null) return;
            var bitmapSource = value as BitmapSource;
            if(bitmapSource != null && BitmapEncoder != null)
            {
                var encoder = Activator.CreateInstance(BitmapEncoder.GetType()) as BitmapEncoder;
                if(encoder != null)
                {
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    encoder.Save(stream);
                }
            }

        }

        public BitmapEncoder BitmapEncoder { get; set; } = new JpegBitmapEncoder();
    }
}
