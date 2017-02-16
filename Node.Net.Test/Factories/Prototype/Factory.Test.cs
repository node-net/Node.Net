using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Prototype
{
    [TestFixture]
    class FactoryTest
    {
        static Dictionary<Type, Type> AbstractTypes = new Dictionary<Type, Type>
        {
            {typeof(IWidget),typeof(Widget) },
            {typeof(IFoo),typeof(Foo) }
        };
        static ResourceDictionary Resources = new ResourceDictionary
        {
            { "Color", Colors.Blue }
        };
        static Type[] DefaultTypes =
        {
            typeof(Widget),
            typeof(IWidget),
            typeof(Foo),
            typeof(IFoo)
        };
        static Type[] FromIDictionaryTypes =
        {
            typeof(Widget),
            typeof(IWidget)
        };
        static Prototype.Factory TestFactory = new Prototype.Factory
        {
            AbstractTypes = AbstractTypes,
            Resources = Resources
        };

        [Test]
        public void Prototype_Factory_Properties()
        {
            Assert.True(TestFactory.AbstractTypes.Count > 0);
            Assert.True(TestFactory.FactoryFunctions.Count > 0);
            Assert.AreEqual(1,TestFactory.Resources.Count);
        }
        [Test]
        public void Prototype_Factory_States()
        {
            var factory = new Factory();
            var states = factory.Create<IDictionary>("States.json");
            Assert.NotNull(states, nameof(states));
            Assert.AreEqual(50, states.Count);
        }

        [Test]
        public void Prototype_Factory_Create_Null()
        {
            Assert.IsNull(TestFactory.Create(null,null));
        }

        [Test]
        public void Prototype_Factory_Create_Default()
        {
            Assert.NotNull(TestFactory.Create<IWidget>());
            foreach(var type in DefaultTypes)
            {
                var instance = TestFactory.Create(type, null);
                Assert.NotNull(instance, $"{type.FullName}");
            }
        }
        [Test]
        public void Prototype_Factory_Create_From_IDictionary()
        {
            foreach(var type in FromIDictionaryTypes)
            {
                var idictionaries = GetIDictionaries(type);
                Assert.True(idictionaries.Count > 0,$"no test dictionaries for type {type.FullName}");

                foreach(var dictionary in idictionaries)
                {
                    var instance = TestFactory.Create(type, dictionary);
                    Assert.NotNull(instance, $"unable to create  of type {type.FullName} from IDictionary");

                    var matrix3d = TestFactory.Create<Matrix3D>(instance);
                }
            }
        }

        private List<IDictionary> GetIDictionaries(Type type)
        {
            var list = new List<IDictionary>();
            foreach(var name in typeof(FactoryTest).Assembly.GetManifestResourceNames())
            {
                if(name.Contains($"{type.Name}.") && name.Contains(".json"))
                {
                    var dictionary = TestFactory.Create<IDictionary>(name);
                    list.Add(dictionary);
                }
            }
            return list;
        }

        [Test]
        public void Prototype_Factory_Create_Widget1()
        {
            var factory = new Factory { AbstractTypes = AbstractTypes, Resources = Resources };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            factory.IDictionaryTypes.Add("Widget", typeof(Widget));
            factory.IDictionaryTypes.Add("Foo", typeof(Foo));
            var widget1 = factory.Create<IWidget>("Widget.1.json");
            Assert.NotNull(widget1,nameof(widget1));
            var foo = widget1["foo0"] as IFoo;
            Assert.NotNull(foo, nameof(foo));
        }
    }
}
