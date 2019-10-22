using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Beta.Internal
{
    public static class ImageExtension
    {
        public static ImageSource GetImageSource(this System.Drawing.Image image)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            var memory = new System.IO.MemoryStream();
            image.Save(memory, ImageFormat.Bmp);
            memory.Seek(0, SeekOrigin.Begin);

            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            memory = null;
            return bitmapImage;
        }
    }
}
