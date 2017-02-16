using NUnit.Framework;


namespace Node.Net.Deprecated.Documents
{
    [TestFixture,Category("Documents.Documents")]
    class Documents_Test
    {
        [TestCase]
        public void Documents_SDI_Usage()
        {
            var docs = new Documents
            {
                MaximumCount = 1
            };
            docs.Types.Add("txt", typeof(TextDocument));
            docs.Open("Text.Sample.A.txt");
            Assert.True(docs.ContainsKey("Text.Sample.A.txt"),
                        "docs does not contain 'Text.Sample.A.txt'");
            Assert.AreEqual(typeof(TextDocument), docs["Text.Sample.A.txt"].GetType(), "doc is not of type TextDocument.");

            Assert.AreEqual(1, docs.Count);
            var filename = System.Environment.GetFolderPath(
                               System.Environment.SpecialFolder.MyDocuments) +
                                @"\Node.Net.Documents.Documents_Test.Documents_SDI_Usage.txt";
            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
            Documents.Save(filename, docs["Text.Sample.A.txt"]);
            Assert.True(System.IO.File.Exists(filename), filename + " does not exist.");
            System.IO.File.Delete(filename);

            docs.Open("Text.Sample.B.txt");
            Assert.AreEqual(1, docs.Count);
        }

        [TestCase]
        public void Documents_New()
        {
            var docs = new Documents();
            Assert.AreEqual(0, docs.Count);
            docs.New();
            Assert.AreEqual(1, docs.Count);
            docs.New();
            Assert.AreEqual(2, docs.Count);

            docs.Clear();
            docs.MaximumCount = 1;
            docs.New();
            docs.New();
            Assert.AreEqual(1, docs.Count);
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Documents_Open()
        {
            var docs = new Documents();
            docs.Open();
            Assert.AreEqual(1, docs.Count);
        }

        [TestCase, NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void Documents_Save()
        {
            var docs = new Documents();
            docs.New();
            Assert.AreEqual(1, docs.Count);
            docs.Save();
        }
    }
}
