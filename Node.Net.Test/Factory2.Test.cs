//using Node.Net.Factories.Deprecated;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    [TestFixture]
    public class Factory2Test
    {
        [TestCase(typeof(IDictionary), "simple.json")]
        [TestCase(typeof(ImageSource), "image.jpg")]
        [TestCase(typeof(ImageSource), "image.png")]
        [TestCase(typeof(ImageSource), "image.bmp")]
        [TestCase(typeof(ImageSource), "image.gif")]
        [TestCase(typeof(ImageSource), "image.tif")]
        [TestCase(typeof(MeshGeometry3D), "mesh.xaml")]
        [TestCase(typeof(Material), "Blue")]
        public void Factory2_Usage(Type targetType, string source)
        {
            using (var factory = new Factory2(typeof(Factory2Test).Assembly))
            {
                var instance = factory.Create(targetType, source);
                Assert.IsNotNull(instance);
                Assert.True(targetType.IsAssignableFrom(instance.GetType()));
            }
        }

        [Test]
        public void Factory2_Translations()
        {
            using (var factory = new Factory2(typeof(Factory2Test).Assembly))
            {
                var widget = factory.Create(typeof(IDictionary), "Translations.json") as IDictionary;
                Assert.IsNotNull(widget);
                var worldOrigin = widget.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(1, worldOrigin.X, "worldOrigin.X");
                Assert.AreEqual(2, worldOrigin.Y, "worldOrigin.Y");
                Assert.AreEqual(3, worldOrigin.Z, "worldOrigin.Z");

                var foo = widget["foo"] as IDictionary;
                Assert.IsNotNull(foo);
                Assert.AreSame(widget, foo.GetParent(), "foo.GetParent()");
                worldOrigin = foo.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(1.1, worldOrigin.X, "worldOrigin.X");
                Assert.AreEqual(2.2, worldOrigin.Y, "worldOrigin.Y");
                Assert.AreEqual(3.3, worldOrigin.Z, "worldOrigin.Z");

                var bar = foo["bar"] as IDictionary;
                Assert.IsNotNull(bar);
                worldOrigin = bar.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(1.11, Round(worldOrigin.X, 2), "worldOrigin.X");
                Assert.AreEqual(2.22, Round(worldOrigin.Y, 2), "worldOrigin.Y");
                Assert.AreEqual(3.33, Round(worldOrigin.Z, 2), "worldOrigin.Z");
            }
        }
        /*
        [Test]
        public void Factory2_LocalToParent()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["X"] = "10 m";

            var localToParent = global::Node.Net.Factories.Deprecated.Factory.Default.Create<Factories.Deprecated.ILocalToParent>(dictionary, null);
            var localToParent = global::Node.Net.Factories.Factory.Default.Create<Factories.Deprecated.ILocalToParent>(dictionary, null);
            Assert.NotNull(localToParent, nameof(localToParent));

            var parentOrigin = localToParent.LocalToParent.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(10, parentOrigin.X);
        }*/
        /*
        [Test]
        public void Factory2_LocalToWorld_Internal()
        {
            using (var factory = new Factory()) // to ensure static intializeer for Factory gets called
            {
                var dictionary = new Dictionary<string, dynamic>();
                dictionary["X"] = "10 m";

                var childDictionary = new Dictionary<string, dynamic>();
                childDictionary["X"] = "1 m";

                dictionary["child"] = childDictionary;
                var child = dictionary["child"] as IDictionary;
                dictionary.DeepUpdateParents();
                //dictionary.DeepCollect<object>();
                //dictionary.UpdateParentBindings();// TODO comment out
                Assert.AreSame(dictionary, child.GetParent(), "child.GetParent()");

                //var localToWorld = Node.Net.Factories.Factory.Default.Create<Node.Net.Factories.ILocalToWorld>(child);
                var localToWorld = global::Node.Net.Factories.Deprecated.Factory.Default.Create(typeof(global::Node.Net.Factories.Deprecated.ILocalToWorld), child, null) as global::Node.Net.Factories.Deprecated.ILocalToWorld;
                Assert.NotNull(localToWorld, nameof(localToWorld));

                var worldOrigin = localToWorld.LocalToWorld.Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(11, worldOrigin.X);
            }

        }*/
        /*
        [Test]
        public void Factory2_LocalToWorld()
        {
            using (var factory = new Factory())
            {
                var dictionary = new Dictionary<string, dynamic>();
                dictionary["X"] = "10 m";

                var childDictionary = new Dictionary<string, dynamic>();
                childDictionary["X"] = "1 m";

                dictionary["child"] = childDictionary;
                var child = dictionary["child"] as IDictionary;
                dictionary.DeepUpdateParents();
                //dictionary.DeepCollect<object>();
                Assert.AreSame(dictionary, child.GetParent(), "child.GetParent()");


                var localToWorld = factory.Create<ILocalToWorld>(child);
                Assert.NotNull(localToWorld, nameof(localToWorld));

                var worldOrigin = localToWorld.LocalToWorld.Transform(new Point3D(0, 0, 0));
                Assert.AreEqual(11, worldOrigin.X);
            }
        }*/

        public class Bar2 : Dictionary<string, dynamic> { }
        [Test]
        public void Factory2_Create_Custom_Dictionary()
        {
            using (var factory = new Factory())
            {
                var bar = factory.Create<Bar2>();
                Assert.NotNull(bar, nameof(bar));
                Assert.True(bar.ContainsKey("Type"));
            }
        }
    }
}
