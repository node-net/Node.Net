using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Interfaces
{
    class IFactoryTest
    {
        public void Factory_Create()
        {
            var factory = new Factory();
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

        public static void Factory_Test_DefaultConstructor(IFactory factory,Type[] types)
        {
            foreach (var type in types)
            {
                // Default Construction
                var instance = factory.Create(type, null);
                Assert.NotNull(instance, $"Default Construction of {type.FullName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"Default Construction {type.FullName} is not assignable from {instance.GetType().FullName}");
            }
        }

        public static void Factory_Test_IDictionaryConstructor(IFactory factory,Type[] types)
        {
            foreach (var type in types)
            {
                // IDictionary constructor
                var name = type.Name.Substring(1);
                var instance = factory.Create(type, new Dictionary<string, dynamic> { { "Type", name } });
                Assert.NotNull(instance, $"IDictionary Construction of {type.FullName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"IDictionary Construction {type.FullName} is not assignable from {instance.GetType().FullName}");
            }
        }

        public static void Factory_Test_CreateFromManifestResourceStream(IFactory factory, Type[] types)
        {
            foreach (var type in types)
            {
                // ManifestResource Construction
                var name = type.Name.Substring(1);
                var manifestResourceName = $"{name}.0.json";
                var instance = factory.Create(type, manifestResourceName);
                Assert.NotNull(instance, $"ManifestResource Construction of {type.FullName} from {manifestResourceName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"ManifestResource Construction {type.FullName} is not assignable from {instance.GetType().FullName} {manifestResourceName}");
            }
        }
    }
}
