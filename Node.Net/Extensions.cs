using System.Drawing;
using System.Windows.Media;

namespace Node.Net
{
    public static class Extension
    {
        public static ImageSource GetImageSource(this Image image)
        {
            return Extensions.Image.GetImageSource(image);
        }
    }
}