using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    [TestFixture,Category("IDictionary")]
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
        public void IDictionary_Extension_GetLengthMeters()
        {
            var data = new Dictionary<string, dynamic>
            {
                {"Length","600 ft" },
                {"Width","10 m" }
            };

            Assert.AreEqual(10, data.GetLengthMeters("Width"), "Width");
            Assert.AreEqual(183,Round (data.GetLengthMeters("Length"),0), "Length");
        }
        [Test]
        public void IDictionary_Extension_GetLocalToWorld()
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
            var matrix = foo.GetLocalToWorld();
            Assert.NotNull(matrix, nameof(matrix));
            Assert.False(matrix.IsIdentity);
            var worldOrigin = matrix.Transform(new Point3D(0, 0, 0));
            Assert.AreEqual(11, worldOrigin.X);
        }

        [Test, Explicit]
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

            for (int x = 0; x < 1000000; x++)
            {
                dictionary["X"] = $"{x} m";
                var matrix = foo.GetLocalToWorld();
            }
        }

        [Test]
        [TestCase("Scene.Cubes.json", typeof(IDictionary), null, 10)]
        [TestCase("States.json", typeof(IDictionary), null, 3205)]
        [TestCase("States.json", typeof(IDictionary), "Colorado", 66)]
        public void IDictionary_Collect(string name, Type type, string search, int expectedCount)
        {
            var data = Factory.Default.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
            Assert.AreEqual(expectedCount, data.Collect(type, search).Count);
        }

        [Test]
        [TestCase("Scene.Cubes.json", null, 10)]
        [TestCase("States.json", null, 3205)]
        [TestCase("States.json", "Colorado", 66)]
        public void IDictionary_Generic_Collect(string name,string search, int expectedCount)
        {
            var data = Factory.Default.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
            Assert.AreEqual(expectedCount, data.Collect<IDictionary>(search).Count);
        }

        [Test]
        public void IDictonary_Collect_Custom()
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
        [TestCase("States.json",2)]
        public void IDictionary_CollectValues(string name,int expectedCount)
        {
            var data = Factory.Default.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
            Assert.AreEqual(expectedCount, data.CollectValues<string>("Type").Count);
        }
        [Test]
        public void IDictonary_CollectValues2()
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
        [TestCase("States.json")]
        public void IDictionary_Clone(string name)
        {
            var data = Factory.Default.Create<IDictionary>(name);
            Assert.NotNull(data, nameof(data));
            var clone = data.Clone();
            Assert.AreEqual(data.Count, clone.Count, "Counts do not match");
            Assert.AreEqual(data.ComputeHashCode(), clone.ComputeHashCode(), "HashCodes do not match");
        }

        [Test]
        public void IDictionary_GetByName_Foo()
        {
            var data = new Dictionary<string, dynamic>();
            var foo = new Dictionary<string, dynamic>();
            data["foo"] = foo;
            var item = data.Get<IDictionary>("foo", null, true);
            Assert.AreSame(item, foo);

            var null_item = data.Get<IDictionary>(null);
        }
        [Test]
        public void IDictionary_GetByName()
        {
            var stream = typeof(IDictionaryExtensionTest).Assembly.GetManifestResourceStream
                ("Node.Net.Tests.Resources.Object.Bars.json");
            Assert.NotNull(stream, nameof(stream));
            var factory = new Factory();
            factory.IDictionaryTypes.Add("Bar", typeof(Bar));
            var data = factory.Create<IDictionary>(stream);

            //var bars = data.Collect<IBar>();
            //Assert.AreEqual(1, bars.Count);

            var bar = data.Get<IBar>("bar-0", null, true);
            Assert.NotNull(bar, nameof(bar));
        }

        [Test]
        public void IDictionary_ConvertTypes()
        {
            var data = new Dictionary<string, dynamic>
            {
                {"Type","Document" },
                {"foo0",new Dictionary<string, dynamic>
                        {
                            { "Type" ,"Foo" },
                            {"bar0",new Dictionary<string, dynamic>
                                    {
                                        {"Type","Bar" }
                                    }
                            }
                        }
                }
            };

            var converted = data.ConvertTypes(new Dictionary<string, Type>(), typeof(Element));
            var elements = converted.Collect<Element>();
            Assert.AreEqual(2, elements.Count);

            var states = Factory.Default.Create<IDictionary>("States.json");
            var dictionaries = states.Collect<IDictionary>();
            Assert.NotNull(states, nameof(states));
            converted = states.ConvertTypes(new Dictionary<string, Type>(), typeof(Element));
            elements = converted.Collect<Element>();
            Assert.AreEqual(dictionaries.Count, elements.Count);
        }
        [Test]
        public void IDictionary_GetHashCode()
        {
            Assert.AreNotEqual(0, "X".GetHashCode(),"X");
            Assert.AreNotEqual(0, "10 m".GetHashCode(),"10 m");
            var data = new Dictionary<string, dynamic> { { "X", "10 m" } };
            Assert.AreNotEqual(0, data.ComputeHashCode());
        }
        [Test]
        public void IDictionary_CompareTo()
        {
            var dictionary1 = new Dictionary<string,dynamic>
            {
                {"X" , "10 m" }
            };
            var dictionary2 = new Dictionary<string,dynamic>
            {
                {"X" ,"11 m" }
            };
            Assert.AreEqual(0, dictionary1.CompareTo(dictionary1),"dictionary1.CompareTo(dictionary1)");
            Assert.AreEqual(1, dictionary1.CompareTo(null),"dictionary1.CompareTo(null)");
            Assert.AreEqual(-1, dictionary1.CompareTo(dictionary2), "dictionary1.CompareTo(dictionary2)");
            Assert.AreEqual(1, dictionary2.CompareTo(dictionary1),"dictionary2.CompareTo(dictionary1)");
        }
        [Test]
        public void IDictionary_SetWorldOrigin()
        {
            var d0 = new Dictionary<string, dynamic>();
            d0.SetWorldOrigin(new Point3D(100, 10, 1));
            Assert.AreEqual("100 m", d0.Get<string>("X"),"X");
            Assert.AreEqual("10 m", d0.Get<string>("Y"), "Y");
            Assert.AreEqual("1 m", d0.Get<string>("Z"), "Z");

            var d1 = new Dictionary<string, dynamic>();
            d0.Add("d1", d1);
            d0.DeepUpdateParents();

            d1.SetWorldOrigin(new Point3D(110, 11, 1.1));
            Assert.AreEqual("10 m", d1.Get<string>("X"), "X");
            Assert.AreEqual("1 m", d1.Get<string>("Y"), "Y");
            Assert.AreEqual("0.1 m", d1.Get<string>("Z"), "Z");
        }
    }
}
