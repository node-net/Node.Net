using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture]
    public class IDictionaryExtensionTest
    {
        [Test]
        public void IDictionary_Extension_GetLocalToParent()
        {
            var dictionary = new Dictionary<string, dynamic>
            {
                {"X","10 m" }
            };
            var matrix = dictionary.GetLocalToParent();
            Assert.NotNull(matrix, nameof(matrix));
            Assert.False(matrix.IsIdentity);
            var parentOrigin = matrix.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(10, parentOrigin.X);
        }

        [Test]
        public void IDictionary_Extension_GetLocalToWorld()
        {
            var foo = new Dictionary<string, dynamic>
            {
                {"X","1 m" }
            };
            var dictionary = new Dictionary<string,dynamic>
            {
                {"X","10 m" },
                {"foo",foo}
            };
            dictionary.DeepUpdateParents();
            var matrix = foo.GetLocalToWorld();
            Assert.NotNull(matrix, nameof(matrix));
            Assert.False(matrix.IsIdentity);
            var worldOrigin = matrix.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(11, worldOrigin.X);
        }

        [Test,Explicit]
        public void IDictionary_Extension_Profile_GetLocalToWorld()
        {
            var foo = new Dictionary<string, dynamic>
            {
                {"X","1 m" }
            };
            var dictionary = new Dictionary<string, dynamic>
            {
                {"X","10 m" },
                {"foo",foo}
            };
            dictionary.DeepUpdateParents();
            
            for(int x = 0; x < 1000000; x++)
            {
                dictionary["X"] = $"{x} m";
                var matrix = foo.GetLocalToWorld();
            }
        }

        [Test]
        public void IDictionary_Collect_Deep()
        {
            var data = Factory.Default.Create<IDictionary>("Scene.Cubes.json");
            Assert.AreEqual(10,data.Collect<IDictionary>().Count);

            data = Factory.Default.Create<IDictionary>("States.json");
            Assert.AreEqual(3205, data.Collect<IDictionary>().Count);
        }
        [Test]
        public void IDictonary_Collect()
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
            var bars = iwidget.Collect("Bar");
            Assert.AreEqual(1, bars.Count);
            var ibar = bars[0] as IBar;
            Assert.AreSame(ifoo, ibar.Parent);
            Assert.AreEqual("bar0", ibar.Name);

       
        }
        [Test]
        public void IDictonary_CollectValues()
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

            var type_names = iwidget.CollectValues<string>("Type");
            Assert.True(type_names.Contains("Widget"));
            Assert.True(type_names.Contains("Foo"));
            Assert.True(type_names.Contains("Bar"));


        }

        [Test]
        public void IDictionary_Clone()
        {
            var source = new Dictionary<string, dynamic>
            {
                { "A" , "abc"},{"B","def"}

            };
            var clone = source.Clone();
            Assert.True(typeof(Dictionary<string, dynamic>) == clone.GetType());
            Assert.AreEqual("abc", clone["A"].ToString());
            Assert.AreEqual("def", clone["B"].ToString());
        }
    }
}
