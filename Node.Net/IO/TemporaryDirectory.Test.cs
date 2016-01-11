using NUnit.Framework;
namespace Node.Net.IO
{
    [TestFixture,Category("Node.Net.IO.TemporaryDirectory")]
    class TemporaryDirectory_Test
    {
        [TestCase]
        public void TemporaryDirectory_Usage()
        {
            string fullName = "";
            using(TemporaryDirectory tempDir = new TemporaryDirectory())
            {
                fullName = tempDir.FullName;
                Assert.True(System.IO.Directory.Exists(fullName));
                string filename = tempDir.GetFileName(@"Resources\test.xaml");
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                Assert.True(System.IO.Directory.Exists(fi.DirectoryName));
            }
            Assert.False(System.IO.Directory.Exists(fullName));
        }
    }
}
