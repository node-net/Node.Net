//using Node.Net.Factories.Deprecated;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net.Deprecated
{
    [TestFixture]
    public class FactoryTest
    {
        [TestCase(typeof(IElement), "simple.json")]
        [TestCase(typeof(ImageSource), "image.jpg")]
        [TestCase(typeof(ImageSource), "image.png")]
        [TestCase(typeof(ImageSource), "image.bmp")]
        [TestCase(typeof(ImageSource), "image.gif")]
        [TestCase(typeof(ImageSource), "image.tif")]
        [TestCase(typeof(MeshGeometry3D), "mesh.xaml")]
        //
        //[TestCase(typeof(Material), "Blue")]
        public void Factory_Usage(Type targetType, string source)
        {
            using (var factory = new Factory(typeof(FactoryTest).Assembly))
            {
                var instance = factory.Create(targetType, source);
                Assert.IsNotNull(instance);
                Assert.True(targetType.IsAssignableFrom(instance.GetType()));
            }
        }

        [Test]
        public void Factory_Translations()
        {
            using (var factory = new Factory(typeof(FactoryTest).Assembly))
            {
                var widget = factory.Create(typeof(IElement), "Translations.json") as IElement;// as IDictionary;
                Assert.IsNotNull(widget,nameof(widget));
                var worldOrigin = widget.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(1, worldOrigin.X, "worldOrigin.X");
                Assert.AreEqual(2, worldOrigin.Y, "worldOrigin.Y");
                Assert.AreEqual(3, worldOrigin.Z, "worldOrigin.Z");

                var foo = widget["foo"] as IElement;
                Assert.IsNotNull(foo,nameof(foo));
                Assert.AreSame(widget, foo.GetParent(), "foo.GetParent()");
                worldOrigin = foo.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(1.1, worldOrigin.X, "worldOrigin.X");
                Assert.AreEqual(2.2, worldOrigin.Y, "worldOrigin.Y");
                Assert.AreEqual(3.3, worldOrigin.Z, "worldOrigin.Z");

                var bar = foo["bar"] as IElement;
                Assert.IsNotNull(bar,nameof(bar));
                worldOrigin = bar.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(1.11, Round(worldOrigin.X, 2), "worldOrigin.X");
                Assert.AreEqual(2.22, Round(worldOrigin.Y, 2), "worldOrigin.Y");
                Assert.AreEqual(3.33, Round(worldOrigin.Z, 2), "worldOrigin.Z");
            }
        }

        public class Bar : Dictionary<string,dynamic>{}
        [Test]
        public void Factory_Create_Custom_Dictionary()
        {
            using (var factory = new Factory())
            {
                var bar = factory.Create<Bar>();
                Assert.NotNull(bar, nameof(bar));
                //Assert.True(bar.ContainsKey("Type"));
            }
        }

        interface IFoo { string Name { get; } }
        class Foo : IFoo
        {
            public Foo() { }
            public Foo(string name) { Name = name; }
            public string Name { get; set; } = "foo";
        }

        [Test]
        public void Factory_Create_Abstract()
        {
            var factory = new Factory();
            Assert.IsNull(factory.Create<IFoo>());

            factory.AbstractTypes = new Dictionary<Type, Type>
            {
                {typeof(IFoo),typeof(Foo) },
                {typeof(IDictionary),typeof(Dictionary<string,dynamic>) }
            };
            //factory.AbstractTypes.Add(typeof(IFoo), typeof(Foo));
            //factory.AbstractTypes.Add(typeof(IDictionary), typeof(Dictionary<string, dynamic>));

            Assert.IsNull(factory.Create<IEnumerable>());
            Assert.NotNull(factory.Create<IFoo>());
            var ifoo = factory.Create<IFoo>("test");
            Assert.NotNull(ifoo);
            Assert.AreEqual("test", ifoo.Name);

            Assert.IsNull(factory.Create<IFoo>(123));
            Assert.NotNull(factory.Create<IDictionary>());

            var assembly = typeof(FactoryTest).Assembly;
            if (!factory.ManifestResourceAssemblies.Contains(assembly)) factory.ManifestResourceAssemblies.Add(assembly);
            Assert.NotNull(factory.Create<IDictionary>("simple.json"));
        }

        [Test]
        public void Factory_Null()
        {
            Assert.IsNull(new Factory().Create(null, null));
        }

        [Test]
        public void Factory_CreateFromManifestResources()
        {
            var factory = new Factory();
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            factory.AbstractTypes.Add(typeof(IDocument), typeof(Document));

            var dictionary = factory.Create<IDictionary>("Scene.Cubes.json");
            Assert.NotNull(dictionary, nameof(dictionary));

            var idocument = factory.Create<IDocument>("Scene.Cubes.json");
            Assert.NotNull(idocument, nameof(idocument));
        }
    }
}
