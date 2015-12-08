using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace Node.Net
{
    public static class Extension
    {
        #region TextReader
        public static void EatWhiteSpace(this TextReader reader) => IO.TextReaderExtension.EatWhiteSpace(reader);

        public static string Seek(this TextReader reader, char value) => IO.TextReaderExtension.Seek(reader, value);
        public static string Seek(this TextReader reader, char[] values) => IO.TextReaderExtension.Seek(reader, values);
        #endregion

        #region Stream
        public static void CopyToFile(this Stream source, string filename) => IO.StreamExtension.CopyToFile(source, filename);
        public static string GetString(this Stream stream) => IO.StreamExtension.GetString(stream);
        public static void SetString(this Stream stream, string value) => IO.StreamExtension.SetString(stream, value);
        #endregion

        public static ImageSource GetImageSource(this Image image) => Extensions.ImageExtension.GetImageSource(image);
        public static void Save(this ImageSource imageSource,string filename) => Extensions.ImageSourceExtension.Save(imageSource, filename);
        public static ImageSource Crop(this ImageSource imageSource, int width, int height) => Extensions.ImageSourceExtension.Crop(imageSource, width, height);

    }
}