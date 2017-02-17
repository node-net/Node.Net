using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture, NUnit.Framework.Category(nameof(Extensions))]
    public class Extensions_Test
    {
        [TestCase]
        public void ImageSource_Usage()
        {
            /*
            var filename=$"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\\GoogleMap-640x480.jpeg";
            if (File.Exists(filename)) File.Delete(filename);
            var imgSrc = ImageSourceExtension.FromFile("http://maps.google.com/maps/api/staticmap?center=38.997934%2C-105.550567&zoom=12&size=640x480&maptype=hybrid&sensor=false");
            imgSrc.Save(filename);
            var bitmapImage = (BitmapImage)imgSrc;
            Assert.AreEqual(640, bitmapImage.PixelWidth);
            Assert.AreEqual(480, bitmapImage.PixelHeight);
            Assert.True(File.Exists(filename));

            filename = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments)}\\GoogleMapTest-400x200.jpeg";
            imgSrc = imgSrc.Crop(400, 200);
            var croppedBitmap = (CroppedBitmap)imgSrc;
            Assert.AreEqual(400, croppedBitmap.PixelWidth);
            Assert.AreEqual(200, croppedBitmap.PixelHeight);
            */
        }

        [Test]
        public void IDictionary_LocalToWorld()
        {
            var factory = new Factory(); // to ensure static intializeer for Factory gets called
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";

            var localToWorld = dictionary.GetLocalToWorld();
            var worldOrigin = localToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(10, worldOrigin.X);

            var childDictionary = new Dictionary<string, dynamic>();
            childDictionary["X"] = "1 m";

            dictionary["child"] = childDictionary;
            var child = dictionary["child"] as IDictionary;
            dictionary.DeepUpdateParents();
            //dictionary.DeepCollect<object>();
            Assert.AreSame(dictionary, child.GetParent(), "child.GetParent()");

            //localToWorld = Node.Net.Factories.Extension.IDictionaryExtension.GetLocalToWorld(child);
            localToWorld = child.GetLocalToWorld();
            worldOrigin = localToWorld.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(11, worldOrigin.X);


        }
    }
}