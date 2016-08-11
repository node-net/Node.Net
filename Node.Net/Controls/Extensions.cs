using Node.Net.Controls.Internal.Extensions;
using System.Windows;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public static class Extensions
    {
        public static string GetKey(this object value) => ObjectExtensions.GetKey(value);
        public static object GetValue(this object value) => ObjectExtensions.GetValue(value);
        public static ImageSource ToImageSource(this UIElement element, Size size) => UIElementExtensions.ToImageSource(element, size);
    }
}
