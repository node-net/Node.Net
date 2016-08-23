using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Factory.Factories.TypeFactories
{
    public class ImageSourceFactory : Generic.TypeFactory<ImageSource>, IFactory
    {
        public object Create(Type type, object value)
        {
            if (value == null) return null;
            if (typeof(UIElement).IsAssignableFrom(value.GetType())) return Create(value as UIElement);
            return null;
        }

        public static ImageSource Create(UIElement element)
        {
            var renderSize = element.RenderSize;
            return Create(element, element.RenderSize);
        }

        public static ImageSource Create(UIElement element,Size size)
        {
            element.Measure(size);
            element.Arrange(new Rect(size));

            var bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(element);
            return bmp;
        }
    }
}
