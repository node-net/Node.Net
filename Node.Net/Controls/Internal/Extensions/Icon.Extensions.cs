using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;

namespace Node.Net.Controls.Internal.Extensions
{
    static class IconExtensions
    {
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        public static Icon GetIcon(string name)
        {
            switch (name)
            {
                case @"Application":
                    {
                        return SystemIcons.Application;
                    }
                case @"Asterik":
                    {
                        return SystemIcons.Asterisk;
                    }
                case @"Error":
                    {
                        return SystemIcons.Error;
                    }
                case @"Exclaimation":
                    {
                        return SystemIcons.Exclamation;
                    }
                case @"Hand":
                    {
                        return SystemIcons.Hand;
                    }
                case @"Information":
                    {
                        return SystemIcons.Information;
                    }
                case @"Question":
                    {
                        return SystemIcons.Question;
                    }
                case @"Shield":
                    {
                        return SystemIcons.Shield;
                    }
                case @"Warning":
                    {
                        return SystemIcons.Warning;
                    }
                case @"WinLogo":
                    {
                        return SystemIcons.WinLogo;
                    }
            }

            if (File.Exists(name))
            {
                var icon = System.Drawing.Icon.ExtractAssociatedIcon(name);
                if (!object.ReferenceEquals(null, icon)) return icon;
            }
            if (Directory.Exists(name))
            {
                return DirectoryIcon.GetDirectoryIcon(name);
            }
            return null;
        }
    }
}
