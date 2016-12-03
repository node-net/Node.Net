//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using Node.Net.Controls.Internal.Extensions;
using System.Drawing;
using System.Windows;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public static class Extensions
    {
        public static string GetKey(this object value) => ObjectExtensions.GetKey(value);
        public static object GetValue(this object value) => ObjectExtensions.GetValue(value);
        public static ImageSource ToImageSource(this UIElement element, System.Windows.Size size) => UIElementExtensions.ToImageSource(element, size);
        public static ImageSource ToImageSource(this Icon icon) => IconExtensions.ToImageSource(icon);
        public static ImageSource ToImageSource(this System.Drawing.Image image) => ImageExtensions.ToImageSource(image);
    }
}
