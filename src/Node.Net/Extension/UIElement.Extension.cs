using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
	public static class UIElementExtension
	{
		public static ImageSource GetImageSource(this UIElement element, int pixelWidth, int pixelHeight, double dpiX, double dpiY)
		{
			var desiredSize = new Size(pixelWidth, pixelHeight);
			element.Measure(new Size(pixelWidth, pixelHeight));
			element.Arrange(new Rect(new Point(0, 0), desiredSize));
			element.UpdateLayout();

			var bitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Default);
			bitmap.Render(element);
			return bitmap;
		}
	}
}
