using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;

namespace Node.Net.Readers.Test
{
    [TestFixture]
    class ReaderTest
    {
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
        [TestCase("simple.txt", typeof(IList))]
        [TestCase("simple2.txt", typeof(IList))]
        public void Reader_Read(string name, Type type)
        {
            var stream = GlobalFixture.GetStream(name);
            Assert.NotNull(stream, nameof(stream));

            using (var reader = new Reader { UnrecognizedSignatureReader = new TextReader() })
            {
                var item = reader.Read(stream);

                Assert.NotNull(item, nameof(item));
                Assert.True(type.IsAssignableFrom(item.GetType()), $"{name} was not Assignable to {type.FullName}");

                if (type == typeof(IDictionary))
                {
                    var doc = item as Document;
                    Assert.NotNull(doc, nameof(doc));
                }
            }
        }

        [Test]
        public void Reader_Read_ResourceName()
        {
            using (var reader = new Reader())
            {
                var item = reader.Read("hash.json");
                Assert.NotNull(item);
            }
        }

        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Read_ShowImageSource()
        {
            var imageSource = Reader.Default.Read(typeof(ReaderTest), "map.jpg") as ImageSource;
            Assert.NotNull(imageSource);
            new Window
            {
                Title = "MapsHelper_GetImageSource",
                WindowState = WindowState.Maximized,
                Content = new Image { Source = imageSource }
            }.ShowDialog();
        }

        class Widget : Dictionary<string, dynamic> { }
        class Foo : Dictionary<string, dynamic> { }
        [Test]
        public void Reader_TypeConversion()
        {
            var stream = GlobalFixture.GetStream("simple.json");
            Assert.NotNull(stream, nameof(stream));

            var types = new Dictionary<string, Type>
            {
                { nameof(Widget), typeof(Widget) }, { nameof(Foo), typeof(Foo) }
            };
            using (var reader = new Reader { Types = types })
            {
                var data = reader.Read(stream) as IDictionary;
                Assert.NotNull(data, nameof(data));
                Assert.AreSame(typeof(Widget), data["widget"].GetType());
                var widget = data["widget"] as Widget;
                var foo = widget["foo"] as Foo;
                Assert.NotNull(foo, nameof(foo));
            }
        }



        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void Reader_Open_OpenFileDialogFilter()
        {
            var document = Reader.Default.Open("JSON Files (.json)|*.json|All Files (*.*)|*.*");
            Assert.NotNull(document, nameof(document));
        }
        [Test]
        public void Reader_Open_ManifestResourceName()
        {
            Assert.NotNull(Reader.Default.Open("simple.json"), "simple.json");
            Assert.Throws<Exception>(() => Reader.Default.Open("doesNotExist.json"), "doesNotExist.json");
            Assert.Throws<Exception>(() => Reader.Default.Open("corrupt.json"), "corrupt.json");

        }
        [Test]
        public void Reader_Open_Filename()
        {
            var filename = Path.GetTempFileName();
            File.WriteAllText(filename, "{}");
            var document = Reader.Default.Open(filename);
            Assert.NotNull(document, nameof(document));
            File.Delete(filename);
        }
    }
}
