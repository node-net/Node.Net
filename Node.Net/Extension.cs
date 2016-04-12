using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace Node.Net
{
    public static class Extension
    {
        #region IDictionary
        public static void Save(IDictionary dictionary, Stream stream) => Extensions.IDictionaryExtension.Save(dictionary, stream);
        public static void SavE(IDictionary dictionary, string filename) => Extensions.IDictionaryExtension.Save(dictionary, filename);

        #endregion

        #region TextReader
        public static void EatWhiteSpace(this TextReader reader) => Extensions.TextReaderExtension.EatWhiteSpace(reader);

        public static string Seek(this TextReader reader, char value) => Extensions.TextReaderExtension.Seek(reader, value);
        public static string Seek(this TextReader reader, char[] values) => Extensions.TextReaderExtension.Seek(reader, values);
        #endregion

        public static ImageSource GetImageSource(this Image image) => Extensions.ImageExtension.GetImageSource(image);
        public static void Save(this ImageSource imageSource,string filename) => Extensions.ImageSourceExtension.Save(imageSource, filename);
        public static ImageSource Crop(this ImageSource imageSource, int width, int height) => Extensions.ImageSourceExtension.Crop(imageSource, width, height);
    }
}