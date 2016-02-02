using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace Node.Net.Controls
{
    [TestFixture,Category("Node.Net.Controls.TreeView")]
    class TreeViewTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void TreeView_Usage_ShowDialog()
        {
            //TabControl tabControl = new TabControl();
            //tabControl.Items.Add(new TabItem() { Header = "null", Content=new ExplorerControl() { DataContext = null } });
            Window w = new Window()
            {
                Title = "TreeView_Usage_ShowDialog",
                Content = ExampleDataContexts.Default.GetTabControl(typeof(Node.Net.Controls.TreeView))
            };
            w.ShowDialog();
        }
    }
}
