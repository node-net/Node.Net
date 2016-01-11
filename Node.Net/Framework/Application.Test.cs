using NUnit.Framework;
using System.Threading;

namespace Node.Net.Framework
{
    [TestFixture,Category("Node.Net.Framework.Application")]
    class ApplicationTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void TextViewer_Usage()
        {
            Application application = new Application();
            application.MainWindow = new System.Windows.Window()
            {
                Title = "Node.Net.Framework.TextViewer",
                Content = new Node.Net.Framework.Controls.SDIControl()
                {
                    DataContext = new Documents() { DefaultDocumentType = typeof(Node.Net.Documents.TextDocument) },
                    DocumentView = new Node.Net.View.TextView()
                }
            };
            application.Run();
        }
    }
}
