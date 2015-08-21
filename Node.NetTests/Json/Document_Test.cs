using NUnit.Framework;
using System.Reflection;

namespace Node.Net.Json.Test
{
    [TestFixture, NUnit.Framework.Category("Json"),NUnit.Framework.Category("Document")]
    class Document_Test
    {
        [TestCase]
        public void Document_Usage()
        {
            Document doc = new Document();
            doc["childA"] = new Hash();
            doc.Update();
            NUnit.Framework.Assert.AreSame(doc, doc["childA"].Document);
        }

        [TestCase]
        public void Document_ReadFromStream()
        {
            Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(Document_Test));
            Document doc = new Document(assembly.GetManifestResourceStream("Node.Net.Json.Traverser_Test.Tree.2.json"));
            Hash a = doc["A"] as Hash;
            Assert.NotNull(a);
            Assert.AreSame(doc, a.Document);
        }

        [TestCase]
        public void Document_Tree_Traversal()
        {
            Document doc = new Document();
            Assert.AreSame(doc, doc.Document);
            Assert.IsNull(doc.Parent);
            Hash a = new Hash();
            doc["a"] = a;
            doc.Update();
            Assert.AreSame(doc, a.Document);
            Assert.AreSame(doc, a.Parent);
        }
    }
}
