using NUnit.Framework;
namespace Node.Net._IO
{
    [TestFixture, Category("Node.Net.IO.FileList")]
    class FileList_Test
    {
        [TestCase]
        public void FileList_Usage()
        {
            var originalDir = System.Environment.CurrentDirectory;
            try
            {
                var files = new FileList("**/*.txt");

                using (TemporaryDirectory tempDir = new TemporaryDirectory())
                {
                    Assert.True(System.IO.Directory.Exists(tempDir.FullName));
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(tempDir.GetFileName("A.txt")))
                    {
                        sw.WriteLine("A");
                    }
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(tempDir.GetFileName(@"B\C.txt")))
                    {
                        sw.WriteLine("C");
                    }
                    System.Environment.CurrentDirectory = tempDir.FullName;
                    var filenames = files.Collect();
                    Assert.AreEqual(2, filenames.Length);
                    Assert.AreEqual("A.txt", filenames[0]);
                    Assert.AreEqual(@"B\C.txt", filenames[1]);
                }
            }
            finally
            {
                System.Environment.CurrentDirectory = originalDir;
            }
        }
    }
}