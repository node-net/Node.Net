using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net.Deprecated.Data.Readers
{
    sealed class ImageSourceReader : IRead
    {
        public object Read(Stream stream) => GetImageSource(Image.FromStream(stream));

        public static ImageSourceReader Default { get; } = new ImageSourceReader();
        public static ImageSource GetImageSource(object value)
        {
            var image = value as Image;
            if (!object.ReferenceEquals(null, image))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                var memory = new System.IO.MemoryStream();
                image.Save(memory, ImageFormat.Bmp);
                memory.Seek(0, SeekOrigin.Begin);

                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                return bitmapImage;
            }
            return null;
        }

        public static Dictionary<byte[],string> BinarySignatures
        {
            get
            {
                var signatures = new Dictionary<byte[], string>();
                signatures.Add(BytesReader.HexStringToByteArray("42 4D"), ".bmp");
                signatures.Add(BytesReader.HexStringToByteArray("FF D8 FF E0"), ".jpg");
                signatures.Add(BytesReader.HexStringToByteArray("FF D8 FF E1"), ".jpg");


                signatures.Add(BytesReader.HexStringToByteArray("47 49 46 38 37 61"), ".gif");
                signatures.Add(BytesReader.HexStringToByteArray("47 49 46 38 39 61"), ".gif");

                signatures.Add(BytesReader.HexStringToByteArray("49 49 2A 00"), ".tif");
                signatures.Add(BytesReader.HexStringToByteArray("4D 4D 00 2A"), ".tif");

                signatures.Add(BytesReader.HexStringToByteArray("89 50 4E 47 0D 0A 1A 0A"), ".png");
                return signatures;
            }
        }
    }
}
