

using NUnit.Framework;
namespace Node.Net.Json
{
    [TestFixture,Category("Node.Net.Json.Traverser")]
    class Traverser_Test
    {
        [TestCase]
        public void Traverser_Usage()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Traverser_Test));
            Hash hash = new Hash(assembly.GetManifestResourceStream("Node.Net.Node.Net.Json.Traverser_Test.Tree.json"));
            Traverser traverser = new Traverser(hash);
            NUnit.Framework.Assert.AreSame(hash, traverser.GetParent(hash["A"]));
            NUnit.Framework.Assert.AreSame(hash, traverser.GetRoot(hash["A"]));

            hash = new Hash(assembly.GetManifestResourceStream("Node.Net.Node.Net.Json.Traverser_Test.Tree.2.json"));
            traverser = new Traverser(hash);
            NUnit.Framework.Assert.AreSame(hash, traverser.GetParent(hash["A"]));
            Hash hA0A0 = hash["A"]["0"]["A"]["0"] as Hash;
            Assert.NotNull(hA0A0);
            Assert.AreSame(hash, traverser.GetRoot(hA0A0));
        }

        class Foo : Hash { }
        [TestCase]
        public void Traverser_AncestorByType()
        {
            Hash hash = new Hash();
            hash["foo"] = new Foo();
            hash["foo"]["h2"] = new Hash("{'Name':'Foo/h2'}");
            Traverser traverser = new Traverser(hash);
            NUnit.Framework.Assert.AreSame(hash["foo"], traverser.GetAncestor<Foo>(hash["foo"]["h2"]));
        }
    }
}
