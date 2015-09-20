using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    namespace Extensions
    {
        class ImageExtension
        {
            public static ImageSource GetImageSource(System.Drawing.Image image)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                System.IO.MemoryStream memory = new System.IO.MemoryStream();
                image.Save(memory, ImageFormat.Bmp);
                memory.Seek(0, SeekOrigin.Begin);

                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}