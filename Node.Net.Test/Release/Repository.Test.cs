using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Node.Net.Tests
{
    [TestFixture]
    class RepositoryTest
    {
        /*
        [Test, Apartment(System.Threading.ApartmentState.STA)]
        [TestCase("image.jpg")]
        [TestCase("info.json")]
        [TestCase("mesh.xaml")]
        public void Repository_Usage(string name)
        {
            var item = Reader.Default.Read(typeof(RepositoryTest), name);
            Assert.NotNull(item, nameof(item));
            var repository = new Repository();
            repository.Set("item", item);
            var clone = repository.Get("item");
            Assert.NotNull(clone, nameof(clone));

        }

        class Foo : Dictionary<string, dynamic> { }
        [Test]
        public void Repository_Custom_Type()
        {
            var foo = new Foo();
            foo["Type"] = "Foo";
            var types = new Dictionary<string, Type>();
            types.Add("Foo", typeof(Foo));

            var repository = new Repository
            {
                ReadFunction = new Reader { Types = types }.Read
            };

            repository.Set("foo", foo);
            var foo_clone = repository.Get("foo") as Foo;
            Assert.NotNull(foo_clone, nameof(foo_clone));
        }*/
    }
}
