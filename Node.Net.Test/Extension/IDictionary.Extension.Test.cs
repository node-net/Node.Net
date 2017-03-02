﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    [TestFixture]
    class IDictionaryExtensionTest
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
    }
}