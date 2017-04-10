using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Threading;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Controls;
using System.Windows;

namespace Node.Net
{
    [TestFixture]
    public class FactoryTest
    {
       [Test]
       public void Factory_Create_Abstract()
        {
            var factory = new Factory();
            Assert.NotNull(factory.Create<IDictionary>(), "factory.Create<IDictionary>()");
            Assert.NotNull(factory.Create<IList>(), "factory.Crate<IList>()");
        }
        [Test]
        public void Factory_Test()
        {
            var factory = new Factory
            {
                AbstractTypes = new Dictionary<Type, Type>
                {
                    {typeof(IWidget),typeof(Widget) },
                    {typeof(IFoo), typeof(Foo) },
                    {typeof(IBar),typeof(Bar) },
                    {typeof(IDictionary),typeof(Dictionary<string,dynamic>) }
                },
                PrimaryModel3DHelperFunction = GetModel3D
            };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            Type[] types = { typeof(IDictionary), typeof(IWidget), typeof(IFoo), typeof(IBar) };
            IFactoryTest.IFactory_Test_DefaultConstructor(factory, types);
            IFactoryTest.IFactory_Test_IDictionaryConstructor(factory, types);
            IFactoryTest.IFactory_Test_CreateFromManifestResourceStream(factory, types);

            var iwidget = factory.Create<IWidget>("Widget.0.json");
            Assert.NotNull(iwidget, nameof(iwidget));
            IFactoryTest.IFactory_Test_CreateMedia3DFromIDictionary(factory, iwidget);
            Assert.NotNull(factory.Create<GeometryModel3D>(iwidget), "GeometryModel3D from iwidget");
            Assert.NotNull(factory.Create<MeshGeometry3D>(iwidget), "MeshGeometry3D from iwidget");

            var ifoo = factory.Create<IFoo>("Foo.0.json");
            Assert.NotNull(ifoo, nameof(ifoo));
            IFactoryTest.IFactory_Test_CreateMedia3DFromIDictionary(factory, ifoo);

            var ibar = factory.Create<IBar>("Bar.0.json");
            IFactoryTest.IFactory_Test_CreateMedia3DFromIDictionary(factory, ibar);

            var image = factory.Create<ImageSource>("image.jpg");
            Assert.NotNull(image, nameof(image));

            var bar = factory.Create(typeof(IBar), "Bar.0.json");
            bar = factory.Create<IBar>(bar);
            bar = factory.Create<IBar>();

            bar = factory.Create<IBar>("Bar.not.there.json");
            Assert.IsNull(bar, nameof(bar));
        }

        public static Model3D GetModel3D(IDictionary data)
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            if (data != null && data.Contains("Type") && data["Type"].ToString() == nameof(Bar))
            {
                return factory.Create<Model3D>("Foo.Model3D.xaml");
            }
            return null;
        }

        [Test]
        public void Factory_Collect()
        {
            var factory = new Factory
            {
                AbstractTypes = new Dictionary<Type, Type>
                {
                    {typeof(IWidget),typeof(Widget) },
                    {typeof(IFoo), typeof(Foo) },
                    {typeof(IBar),typeof(Bar) }
                },
                IDictionaryTypes = new Dictionary<string, Type>
                {
                    {nameof(Widget) , typeof(Widget) },
                    {nameof(Foo), typeof(Foo) },
                    {nameof(Bar),typeof(Bar) }
                }
            };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);


