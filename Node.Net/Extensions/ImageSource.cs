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
                FileInfo fileInfo = new FileInfo(filename);
                BitmapSource bitmapSource = imageSource as BitmapSource;
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
                JpegBitmapEncoder en = new JpegBitmapEncoder();
                en.Frames.Add(BitmapFrame.Create(bitmapSource));
                en.Save(stream);
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
                            byte[] data = webClient.DownloadData(url);

                            using (MemoryStream mem = new MemoryStream(data))
                            {
                                result = Image.FromStream(mem).GetImageSource();
                            }

                        }
                    }
                    else
                    {
                        result = Image.FromFile(url).GetImageSource();
                    }
                }
                catch
                {
                    result = null;
                }
                return result;
            }
        }
    }
}