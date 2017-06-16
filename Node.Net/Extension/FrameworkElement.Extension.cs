using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public static class FrameworkElementExtension
    {
        public static readonly float DpiX;
        public static readonly float DpiY;

        static FrameworkElementExtension()
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                DpiX = g.DpiX;
                DpiY = g.DpiY;
            }
        }
        public static System.Windows.Media.Color GetColor(this FrameworkElement element, double x, double y)
        {
            var renderTargetBitmap = new RenderTargetBitmap(
            (int)element.ActualWidth,
            (int)element.ActualHeight,
            DpiX, DpiY, PixelFormats.Default);
            renderTargetBitmap.Render(element);

            if (x <= renderTargetBitmap.PixelWidth && y <= renderTargetBitmap.PixelHeight)
            {
                var croppedBitmap = new CroppedBitmap(
                    renderTargetBitmap, new Int32Rect((int)x, (int)y, 1, 1));
                var pixels = new byte[4];
                croppedBitmap.CopyPixels(pixels, 4, 0);
                return System.Windows.Media.Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
            }
            return Colors.Transparent;
        }
        public static double GetMatchingPixelPercentage(this FrameworkElement element, FrameworkElement element2)
        {
            if (element.ActualHeight == 0 || element.ActualWidth == 0) return 1.0;
            if (element.ActualHeight != element2.ActualHeight)
            {
                throw new InvalidOperationException("elements must be the same size");
            }
            if (element.ActualWidth != element2.ActualWidth)
            {
                throw new InvalidOperationException("element must be the same size");
            }
            var totalPixels = 0;
            var matchingPixels = 0;
            for (double x = 0; x <= element.ActualWidth; x++)
            {
                for (double y = 0; y <= element.ActualHeight; y++)
                {
                    var color = element.GetColor(x, y);
                    var color2 = element2.GetColor(x, y);
                    if (color == color2) ++matchingPixels;
                    ++totalPixels;
                }
            }
            return matchingPixels / totalPixels;
        }
    }
}
