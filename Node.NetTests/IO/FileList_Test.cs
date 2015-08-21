using NUnit.Framework;
namespace Node.Net.IO
{
    [TestFixture, Category("IO"), Category("FileList")]
    class FileList_Test
    {
        [TestCase]
        public void FileList_Usage()
        {
            string originalDir = System.Environment.CurrentDirectory;
            try
            {
                FileList files = new FileList("**/*.txt");

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
                    string[] filenames = files.Collect();
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