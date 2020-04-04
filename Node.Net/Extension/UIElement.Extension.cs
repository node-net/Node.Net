using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public static class UIElementExtension
    {
        public static ImageSource GetImageSource(this UIElement element, int pixelWidth, int pixelHeight, double dpiX, double dpiY)
        {
            Size desiredSize = new Size(pixelWidth, pixelHeight);
            element.Measure(new Size(pixelWidth, pixelHeight));
            element.Arrange(new Rect(new Point(0, 0), desiredSize));
            element.UpdateLayout();

            RenderTargetBitmap? bitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Default);
            bitmap.Render(element);
            return bitmap;
        }
    }
}