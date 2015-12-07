

namespace Node.Net.View
{
    

    [NUnit.Framework.TestFixture]
    class SDIApplication_Test
    {
        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void SDIApplication_DictionaryModel_Usage()
        {
            System.Collections.Generic.Dictionary<string,object> hash
                = new System.Collections.Generic.Dictionary<string,object>();
            System.Collections.Generic.KeyValuePair<string, object> sdiAppModel
                = new System.Collections.Generic.KeyValuePair<string, object>("MyDocument", hash);
            SDIApplication sdiAppView = new SDIApplication(sdiAppModel);
            System.Windows.Window window = new System.Windows.Window() { Content = sdiAppView, Title = "SDIApplication" };
            window.ShowDialog();
        }

        public class NotePadDocument : System.Collections.Generic.List<string>
        {
            private System.IO.StreamReader streamReader = null;
            public void Open(System.IO.Stream stream)
            {
                Clear();
                streamReader = new System.IO.StreamReader(stream);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    Add(line);
                }
            }
            private System.IO.StreamWriter streamWriter = null;
            public void Save(System.IO.Stream stream)
            {
                streamWriter = new System.IO.StreamWriter(stream);
                foreach (string line in this)
                {
                    streamWriter.WriteLine(line);
                }
                streamWriter.Flush();
            }
        }

        [NUnit.Framework.TestCase,NUnit.Framework.RequiresSTA,NUnit.Framework.Explicit]
        public void SDIApplication_Notepad_Usage()
        {
            NotePadDocument doc = new NotePadDocument();
            for (int i = 0; i < 100; ++i)
            {
                doc.Add("The quick brown fox");
                doc.Add("jumps over the lazy dog.");
            }
            System.Collections.Generic.KeyValuePair<string, object> sdiAppModel
                = new System.Collections.Generic.KeyValuePair<string, object>("MyDocument", doc);
            SDIApplication sdiAppView = new SDIApplication(sdiAppModel);
            sdiAppView.DocumentViewType = typeof(TextView);
            System.Windows.Window window = new System.Windows.Window() { Content = sdiAppView, Title = "MyDocument - NotepadTest" };
            window.ShowDialog();
        }
    }
}
