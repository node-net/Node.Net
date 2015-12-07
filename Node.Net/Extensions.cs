using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace Node.Net
{
    public static class Extension
    {
        public static ImageSource GetImageSource(this Image image) => Extensions.ImageExtension.GetImageSource(image);
        public static void Save(this ImageSource imageSource,string filename) => Extensions.ImageSourceExtension.Save(imageSource, filename);
        public static ImageSource Crop(this ImageSource imageSource, int width, int height) => Extensions.ImageSourceExtension.Crop(imageSource, width, height);
        public static void CopyToFile(this Stream source, string filename) => Extensions.StreamExtension.CopyToFile(source, filename);
    }
}