            var iwidget = factory.Create<IWidget>("Widget.1.json");
            Assert.NotNull(iwidget, "iwidget Widget.1.json");
            var foos = iwidget.Collect(typeof(IFoo));
            Assert.AreEqual(1, foos.Count);
            var ifoo = foos[0] as IFoo;
            Assert.AreSame(iwidget, ifoo.Parent);
            var bars = iwidget.Collect(typeof(IBar));
            Assert.AreEqual(1, bars.Count);
            var ibar = bars[0] as IBar;
            Assert.AreSame(ifoo, ibar.Parent);
            Assert.AreEqual("bar0", ibar.Name);
        }

        [Test]
        public void Factory_StreamSignatures()
        {
            var data = new Dictionary<string, string>
            {
                { "image.bmp", "42 4D" },
                { "image.gif", "47 49 46 38 39 61" },
                { "Widget.MeshGeometry3D.xaml", "<" },
                { "Widget.0.json", "{" },
                { "Text.Sample.A.txt","//" }
            };
            var factory = new Factory();
            foreach (var resource_name in data.Keys)
            {
                var signature = factory.Create<IStreamSignature>(resource_name);
                Assert.NotNull(signature);
                Assert.True(signature.ToString().Contains(data[resource_name]), $"{resource_name} {data[resource_name]} {signature.ToString()}");
            }
        }

        [Test]
        public void Factory_Units_Conversion()
        {
            var factory = new Factory();
            Assert.AreEqual(2, factory.Create<double>("2"));
            Assert.AreEqual(1, factory.Create<double>("1 m"));
            Assert.AreEqual(1, factory.Create<double>("100 cm"));
            Assert.AreEqual(45, factory.Create<double>("45 degrees"));
        }

        [Test]
        public void Factory_Brushes()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var solidColor = factory.Create<Brush>("Blue");
            Assert.NotNull(solidColor, nameof(solidColor));

            var image = factory.Create<ImageSource>("image.bmp");
            Assert.NotNull(image, nameof(image));

            var imageBrush = factory.Create<Brush>("image.bmp");
            Assert.NotNull(imageBrush, nameof(imageBrush));
        }
        [Test]
        public void Factory_Materials()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var diffuseBlue = factory.Create<Material>("Blue");
            Assert.NotNull(diffuseBlue, nameof(diffuseBlue));

            var imageMaterial = factory.Create<Material>("image.bmp");
            Assert.NotNull(imageMaterial, nameof(imageMaterial));

        }

        [Test]
        public void Factory_Visual3D()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            var m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d));

            var v3d = factory.Create<Visual3D>(scene);
            Assert.NotNull(v3d, nameof(v3d));

            string filename = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Scene.Cubes.xaml";
            using (var s = new FileStream(filename, FileMode.Create))
            {
                Node.Net.Writer.Default.Write(s, v3d);
            }
        }

        [Test,Apartment(ApartmentState.STA),Explicit]
        public void Factory_Viewport3D()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            var m3d = factory.Create<Model3D>(scene);
            Assert.NotNull(m3d, nameof(m3d));

            var viewport = factory.Create<Viewport3D>(scene);
            Assert.NotNull(viewport, nameof(viewport));

            new Window
            {
                Title = "Factory Viewport3D",
                WindowState = WindowState.Maximized,
                Content = viewport
            }.ShowDialog();
        }

        [Test,Explicit]
        public void Factory_Visual3D_Profile()
        {
            var factory = new Factory { Cache = true };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var scene = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(scene, nameof(scene));

            for (int i = 0; i < 500; ++i)
            {
                var v3d = factory.Create<Visual3D>(scene);
                Assert.NotNull(v3d, nameof(v3d));
            }
        }

        [Test]
        public void Factory_Transform3D()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            var widget = factory.Create<IDictionary>("Widget.1.json");
            Assert.NotNull(widget, nameof(widget));

            var matrix3D = factory.Create<Matrix3D>(widget);
            Assert.NotNull(matrix3D,nameof(matrix3D));
            Assert.False(matrix3D.IsIdentity);

            var transform3D = factory.Create<Transform3D>(widget);
            Assert.NotNull(transform3D,nameof(transform3D));

            var origin = transform3D.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(2, origin.X);
        }

        [Test]
        public void Factory_Resources()
        {
            var factory = new Factory();
            var camera = factory.Create<Camera>("Top");
            Assert.NotNull(camera, nameof(camera));
        }

        [Test,Explicit,Apartment(ApartmentState.STA)]
        public void Factory_Create_From_OpenFileDialogFilter()
        {
            var factory = new Factory();
            var item = factory.Create<object>("Text Files(.txt)|*.txt|All Files(*.*)|*.*");
            item = null;
        }

        [Test,Apartment(ApartmentState.STA)]
        public void Factory_Clone()
        {
            var ellipse = new System.Windows.Shapes.Ellipse { Width = 10, Height = 15 };
            var clone = Factory.Default.Create<Ellipse>(ellipse);
            Assert.NotNull(clone, nameof(clone));
        }

        [Test]
        public void Factory_Create_Stream()
        {
            Assert.NotNull(Factory.Default.Create<Stream>("http://www.makoto3.net/xaml/CubeSample002.xaml"));
            GC.Collect();
        }
        [Test,Apartment(ApartmentState.STA)]
        public void Factory_Create_From_Uri()
        {
            var page = Factory.Default.Create<Page>("http://www.makoto3.net/xaml/CubeSample002.xaml");
            Assert.NotNull(page);
        }

        [Test, Apartment(ApartmentState.STA)]
        public void Factory_Create_From_Filename()
        {
            var data = new Dictionary<string, dynamic>
            {
                {"Type","Widget" }
            };
            var tmp_filename = System.IO.Path.GetTempFileName();
            Node.Net.Writer.Default.Write(tmp_filename, data);

            var dictionary = Factory.Default.Create<IDictionary>(tmp_filename);
            Assert.NotNull(dictionary, nameof(dictionary));
            File.Delete(tmp_filename);
        }
    }
}
