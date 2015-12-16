using NUnit.Framework;
using Node.Net;
using Node.Net.Extensions;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Node.Net
{
    [TestFixture, NUnit.Framework.Category("Extensions")]
    public class Extensions_Test
    {
        [TestCase]
        public void ImageSource_Usage()
        {
            //string filename=$"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\\GoogleMapTest-640x480.jpeg";
            string filename=$"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\\GoogleMap-640x480.jpeg";
            if (File.Exists(filename)) File.Delete(filename);
            ImageSource imgSrc = ImageSourceExtension.FromFile("http://maps.google.com/maps/api/staticmap?center=38.997934%2C-105.550567&zoom=12&size=640x480&maptype=hybrid&sensor=false");
            imgSrc.Save(filename);
            BitmapImage bitmapImage = (BitmapImage)imgSrc;
            Assert.AreEqual(640, bitmapImage.PixelWidth);
            Assert.AreEqual(480, bitmapImage.PixelHeight);
            Assert.True(File.Exists(filename));

            filename = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\\GoogleMapTest-400x200.jpeg";
            imgSrc = imgSrc.Crop(400, 200);
            CroppedBitmap croppedBitmap = (CroppedBitmap)imgSrc;
            Assert.AreEqual(400, croppedBitmap.PixelWidth);
            Assert.AreEqual(200, croppedBitmap.PixelHeight);
        }
    }
}