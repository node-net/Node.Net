using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Readers
{
    public class ImageSourceReader : IRead
    {
        public object Read(Stream stream)
        {
            return GetImageSource(System.Drawing.Image.FromStream(stream));
        }

        public static ImageSource GetImageSource(System.Drawing.Image image)
        {
            if (!object.ReferenceEquals(null, image))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                var memory = new System.IO.MemoryStream();
                image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Seek(0, SeekOrigin.Begin);

                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            return null;
        }
    }
}
