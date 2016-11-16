using System.Drawing;
using System.IO;

using System.Net;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    namespace Extensions
    {
        public class ImageSourceExtension
        {
            public static void Save(ImageSource imageSource,string filename)
            {
                var fileInfo = new FileInfo(filename);
                var bitmapSource = imageSource as BitmapSource;
                using (System.IO.Stream fs = System.IO.File.OpenWrite(filename))
                {
                    switch(fileInfo.Extension.ToLower())
                    {
                        case "x":
                            {
                                break;
                            }
                        default:
                            {
                                SaveJpeg(bitmapSource, fs);
                                break;
                            }
                    }
                }

            }

            private static void SaveJpeg(BitmapSource bitmapSource, System.IO.Stream stream)
            {
                var en = new JpegBitmapEncoder();
                en.Frames.Add(BitmapFrame.Create(bitmapSource));
                en.Save(stream);
            }

            public static ImageSource FromStream(Stream stream)
            {
                return ImageExtension.GetImageSource(Image.FromStream(stream));
            }

            public static ImageSource FromFile(string url)
            {
                ImageSource result = null;
                try
                {
                    if (url.IndexOf("http:") == 0 || url.IndexOf("https:") == 0)
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            var data = webClient.DownloadData(url);

                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                result = ImageExtension.GetImageSource(Image.FromStream(mem));
                            }

                        }
                    }
                    else
                    {
                        result = ImageExtension.GetImageSource(Image.FromFile(url));
                    }
                }
                catch
                {
                    result = null;
                }
                return result;
            }

            public static ImageSource Crop(ImageSource source, int width, int height) => Crop(source as BitmapSource, width, height);
            public static ImageSource Crop(BitmapSource source,int width,int height)
            {
                var x = (int)(source.Width / 2 - width / 2);
                var y = (int)(source.Height / 2 - height / 2);
                if (x < 0) x = 0;
                if (y < 0) y = 0;
                return new CroppedBitmap(source, new System.Windows.Int32Rect(x, y, width, height));

            }
        }
    }
}