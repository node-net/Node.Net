using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;

namespace Node.Net.Writers.Test
{
    [TestFixture]
    public class WriterTest
    {
        [Test]
        [TestCase("EmptyHash","{}")]
        public void Writer_Text(string name,string text_pattern)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                Writer.Default.Write(memory, GetInstance(name));
            }
        }

        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void Writer_Save()
        {
            var data = new Dictionary<string, dynamic>();
            Writer.Default.Save(data);
        }

        [Test]
        [TestCase("simple.json", typeof(IDictionary))]
        [TestCase("hash.json", typeof(IDictionary))]
        [TestCase("array.json", typeof(IEnumerable))]
        [TestCase("mesh.xaml", typeof(MeshGeometry3D))]
        [TestCase("simple.html", typeof(XmlDocument))]
        [TestCase("image.bmp", typeof(ImageSource))]
        [TestCase("image.gif", typeof(ImageSource))]
        [TestCase("image.jpg", typeof(ImageSource))]
        [TestCase("image.png", typeof(ImageSource))]
        [TestCase("image.tif", typeof(ImageSource))]
        public void Reader_Read(string name, Type type)
        {
            var stream = GlobalFixture.GetStream(name);
            Assert.NotNull(stream, nameof(stream));

            var reader = new Node.Net.Readers.Reader();
            var item = reader.Read(stream);

            Assert.NotNull(item, nameof(item));
            Assert.True(type.IsAssignableFrom(item.GetType()), $"{name} was not Assignable to {type.FullName}");

            var tmp_filename = Path.GetTempFileName();
            var writer = new Writer();
            using (FileStream fs = new FileStream(tmp_filename, FileMode.Create))
            {
                writer.Write(fs, item);
            }

            var item2 = reader.Read(tmp_filename);
            Assert.NotNull(item2, nameof(item2));
            Assert.AreSame(item.GetType(), item2.GetType());
            File.Delete(tmp_filename);
        }
        private static object GetInstance(string name)
        {
            switch(name)
            {
                case "EmptyHash":
                    return new Dictionary<string, dynamic>();
                default:
                    return null;
            }
        }

       
    }
}
