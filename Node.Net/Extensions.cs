using System.Drawing;
using System.Windows.Media;

namespace Node.Net
{
    public static class Extension
    {
        public static ImageSource GetImageSource(this Image image) => Extensions.ImageExtension.GetImageSource(image);
        public static void Save(this ImageSource imageSource,string filename) => Extensions.ImageSourceExtension.Save(imageSource, filename);
    }
}