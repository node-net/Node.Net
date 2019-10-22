using NUnit.Framework;
namespace Node.Net.Environment
{
    [TestFixture, NUnit.Framework.Category("Node.Net.Environment.MyDocuments")]
    class MyDocuments_Test
    {
        [TestCase]
        public void MyDocuments_Usage()
        {
            // GetFileName
            var filename = MyDocuments.GetFileName("Test.txt");

            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);

        }
    }
}