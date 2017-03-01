using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public static class VisualExtension
    {
        public static BitmapSource CreateBitmapSource(this Visual visual, double width, double height,double dpiX=96,double dpiY=96)
        {
            var bmp = new RenderTargetBitmap((Int32)Math.Ceiling(width), (Int32)Math.Ceiling(height), dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(visual);
            return bmp;
        }
    }
}
