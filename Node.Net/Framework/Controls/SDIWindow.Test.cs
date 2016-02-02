using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Node.Net.Framework.Controls
{
    [TestFixture,Category("Node.Net.Framework.Controls.SDIWindow")]
    class SDIWindowTest
    {

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Text_SingleView()
        {
            SDIWindow window = new SDIWindow("SDIWindow_Usage_Text_SingleView",
                                           typeof(Node.Net.Documents.TextDocument),
                                           "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                                           new Node.Net.View.ReadOnlyTextView());
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_MultipleViews()
        {
            FrameworkElement[] views = {new Node.Net.Controls.ReadOnlyTextBox(),
                                        new Node.Net.View.TreeView(),
                                        new Node.Net.View.HelixView3D()};
            SDIWindow window = new SDIWindow("SDIWindow_Usage_Text_SingleView",
                                            typeof(Node.Net.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            views);
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_MultipleViews2()
        {
            Dictionary<string, FrameworkElement> views = new Dictionary<string, FrameworkElement>();
            views["JSON"] = new Node.Net.Controls.ReadOnlyTextBox();
            views["Tree"] = new Node.Net.View.TreeView();
            SDIWindow window = new SDIWindow("SDIWindow_Usage_Dictionary_MultipleViews2",
                                            typeof(Node.Net.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            views);
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_ExplorerView()
        {
            SDIWindow window = new SDIWindow("SDIWindow_Usage_Dictionary_ExplorerView",
                                            typeof(Node.Net.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            new View.ExplorerView());
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_ExplorerDynamicView()
        {
            Dictionary<string, FrameworkElement> views = new Dictionary<string, FrameworkElement>();
            views["JSON"] = new Node.Net.Controls.ReadOnlyTextBox();
            views["Tree"] = new Node.Net.View.TreeView();

            SDIWindow window = new SDIWindow("SDIWindow_Usage_Dictionary_ExplorerView",
                                            typeof(Node.Net.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            new View.ExplorerView()
                                            {
                                                ContentView = new DynamicView(views)
                                            });
            window.ShowDialog();
        }
    }
}
