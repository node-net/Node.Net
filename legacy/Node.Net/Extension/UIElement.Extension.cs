using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net
{
    public static class UIElementExtension
    {
        public static ImageSource CreateBitmapSource(this UIElement visual, double width, double height, Visual backgoundVisual)
        {
            if (width <= 0.0 || height <= 0.0) return null;
            var background_bitmap = backgoundVisual.CreateBitmapSource(width, height);
            var grid = new Grid();
            grid.Children.Add(new Image { Source = background_bitmap });
            grid.Children.Add(visual);
            return grid.CreateBitmapSource(width, height);
        }
    }
}
