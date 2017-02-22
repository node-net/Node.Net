using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    class IFactoryTest
    {
        public static void IFactory_Test_DefaultConstructor(IFactory factory,Type[] types)
        {
            foreach (var type in types)
            {
                // Default Construction
                var instance = factory.Create(type, null);
                Assert.NotNull(instance, $"Default Construction of {type.FullName}");
                Assert.True(type.IsAssignableFrom(instance.GetType()), $"Default Construction {type.FullName} is not assignable from {instance.GetType().FullName}");
            }
        }

        public static void IFactory_Test_IDictionaryConstructor(IFactory factory,Type[] types)
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

        public static void IFactory_Test_CreateFromManifestResourceStream(IFactory factory, Type[] types)
        {
            foreach (var type in types)
            {
                // ManifestResource Construction
                var name = type.Name.Substring(1);
                for (int i = 0; i < 10; ++i)
                {

                    var manifestResourceName = $"{name}.{i}.";
                    var stream = factory.Create<Stream>(manifestResourceName);
                    if (stream != null)
                    {
                        try
                        {
                            var instance = factory.Create(type, manifestResourceName);

                            Assert.NotNull(instance, $"ManifestResource Construction of {type.FullName} from {manifestResourceName}");
                            Assert.True(type.IsAssignableFrom(instance.GetType()), $"ManifestResource Construction {type.FullName} is not assignable from {instance.GetType().FullName} {manifestResourceName}");
                        }
                        catch(Exception e)
                        {
                            throw new Exception($"Error creating instance from '{manifestResourceName}'", e);
                        }
                    }
                    else { break; }
                }
            }
        }

        public static void IFactory_Test_CreateMedia3DFromIDictionary(IFactory factory,IDictionary data)
        {
            Assert.NotNull(data);

            var v3d = factory.Create<Visual3D>(data);
            Assert.NotNull(v3d, nameof(v3d));

            var m3d = factory.Create<Model3D>(data);
            Assert.NotNull(m3d, nameof(m3d));



        }
    }
}
