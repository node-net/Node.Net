using NUnit.Framework;

namespace Node.Net.Documents
{
    [TestFixture,Category("Node.Net.Documents.TextDocument")]
    public class TextDocument_Test
    {
        [TestCase]
        public void TextDocument_Usage()
        {
            /*
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(TextDocument_Test));
            TextDocument textDoc = new TextDocument();
            textDoc.Open(assembly.GetManifestResourceStream("Node.Net.Resources.Sample.txt"));
            NUnit.Framework.Assert.AreEqual("a", textDoc[0], "first line of text document is not 'a'.");

            string text = "";
            using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                textDoc.Save(memory);
                memory.Flush();
                memory.Seek(0, System.IO.SeekOrigin.Begin);
                System.IO.StreamReader sr = new System.IO.StreamReader(memory);
                text = sr.ReadToEnd();
            }
            NUnit.Framework.Assert.True(text.Contains("a"));
            NUnit.Framework.Assert.True(text.Contains("b"));
             */
        }
    }
}
