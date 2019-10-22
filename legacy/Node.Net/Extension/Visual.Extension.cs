using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public static class VisualExtension
    {
        public static readonly float DpiX;
        public static readonly float DpiY;

        static VisualExtension()
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                DpiX = g.DpiX;
                DpiY = g.DpiY;
            }
        }

        public static BitmapSource CreateBitmapSource(this Visual visual, double width, double height)
        {
            return CreateBitmapSource(visual, width, height, DpiX, DpiY);
        }
        public static BitmapSource CreateBitmapSource(this Visual visual, double width, double height, double dpiX, double dpiY)
        {
            var awidth = width * dpiX / 96.0;
            var aheight = height * dpiX / 96.0;
            var frameworkElement = visual as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.Measure(new System.Windows.Size(awidth, aheight));
                frameworkElement.Arrange(new System.Windows.Rect(new System.Windows.Size(awidth, aheight)));
                frameworkElement.UpdateLayout();
            }
            var bmp = new RenderTargetBitmap((Int32)Math.Ceiling(awidth), (Int32)Math.Ceiling(aheight), dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(visual);
            return bmp;
        }
    }
}
