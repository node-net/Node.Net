using NUnit.Framework;

namespace Node.Net.IO
{
    [TestFixture, Category("Node.Net.IO.Directory")]
    class Directory_Test
    {
        [TestCase]
        public void Directory_IsGlobMatch()
        {
            Assert.True(Directory.IsGlobMatch("example.txt", "*.txt"));
            Assert.False(Directory.IsGlobMatch("example.bin", "*.txt"));
            Assert.True(Directory.IsGlobMatch("example.txt", "**/*.txt"));
        }

        [TestCase]
        public void Directory_Glob()
        {
            string directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\tmp\LEP.IO.Directory_Test\Directory_Glob";
            System.Collections.Generic.Dictionary<string, object> directoryDictionary = new System.Collections.Generic.Dictionary<string, object>();
            directoryDictionary.Add("example.txt", "abcdefg");
            directoryDictionary.Add(@"code\Widget.cs", "public class Widget{}");
            Directory.Create(directory, directoryDictionary);

            string[] names = Directory.Glob(directory, "*.txt");
            Assert.AreEqual(1, names.Length);
            
            names = Directory.Glob(directory, "**/*.txt");
            Assert.AreEqual(1, names.Length, "glob '**/*.txt'");

            names = Directory.Glob(directory, "**/*.cs");
            Assert.AreEqual(1, names.Length, "glob '**/*.cs'");
            System.IO.Directory.Delete(directory, true);

        }

        [TestCase]
        public void Directory_Zip()
        {
            string directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\tmp\LEP.IO.Directory_Test\Directory_Zip";
            if(System.IO.Directory.Exists(directory)) System.IO.Directory.Delete(directory, true);

            System.Collections.Generic.Dictionary<string, object> directoryDictionary = new System.Collections.Generic.Dictionary<string, object>();
            directoryDictionary.Add("example.txt", "abcdefg");
            directoryDictionary.Add(@"code\Widget.cs", "public class Widget{}");
            Directory.Create(directory, directoryDictionary);

            Assert.True(System.IO.Directory.Exists(directory));

            string zipfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\tmp\LEP.IO.Directory_Test\Directory_Zip.zip";
            if(System.IO.File.Exists(zipfile)) System.IO.File.Delete(zipfile);
            Directory.Zip(directory,zipfile);
            Assert.True(System.IO.File.Exists(zipfile),zipfile + " does not exist");
            System.IO.Directory.Delete(directory, true);
            Assert.False(System.IO.Directory.Exists(directory));
            Directory.UnZip(zipfile, directory);
            Assert.True(System.IO.Directory.Exists(directory));
            System.IO.Directory.Delete(directory, true);
            System.IO.File.Delete(zipfile);
        }
    }
}
