using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Media.Media3D;

namespace Node.Net.Factory.Test.Factories.TypeSourceFactories
{
    [TestFixture]
    class ObjectFromStringTest
    {
        [Test]
        public void ObjectFromString_Null()
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromString();
            Assert.IsNull(factory.Create<object>(null));
        }

        [Test, Apartment(System.Threading.ApartmentState.STA)]
        [TestCase(typeof(MeshGeometry3D),"Mesh.Cube.xaml")]
        [TestCase(typeof(IDictionary),"Scene.Cube.json")]
        [TestCase(typeof(IList),"List")]
        public void ObjectFromString(Type targetType,string source)
        {
            var factory = new Node.Net.Factory.Factories.TypeSourceFactories.ObjectFromString { GetFunction = GetFunction };
            factory.ResourceAssemblies.Add(typeof(StreamFromStringTest).Assembly);
            if (source.Contains(".json")) factory.ReadFunction = ReadJson;
            Assert.True(targetType.IsAssignableFrom(factory.Create(targetType, source).GetType()));
        }

        public static object GetFunction(string name)
        {
            if(name == "List")
            {
                return new List<dynamic>();
            }
            return null;
        }
        public static object ReadJson(Stream stream)
        {
            var reader = new Node.Net.Factory.Test.Internal.JsonReader();
            return reader.Read(stream);
        }
    }
}
