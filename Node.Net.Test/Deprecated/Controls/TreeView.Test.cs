using NUnit.Framework;
using System.Threading;
using System.Windows;

namespace Node.Net.Deprecated.Controls
{
    [TestFixture,Category("Node.Net.Controls.TreeView")]
    class TreeViewTest
    {
        [TestCase,Explicit,Apartment(ApartmentState.STA)]
        public void TreeView_Usage_ShowDialog()
        {
            //TabControl tabControl = new TabControl();
            //tabControl.Items.Add(new TabItem() { Header = "null", Content=new ExplorerControl() { DataContext = null } });
            var w = new Window
            {
                Title = nameof(TreeView_Usage_ShowDialog),
                Content = Deprecated.Controls.ExampleDataContexts.Default.GetTabControl(typeof(Node.Net.Deprecated.Controls.TreeView))
            };
            w.ShowDialog();
        }
    }
}
