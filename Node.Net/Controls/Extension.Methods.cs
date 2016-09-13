using Node.Net.Controls.Internal.Extensions;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Controls
{
    public static class ExtensionMethods
    {
        public static string GetKey(this object value) => ObjectExtensions.GetKey(value);
        public static object GetValue(this object value) => ObjectExtensions.GetValue(value);
        public static ImageSource ToImageSource(this UIElement element, System.Windows.Size size) => UIElementExtensions.ToImageSource(element, size);
        public static ImageSource ToImageSource(this Icon icon) => IconExtensions.ToImageSource(icon);
        public static ImageSource ToImageSource(this System.Drawing.Image image) => ImageExtensions.ToImageSource(image);
        public static void RemoveChild(this DependencyObject parent, UIElement child) => Extensions.DependencyObjectExtension.RemoveChild(parent, child);
        public static void RemoveFromParent(this DependencyObject child) => Extensions.DependencyObjectExtension.RemoveFromParent(child);
        public static void RemoveChild(this DependencyObject parent, Visual3D v3d) => Extensions.Visual3DExtension.RemoveChild(parent, v3d);
    }
}
