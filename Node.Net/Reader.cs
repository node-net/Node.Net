using System;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    public class Reader : IReader
    {
        private static IReader _default;
        public static IReader Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new Reader();
                }
                return _default;
            }
            set
            {
                _default = value;
            }
        }

        public object Load(Stream stream, string name)
        {
            try
            {
                switch (GetExtension(name).ToLower())
                {
                    case "jpg":
                        {
                            return GetImageSource(stream);
                        };
                    case "xaml":
                        {
                            return XamlReader.Load(stream);
                        }
                }
                return Json.Reader.Default.Load(stream, name);
            }
            catch(Exception e)
            {
                throw new System.Exception($"Exception occured during load of stream {name}", e);
            }
        }

        private string GetExtension(string name)
        {
            string[] words = name.Split(".".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            if (words.Length > 0) return words[words.Length - 1];
            return "";
        }
        public static ImageSource GetImageSource(Stream stream)
        {
            var image = System.Drawing.Image.FromStream(stream);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();

            MemoryStream memory = new MemoryStream();
            image.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Seek(0, SeekOrigin.Begin);

            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            memory = null;
            return bitmapImage;

        }
    }
}
