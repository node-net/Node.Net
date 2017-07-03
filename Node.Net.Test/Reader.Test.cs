using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;

namespace Node.Net
{
    [TestFixture,Category("Reader")]
    public class ReaderTest
    {
        [Test]
        public void Read_DefaultObjectType()
        {
            var reader = new Reader { DefaultObjectType = typeof(SpatialElement) };
            var data = reader.Read("States.json") as IDictionary;
            Assert.NotNull(data, nameof(data));

            var dictionaries = data.Collect<IDictionary>();
            var spatialElements = data.Collect<SpatialElement>();
            Assert.AreEqual(dictionaries.Count, spatialElements.Count);
        }
        [Test,Apartment(ApartmentState.STA)]
        [TestCase("{}",typeof(IDictionary))]
        [TestCase("JSON Files(*.json)|*.json",typeof(IDictionary)),Explicit]
        public void Read(string name,Type expectedType)
        {
            var reader = new Reader();
            var instance = reader.Read(name);
            Assert.NotNull(instance, nameof(instance));

            var filename = instance.GetFileName();
        }

        [Test, Apartment(ApartmentState.STA), Explicit]
        //[TestCase("States.json", typeof(IDictionary), 200)]
        public void Read_StressTest()// string name,Type expectedType,int iterations)
        {
            string name = "States.json"; //"Scene.50000.json";// "States.json";
            Type expectedType = typeof(IDictionary);
            int iterations = 500;

            var reader = new Reader();
            for(int i = 0; i < iterations; ++i)
            {
                var instance = reader.Read(name);
                Assert.NotNull(instance, nameof(instance));
                Assert.True(expectedType.IsAssignableFrom(instance.GetType()), $"type {expectedType.FullName} not assignable from instance");
            }
        }

        [Test]
        public void Reader_Read()
        {
            var data = new Dictionary<string, Type>
            {
                {"MeshGeometry3D.Widget.xaml" , typeof(MeshGeometry3D) },
                {"index.html", typeof(XmlDocument) },
                {"image.bmp", typeof(ImageSource) },
                {"image.gif",typeof(ImageSource) },
                {"image.jpg",typeof(ImageSource) },
                {"image.png",typeof(ImageSource) },
                {"image.tif",typeof(ImageSource) },
                {"Dictionary.0.json", typeof(IDictionary) }
            };
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(ReaderTest).Assembly);
            var reader = new Reader();
            foreach (var name in data.Keys)
            {
                var stream = factory.Create<Stream>(name);
                Assert.NotNull(stream, $"null stream for '{name}'");

                var instance = reader.Read(stream);
                Assert.NotNull(instance, $"null instance for '{name}'");

                Assert.True(data[name].IsAssignableFrom(instance.GetType()), $"type {data[name].FullName} not assignable from instance");

                stream.Close();
                stream = factory.Create<Stream>(name);
                Assert.NotNull(stream, $"null stream for '{name}'");

                var instance2 = reader.Read(stream);
                Assert.NotNull(instance2, $"null instance for '{name}'");
            }
        }

        [Test]
        public void Reader_Read_Nested_Arrays()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(ReaderTest).Assembly);
            var reader = new Reader();
            var stream = factory.Create<Stream>("Array.Nested.json");
            var instance = reader.Read(stream);
            Assert.NotNull(instance, nameof(instance));
            var ienumerable = instance as IEnumerable;
            Assert.NotNull(ienumerable, nameof(ienumerable));
            var array_0 = ienumerable.GetAt(0) as IEnumerable;
            Assert.NotNull(array_0, nameof(array_0));
            Assert.AreEqual(1, array_0.GetAt(0));
            var array_1 = ienumerable.GetAt(1) as IEnumerable;
            Assert.NotNull(array_1, nameof(array_1));
            Assert.AreEqual(2, array_1.GetAt(0));
            Assert.AreEqual(3, array_1.GetAt(1));
        }

        [Test]
        public void Reader_Read_Write_Base64_a()
        {
            string[] data = { "a", "ab", "abc", "abcd", "abcde", "" };
            foreach (var s in data)
            {
                var bytes = Encoding.UTF8.GetBytes(s);
                var base64 = Convert.ToBase64String(bytes);

                var equalsCount = base64.Count(x => x == '=');

                var bytes2 = Convert.FromBase64String(base64);
                if (bytes.Length != bytes2.Length)
                {

                }
            }
        }
        [Test]
        public void Reader_Read_Write_Base64()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(ReaderTest).Assembly);

            var mesh = factory.Create<MeshGeometry3D>("mesh.xaml");
            Assert.NotNull(mesh, nameof(mesh));

            var base64 = Writer.Default.WriteToBase64String(mesh);
            var mesh2 = Reader.Default.ReadFromBase64String(base64);
            Assert.NotNull(mesh2, nameof(mesh2));
            Assert.AreNotSame(mesh, mesh2);
        }

        [Test]
        public void Reader_Read_Special_Strings()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(ReaderTest).Assembly);

            var data = factory.Create<IDictionary>("Object.Sample.Special.Strings.json");
            Assert.AreEqual("6\"", data["X"].ToString());
        }

        [Test, Explicit]
        public void Reader_Read_Atypical()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(ReaderTest).Assembly);

            var data = factory.Create<IDictionary>("Object.Sample.json");
        }

        [Test]
        public void ReadOnlyFile()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(ReaderTest).Assembly);

            var mesh = factory.Create<MeshGeometry3D>("mesh.xaml");
            Assert.NotNull(mesh, nameof(mesh));

            var tempfile = Path.GetTempFileName();
            using (var fs = new FileStream(tempfile, FileMode.Create))
            {
                XamlWriter.Save(mesh, fs);
            }

            var fileInfo = new FileInfo(tempfile);
            fileInfo.IsReadOnly = true;

            factory.Create<MeshGeometry3D>(tempfile);

            fileInfo.IsReadOnly = false;
            File.Delete(tempfile);
        }
    }
}
