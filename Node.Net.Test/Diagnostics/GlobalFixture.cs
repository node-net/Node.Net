using NUnit.Framework;
using System.IO;

namespace Node.Net.Diagnostics
{
    [TestFixture]
    class GlobalFixture
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CleanUpTempDirectories();
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            CleanUpTempDirectories();
        }

        public static string GetTempPath(string subdir = "")
        {
            var dir = $"{FileSystem.TempDir}\\Node.Net.Diagnostics.Test".Replace("\\\\","\\");
            if (subdir.Length > 0) dir = $"{dir}\\{subdir}";
            /*
            if(Directory.Exists(dir))
            {
                FileSystem.Delete(dir);
            }*/
            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        public static void CleanUpTempDirectories()
        {
            FileSystem.Delete(GetTempPath());
        }
    }
}
