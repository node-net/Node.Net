using NUnit.Framework;
using Node.Net;
using Node.Net.Extensions;
using System;
using System.IO;
using System.Windows.Media;

namespace Node.Net
{
    [TestFixture, NUnit.Framework.Category("Extensions")]
    public class Extensions_Test
    {
        [TestCase]
        public void ImageSource_Usage()
        {
            string filename=$"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\\GoogleMapTest.jpeg";
            if (File.Exists(filename)) File.Delete(filename);
            ImageSource imgSrc = ImageSourceExtension.FromFile("http://maps.google.com/maps/api/staticmap?center=38.997934%2C-105.550567&zoom=12&size=640x480&maptype=hybrid&sensor=false");
            imgSrc.Save(filename);
            Assert.True(File.Exists(filename));
        }
    }
}