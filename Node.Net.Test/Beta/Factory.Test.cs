using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta
{
    [TestFixture, Category(nameof(Beta))]
    class FactoryTest
    {
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
            Interfaces.IFactoryTest.IFactory_Test_DefaultConstructor(factory, types);
            Interfaces.IFactoryTest.IFactory_Test_IDictionaryConstructor(factory, types);
            Interfaces.IFactoryTest.IFactory_Test_CreateFromManifestResourceStream(factory, types);

            var iwidget = factory.Create<IWidget>("Widget.0.json");
            Assert.NotNull(iwidget, nameof(iwidget));
            Interfaces.IFactoryTest.IFactory_Test_CreateMedia3DFromIDictionary(factory, iwidget);
            Assert.NotNull(factory.Create<GeometryModel3D>(iwidget), "GeometryModel3D from iwidget");
            Assert.NotNull(factory.Create<MeshGeometry3D>(iwidget), "MeshGeometry3D from iwidget");

            var ifoo = factory.Create<IFoo>("Foo.0.json");
            Assert.NotNull(ifoo, nameof(ifoo));
            Interfaces.IFactoryTest.IFactory_Test_CreateMedia3DFromIDictionary(factory, ifoo);

            var ibar = factory.Create<IBar>("Bar.0.json");
            Interfaces.IFactoryTest.IFactory_Test_CreateMedia3DFromIDictionary(factory, ibar);

            
        }

        public Model3D GetModel3D(IDictionary data)
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
    }
}
