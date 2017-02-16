using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Node.Net.Controls.Internal.Extensions
{
    [StructLayout(LayoutKind.Sequential)]
    struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath,
                              uint dwFileAttributes,
                              ref SHFILEINFO psfi,
                              uint cbSizeFileInfo,
                              uint uFlags);
        [DllImport("user32")]
        public static extern int DestroyIcon(IntPtr hIcon);
    }

    class DirectoryIcon
    {
        public static Icon GetDirectoryIcon(string name)
        {
            var shinfo = new SHFILEINFO();
            var hImgSmall = Win32.SHGetFileInfo(name, 0, ref shinfo,
                                  (uint)Marshal.SizeOf(shinfo),
                                   Win32.SHGFI_ICON |
                                   Win32.SHGFI_SMALLICON);
            var myIcon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
            Win32.DestroyIcon(shinfo.hIcon);
            return myIcon;
        }
    }
}
