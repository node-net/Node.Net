using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace Node.Net.Controls.Internal
{
    class TreeViewUpdater
    {
        public TreeViewUpdater(System.Windows.Controls.TreeView treeView)
        {
            treeView.DataContextChanged += TreeView_DataContextChanged;
        }

        private void TreeView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            UpdateTreeView(sender as System.Windows.Controls.TreeView);
        }

        private Dictionary<object, System.Windows.Controls.TreeViewItem> treeViewItems = new Dictionary<object, System.Windows.Controls.TreeViewItem>();
        public void UpdateTreeView(System.Windows.Controls.TreeView treeView)
        {
            treeView.Background = Brushes.Azure;
            treeView.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            treeView.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            treeView.Items.Clear();
            var dictionary = treeView.DataContext.GetValue() as IDictionary;
            if (dictionary != null)
            {
                var keys = new List<string>(TreeViewItemUpdater.GetChildKeys(dictionary));
                foreach (var item in dictionary)
                {
                    if (keys.Contains(item.GetKey()))
                    {
                        System.Windows.Controls.TreeViewItem tvi = null;
                        if (treeViewItems.ContainsKey(item)) tvi = treeViewItems[item];
                        else
                        {
                            tvi = new System.Windows.Controls.TreeViewItem { DataContext = item , HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch};
                            tvi.Tag = new TreeViewItemUpdater(tvi);
                            if (tvi != null) treeViewItems[item] = tvi;
                        }
                        if (tvi != null && !treeView.Items.Contains(tvi)) treeView.Items.Add(tvi);
                    }
                }
            }
                
        }
        public void UpdateTreeViewX(System.Windows.Controls.TreeView treeView)
        {
            treeView.Items.Clear();
            var dictionary = treeView.DataContext as IDictionary;
            if (dictionary != null)
            {

                foreach (var item in dictionary)
                {
                    System.Windows.Controls.TreeViewItem tvi = null;
                    if (treeViewItems.ContainsKey(item)) tvi = treeViewItems[item];
                    else
                    {
                        tvi = new System.Windows.Controls.TreeViewItem { DataContext = item };
                        tvi.Tag = new TreeViewItemUpdater(tvi);
                        if (tvi != null) treeViewItems[item] = tvi;
                    }
                    if (tvi != null && !treeView.Items.Contains(tvi)) treeView.Items.Add(tvi);
                }
            }
        }

        
    }
}
