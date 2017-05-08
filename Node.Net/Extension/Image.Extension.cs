using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class ImageExtension
    {
        public static System.Windows.Media.ImageSource GetImageSource(this System.Drawing.Image image) => Beta.Internal.ImageExtension.GetImageSource(image);
    }
}
