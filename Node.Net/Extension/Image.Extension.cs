#if IS_WINDOWS
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public static class ImageExtension
    {
        public static ImageSource GetImageSource(this System.Drawing.Image image)
        {
            BitmapImage? bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            MemoryStream? memory = new System.IO.MemoryStream();
            image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Seek(0, SeekOrigin.Begin);

            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
#endif