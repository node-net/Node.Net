using NUnit.Framework;
namespace Node.Net._IO
{
    [TestFixture,Category("Node.Net.IO.TemporaryDirectory")]
    class TemporaryDirectory_Test
    {
        [TestCase]
        public void TemporaryDirectory_Usage()
        {
            var fullName = "";
            using (TemporaryDirectory tempDir = new TemporaryDirectory())
            {
                fullName = tempDir.FullName;
                Assert.True(System.IO.Directory.Exists(fullName));
                var filename = tempDir.GetFileName(@"Resources\test.xaml");
                var fi = new System.IO.FileInfo(filename);
                Assert.True(System.IO.Directory.Exists(fi.DirectoryName));
            }
            Assert.False(System.IO.Directory.Exists(fullName));
        }
    }
}
