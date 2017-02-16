using NUnit.Framework;

namespace Node.Net.View
{


    [TestFixture]
    class SDIApplication_Test
    {
        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA), NUnit.Framework.Explicit]
        public void SDIApplication_DictionaryModel_Usage()
        {
            var hash
                = new System.Collections.Generic.Dictionary<string,object>();
            var sdiAppModel
                = new System.Collections.Generic.KeyValuePair<string, object>("MyDocument", hash);
            var sdiAppView = new SDIApplication(sdiAppModel);
            var window = new System.Windows.Window { Content = sdiAppView, Title = nameof(SDIApplication) };
            window.ShowDialog();
        }

        public class NotePadDocument : System.Collections.Generic.List<string>
        {
            private System.IO.StreamReader streamReader;
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
            private System.IO.StreamWriter streamWriter;
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

        [TestCase,NUnit.Framework.Apartment(System.Threading.ApartmentState.STA),NUnit.Framework.Explicit]
        public void SDIApplication_Notepad_Usage()
        {
            var doc = new NotePadDocument();
            for (int i = 0; i < 100; ++i)
            {
                doc.Add("The quick brown fox");
                doc.Add("jumps over the lazy dog.");
            }
            var sdiAppModel
                = new System.Collections.Generic.KeyValuePair<string, object>("MyDocument", doc);
            var sdiAppView = new SDIApplication(sdiAppModel)
            {
                DocumentViewType = typeof(TextView)
            };
            var window = new System.Windows.Window { Content = sdiAppView, Title = "MyDocument - NotepadTest" };
            window.ShowDialog();
        }
    }
}
