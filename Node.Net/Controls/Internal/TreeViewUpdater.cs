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

        private Dictionary<string, System.Windows.Controls.TreeViewItem> treeViewItems = new Dictionary<string, System.Windows.Controls.TreeViewItem>();
        public void UpdateTreeView(System.Windows.Controls.TreeView treeView)
        {
            treeView.Background = Brushes.Azure;
            treeView.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            treeView.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            treeView.Items.Clear();

            var dictionary = GetItems(treeView);
            foreach(string key in dictionary.Keys)
            {
                System.Windows.Controls.TreeViewItem tvi = null;
                if (treeViewItems.ContainsKey(key)) tvi = treeViewItems[key];
                else
                {
                    tvi = new System.Windows.Controls.TreeViewItem { DataContext = new KeyValuePair<string, dynamic>(key, dictionary[key]) };
                    tvi.Tag = new Internal.TreeViewItemUpdaters.TreeViewItemUpdater(tvi);
                    treeViewItems[key] = tvi;
                }
                treeView.Items.Add(tvi);
            }  
        }
        
        private Dictionary<string,dynamic> GetItems(System.Windows.Controls.TreeView treeView)
        {
            if (treeView.DataContext != null)
            {
                var result = new Dictionary<string, dynamic>();
                result.Add(treeView.DataContext.GetKey(), treeView.DataContext.GetValue());
                return result;
            }
            return GetChildren(treeView);
        }
        private Dictionary<string,dynamic> GetChildren(System.Windows.Controls.TreeView treeView)
        {
            var children = new Dictionary<string, dynamic>();
            if(treeView.DataContext != null)
            {
                var dictionary = treeView.DataContext.GetValue() as IDictionary;
                if (dictionary != null)
                {

                    var keys = new List<string>(Internal.TreeViewItemUpdaters.TreeViewItemUpdater.GetChildKeys(dictionary));
                    foreach (var item in dictionary)
                    {
                        if (keys.Contains(item.GetKey()))
                        {
                            children.Add(item.GetKey(), item.GetValue());
                        }
                    }
                }
            }
            return children;
        }        
    }
}
