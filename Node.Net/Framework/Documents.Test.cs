using NUnit.Framework;


namespace Node.Net.Framework
{
    [TestFixture,Category("Node.Net.Framework.Documents")]
    class Documents_Test
    {
        [TestCase]
        public void Documents_Usage()
        {
            Documents documents = new Documents();
            Assert.AreEqual(1, documents.MaximumCount, "default MaximumCount for Documents is not 1");
            Assert.AreEqual(0, documents.Count, "documents.Count is not zero");
            Assert.AreSame(typeof(Collections.Dictionary), documents.DefaultDocumentType);

            documents.Open("Documents.Test.Sample.json");
            Assert.AreEqual(1, documents.Count, "documents.Count is not 1.");
            Assert.AreSame(typeof(Collections.Dictionary), documents["Documents.Test.Sample.json"].GetType());

            Collections.Dictionary doc = documents["Documents.Test.Sample.json"] as Collections.Dictionary;
            Assert.True(doc.ContainsKey("Name"));
        }
    }
}
