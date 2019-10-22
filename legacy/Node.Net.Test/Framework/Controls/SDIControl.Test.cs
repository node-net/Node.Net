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
            var window = new Window
            {
                Title = nameof(SDIControl_Usage_Default),
                Content = new SDIControl(),
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Text_SingleView()
        {
            var window = new Window
            {
                Title = nameof(SDIControl_Usage_Default),
                Content = new SDIControl
                {
                    Documents = new Framework.Documents
                    {
                        DefaultDocumentType=typeof(Node.Net.Deprecated.Documents.TextDocument),
                        OpenFileDialogFilter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.Deprecated.Controls.ReadOnlyTextBox()
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Dictionary_SingleView()
        {
            var window = new Window
            {
                Title = nameof(SDIControl_Usage_Default),
                Content = new SDIControl
                {
                    Documents = new Framework.Documents
                    {
                        DefaultDocumentType = typeof(Node.Net.Deprecated.Collections.Dictionary),
                        OpenFileDialogFilter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.Deprecated.Controls.ReadOnlyTextBox()
                },
                WindowState = WindowState.Maximized
            };
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIControl_Usage_Dictionary_MultipleViews()
        {
            var namedViews = new Dictionary<string, FrameworkElement>();
            namedViews["JSON"] = new Node.Net.Deprecated.Controls.ReadOnlyTextBox();
            namedViews["Tree"] = new Node.Net.Deprecated.Controls.TreeView();

            var window = new Window
            {
                Title = nameof(SDIControl_Usage_Default),
                Content = new SDIControl
                {
                    Documents = new Framework.Documents
                    {
                        DefaultDocumentType = typeof(Node.Net.Deprecated.Collections.Dictionary),
                        OpenFileDialogFilter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
                    },
                    DocumentView = new Node.Net.Framework.Controls.DynamicView
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
