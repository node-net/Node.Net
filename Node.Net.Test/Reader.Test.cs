using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;

namespace Node.Net
{
    [TestFixture]
    class ReaderTest
    {
        [TestCase("{}", typeof(IElement))]
        [TestCase("[]", typeof(IEnumerable))]
        [TestCase("simple.json", typeof(IElement))]
        //[TestCase("States.json", typeof(IElement))]
        [TestCase("image.jpg", typeof(ImageSource))]
        [TestCase("image.png", typeof(ImageSource))]
        [TestCase("image.bmp", typeof(ImageSource))]
        [TestCase("image.gif", typeof(ImageSource))]
        [TestCase("image.tif", typeof(ImageSource))]
        [TestCase("mesh.xaml", typeof(MeshGeometry3D))]
        public void Reader_Usage(string source, Type targetType)
        {
            using (var reader = new Reader())
            {
                using (MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(source.Replace("'", "\""))))
                {
                    var stream = typeof(ReaderTest).Assembly.GetStream(source);
                    if (stream == null) stream = memory;
                    var value = reader.Read(stream);
                    Assert.NotNull(value, $"value for {source}");
                    Assert.True(targetType.IsAssignableFrom(value.GetType()), $"type {value.GetType()} for '{source}'");

                    // Should be able to write what was read
                    var tmp_filename = Path.GetTempFileName();
                    Writer.Default.Write(tmp_filename, value);
                    File.Delete(tmp_filename);
                }
            }
        }

        [Test,Explicit]
        public void Reader_Open_Document()
        {
            var stream = typeof(ReaderTest).Assembly.GetStream("States.json");
            var doc = Reader.Default.Read(stream);
            Assert.NotNull(doc, nameof(doc));
            var document = doc as IElement;
            Assert.NotNull(document, nameof(document));

            //var itemsSource = document.ItemsSource;
            foreach(var item in document.Keys)
            {
                var d = document[item] as IElement;
                //var d = item as Collections.Dictionary;
                Assert.NotNull(d, nameof(d));

                var parent = global::Node.Net.Collections.ObjectExtension.GetParent(d);
                Assert.NotNull(parent, nameof(parent));

                Assert.NotNull(d.Parent, "d.Parent");
                //var key = d.Key;
                //Assert.True(key.Length > 0);
            }
        }

        [Test]
        public void Reader_Open()
        {
            Assert.NotNull(Reader.Default.Open("simple.json"), "simple.json");
            Assert.Throws<Exception>(()=>Reader.Default.Open("doesNotExist.json"), "doesNotExist.json");
            Assert.Throws<Exception>(() => Reader.Default.Open("corrupt.json"), "corrupt.json");
        }

        [Test]
        public void Reader_Parent_References()
        {
            var value = new Reader().Read(typeof(ReaderTest).Assembly.GetStream("Scene.Cubes.json")) as IDictionary;
            var instances = value.Collect();
            foreach(var key in instances.Keys)
            {
                var child = instances[key] as IDictionary;
                if(child != null)
                {
                    Assert.NotNull(child.GetParent());
                    Assert.AreSame(value, child.GetRootAncestor());
                }
               
            }
        }

        [Test, Explicit, Apartment(ApartmentState.STA)]
        public void Reader_Open_OpenFileDialogFilter()
        {
            var document = Reader.Default.Open("JSON Files (.json)|*.json|All Files (*.*)|*.*");
            Assert.NotNull(document, nameof(document));
        }
        [Test]
        public void Reader_Open_ManifestResourceName()
        {
            var document = Reader.Default.Open("simple.json");
            Assert.NotNull(document, nameof(document));
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
