using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Controls.Internal.Extensions
{
    static class ImageExtensions
    {
        public static ImageSource ToImageSource(System.Drawing.Image image)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            var memory = new System.IO.MemoryStream();
            image.Save(memory, ImageFormat.Bmp);
            memory.Seek(0, SeekOrigin.Begin);

            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
