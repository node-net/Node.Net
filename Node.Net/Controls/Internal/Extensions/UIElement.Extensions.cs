using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Controls.Internal.Extensions
{
    static class UIElementExtensions
    {
        public static ImageSource ToImageSource(UIElement element, Size size)
        {
            element.Measure(size);
            element.Arrange(new Rect(size));

            var bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(element);
            return bmp;
        }
    }
}
