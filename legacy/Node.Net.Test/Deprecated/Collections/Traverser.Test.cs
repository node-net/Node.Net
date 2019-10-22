using System.Collections;
using NUnit.Framework;

namespace Node.Net.Deprecated.Collections
{
    [TestFixture,Category("Collections.Traverser")]
    class Traverser_Test
    {
        [Test]
        public void Traverser_Usage()
        {
            var resources = NodeNetTests.Resources;
            var a = resources["Node.Net.Tests.Resources.Dictionary.Traverser.A.json"] as IDictionary;
            var traverser = new Traverser(a);
            Assert.AreSame(a, traverser.GetParent(a["A"]));
            Assert.AreSame(a, traverser.GetRoot(a["A"]));

            var b = resources["Node.Net.Tests.Resources.Dictionary.Traverser.B.json"] as IDictionary;
            traverser = new Traverser(b);
            Assert.AreSame(b, traverser.GetParent(b["A"]));
            //IDictionary hA0A0 = traverser.Get("A/0/A/0") as IDictionary;
            //Assert.NotNull(hA0A0);
            //IDictionary hA0A0 = b["A"]["0"]["A"]["0"] as IDictionary;
            //Assert.NotNull(hA0A0);
            //Assert.AreSame(b, traverser.GetRoot(hA0A0));
            /*
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Traverser_Test));
            Hash hash = new Hash(IO.StreamExtension.GetStream("Json.Traverser.Test.Tree.json"));
            Traverser traverser = new Traverser(hash);
            NUnit.Framework.Assert.AreSame(hash, traverser.GetParent(hash["A"]));
            NUnit.Framework.Assert.AreSame(hash, traverser.GetRoot(hash["A"]));

            hash = new Hash(IO.StreamExtension.GetStream("Json.Traverser.Test.Tree.2.json"));
            traverser = new Traverser(hash);
            NUnit.Framework.Assert.AreSame(hash, traverser.GetParent(hash["A"]));
            IDictionary hA0A0 = hash["A"]["0"]["A"]["0"] as IDictionary;
            Assert.NotNull(hA0A0);
            Assert.AreSame(hash, traverser.GetRoot(hA0A0));
            */
        }

        /*
        class Foo : Hash { }
        [Test]
        public void Traverser_AncestorByType()
        {
            Hash hash = new Hash();
            hash["foo"] = new Foo();
            hash["foo"]["h2"] = new Hash("{'Name':'Foo/h2'}");
            Traverser traverser = new Traverser(hash);
            NUnit.Framework.Assert.AreSame(hash["foo"], traverser.GetAncestor<Foo>(hash["foo"]["h2"]));
        }*/
    }
}
