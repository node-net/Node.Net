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
            SDIWindow window = new SDIWindow("SDIWindow_Usage_Text_SingleView",
                                            typeof(Node.Net.Collections.Dictionary),
                                            "JSON Files (*.json)|*.json|All Files (*.*)|*.*",
                                            new Node.Net.View.JsonView(),
                                            new Node.Net.View.TreeView());
            window.ShowDialog();
        }
    }
}
