using NUnit.Framework;
using System.IO;

namespace Node.Net.Framework
{
    [TestFixture,Category("Node.Net.Framework.RecentFiles")]
    class RecentFilesTest
    {
        [TestCase]
        public void RecentFiles_Usage()
        {
            RecentFiles recentFiles = new RecentFiles();
            recentFiles.Insert(0, "C:\temp\test.txt");
            Assert.AreEqual(1, recentFiles.Count);
            //Assert.True(File.Exists(recentFiles.Filename);
        }
    }
}
