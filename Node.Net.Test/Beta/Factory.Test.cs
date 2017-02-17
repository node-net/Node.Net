using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Beta
{
    [TestFixture,Category("Beta")]
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
                    {typeof(IDictionary),typeof(Dictionary<string,dynamic>) }
                }
            };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);

            Type[] types = { typeof(IDictionary)};
            Interfaces.IFactoryTest.Factory_Test_DefaultConstructor(factory, types);
            Interfaces.IFactoryTest.Factory_Test_IDictionaryConstructor(factory, types);
            Interfaces.IFactoryTest.Factory_Test_CreateFromManifestResourceStream(factory, types);
        }
        [Test]
        public void Factory_Create()
        {
            var factory = new Beta.Factory
            {
                AbstractTypes = new Dictionary<Type, Type>
                {
                    {typeof(IDictionary),typeof(Dictionary<string,dynamic>) }
                }
            };
            factory.ManifestResourceAssemblies.Add(typeof(FactoryTest).Assembly);
            Type[] types = { typeof(IDictionary) };
            foreach (var type in types)
            {
                // Default Construction
                var instance = factory.Create(type, null);
                Assert.NotNull(instance, $"Default Construction of {type.FullName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"Default Construction {type.FullName} is not assignable from {instance.GetType().FullName}");

                // IDictionary Constructor
                var name = type.Name.Substring(1);
                instance = factory.Create(type, new Dictionary<string, dynamic> { { "Type", name } });
                Assert.NotNull(instance, $"IDictionary Construction of {type.FullName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"IDictionary Construction {type.FullName} is not assignable from {instance.GetType().FullName}");

                // ManifestResource Construction
                var manifestResourceName = $"{name}.0.json";
                instance = factory.Create(type, manifestResourceName);
                Assert.NotNull(instance, $"ManifestResource Construction of {type.FullName} from {manifestResourceName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"ManifestResource Construction {type.FullName} is not assignable from {instance.GetType().FullName} {manifestResourceName}");
            }
        }
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
        static Beta.Factory TestFactory = new Beta.Factory
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
