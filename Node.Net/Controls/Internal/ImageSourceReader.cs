using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Node.Net.Controls.Internal
{
    class ImageSourceReader
    {
        public static object Read(Stream stream)
        {
            var image = System.Drawing.Image.FromStream(stream);
            return image.ToImageSource();
        }

        private static ImageSourceReader _default;
        public static ImageSourceReader Default
        {
            get
            {
                if (_default == null) _default = new ImageSourceReader();
                return _default;
            }
        }
    }
}
