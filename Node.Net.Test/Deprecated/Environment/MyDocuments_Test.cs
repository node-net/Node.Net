namespace Node.Net.Environment
{
    [NUnit.Framework.TestFixture, NUnit.Framework.Category("Environment")]
    class MyDocuments_Test
    {
        [NUnit.Framework.TestCase]
        public void MyDocuments_Usage()
        {
            // GetFileName
            string filename = MyDocuments.GetFileName("Test.txt");

            if (System.IO.File.Exists(filename)) System.IO.File.Delete(filename);
        
        }
    }
}