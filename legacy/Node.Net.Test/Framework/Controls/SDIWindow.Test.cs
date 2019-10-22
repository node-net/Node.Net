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
            var window = new SDIWindow(nameof(SDIWindow_Usage_Text_SingleView),
                                           typeof(Node.Net.Deprecated.Documents.TextDocument),
                                           "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                                           new Node.Net.Deprecated.Controls.ReadOnlyTextBox());
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_MultipleViews()
        {
            FrameworkElement[] views = {new Node.Net.Deprecated.Controls.ReadOnlyTextBox(),
                                        new Node.Net.Deprecated.Controls.TreeView(),
                                        new Node.Net.View.HelixView3D()};
            var window = new SDIWindow(nameof(SDIWindow_Usage_Text_SingleView),
                                            typeof(Node.Net.Deprecated.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            views);
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_MultipleViews2()
        {
            var views = new Dictionary<string, FrameworkElement>();
            views["JSON"] = new Node.Net.Deprecated.Controls.ReadOnlyTextBox();
            views["Tree"] = new Node.Net.Deprecated.Controls.TreeView();
            var window = new SDIWindow(nameof(SDIWindow_Usage_Dictionary_MultipleViews2),
                                            typeof(Node.Net.Deprecated.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            views);
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_ExplorerView()
        {
            var window = new SDIWindow(nameof(SDIWindow_Usage_Dictionary_ExplorerView),
                                            typeof(Node.Net.Deprecated.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            new View.ExplorerView());
            window.ShowDialog();
        }

        [TestCase, Explicit, Apartment(ApartmentState.STA)]
        public void SDIWindow_Usage_Dictionary_ExplorerDynamicView()
        {
            var views = new Dictionary<string, FrameworkElement>();
            views["JSON"] = new Node.Net.Deprecated.Controls.ReadOnlyTextBox();
            views["Tree"] = new Node.Net.Deprecated.Controls.TreeView();

            var window = new SDIWindow(nameof(SDIWindow_Usage_Dictionary_ExplorerView),
                                            typeof(Node.Net.Deprecated.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            new View.ExplorerView
                                            {
                                                ContentView = new DynamicView(views)
                                            });
            window.ShowDialog();
        }
    }
}
