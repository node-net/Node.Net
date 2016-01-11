using NUnit.Framework;


namespace Node.Net.Documents
{
    [TestFixture,Category("Node.Net.Documents.Documents")]
    class Documents_Test
    {
        [TestCase]
        public void Documents_SDI_Usage()
        {
            Documents docs = new Documents();
            docs.MaximumCount = 1;
            docs.Types.Add("txt", typeof(TextDocument));
            docs.Open("Documents.Test.Sample.txt");
            NUnit.Framework.Assert.True(docs.ContainsKey("Documents.Test.Sample.txt"),
                        "docs does not contain 'Documents.Test.Sample.txt'");
            NUnit.Framework.Assert.AreEqual(typeof(TextDocument), docs["Documents.Test.Sample.txt"].GetType(), "doc is not of type TextDocument.");

            Assert.AreEqual(1, docs.Count);
            string filename = System.Environment.GetFolderPath(
                               System.Environment.SpecialFolder.MyDocuments) +
                                @"\Node.Net.Documents.Documents_Test.Documents_SDI_Usage.txt";
            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
            Documents.Save(filename, docs["Documents.Test.Sample.txt"]);
            NUnit.Framework.Assert.True(System.IO.File.Exists(filename),filename + " does not exist.");
            System.IO.File.Delete(filename);

            docs.Open("Documents.Test.Sample2.txt");
            Assert.AreEqual(1, docs.Count);
        }

        [TestCase]
        public void Documents_New()
        {
            Documents docs = new Documents();
            NUnit.Framework.Assert.AreEqual(0, docs.Count);
            docs.New();
            NUnit.Framework.Assert.AreEqual(1, docs.Count);
            docs.New();
            NUnit.Framework.Assert.AreEqual(2, docs.Count);

            docs.Clear();
            docs.MaximumCount = 1;
            docs.New();
            docs.New();
            NUnit.Framework.Assert.AreEqual(1, docs.Count);
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Documents_Open()
        {
            Documents docs = new Documents();
            docs.Open();
            NUnit.Framework.Assert.AreEqual(1, docs.Count);
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Documents_Save()
        {
            Documents docs = new Documents();
            docs.New();
            NUnit.Framework.Assert.AreEqual(1, docs.Count);
            docs.Save();
        }
    }
}
