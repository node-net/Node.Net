using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.SDIControl")]
    class SDIControlTest
    {
        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Text_SingleView()
        {
            Window window = new Window()
            {
                Title = "SDIControl_Usage_Default",
                Content = new SDIControl()
                {
                    Documents = new Collections.Documents()
                    {
                        DefaultDocumentType = typeof(Node.Net.Documents.TextDocument),
                        OpenFileDialogFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.View.ReadOnlyTextView()
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_JSON_SingleView()
        {
            Window window = new Window()
            {
                Title = "SDIControl_Usage_JSON_SingleView",
                Content = new SDIControl()
                {
                    Documents = new Collections.Documents()
                    {
                        DefaultDocumentType = typeof(Collections.Dictionary),
                        OpenFileDialogFilter = "JSON Files (*.json)|*.json"
                    },
                    DocumentView = new Controls.ReadOnlyTextBox()
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
