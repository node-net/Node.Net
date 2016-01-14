using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Node.Net.Framework.Controls
{
    [TestFixture,Category("Node.Net.Framework.Controls.SDIControl")]
    class SDIControlTest
    {
        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Default()
        {
            Window window = new Window()
            {
                Title = "SDIControl_Usage_Default",
                Content = new SDIControl(),
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Text_SingleView()
        {
            Window window = new Window()
            {
                Title = "SDIControl_Usage_Default",
                Content = new SDIControl()
                {
                    Documents = new Framework.Documents()
                    {
                        DefaultDocumentType=typeof(Node.Net.Documents.TextDocument),
                        OpenFileDialogFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.View.ReadOnlyTextView()
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Dictionary_SingleView()
        {
            Window window = new Window()
            {
                Title = "SDIControl_Usage_Default",
                Content = new SDIControl()
                {
                    Documents = new Framework.Documents()
                    {
                        DefaultDocumentType = typeof(Node.Net.Collections.Dictionary),
                        OpenFileDialogFilter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.View.JsonView()
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Dictionary_MultipleViews()
        {
            Dictionary<string, FrameworkElement> namedViews = new Dictionary<string, FrameworkElement>();
            namedViews["JSON"] = new Node.Net.View.JsonView();
            namedViews["Tree"] = new Node.Net.View.TreeView();

            Window window = new Window()
            {
                Title = "SDIControl_Usage_Default",
                Content = new SDIControl()
                {
                    Documents = new Framework.Documents()
                    {
                        DefaultDocumentType = typeof(Node.Net.Collections.Dictionary),
                        OpenFileDialogFilter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.Framework.Controls.DynamicView()
                    {
                        NamedViews = namedViews
                    }
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }
    }
}
