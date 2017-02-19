using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace Node.Net.Deprecated.Controls.Internal
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

        private object treeViewDataContext = null;
        private Dictionary<string, System.Windows.Controls.TreeViewItem> treeViewItems = new Dictionary<string, System.Windows.Controls.TreeViewItem>();
        public void UpdateTreeView(System.Windows.Controls.TreeView treeView)
        {
            treeView.Background = Brushes.Azure;
            treeView.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Stretch;
            treeView.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            treeView.Items.Clear();

            if(treeViewDataContext != treeView.DataContext)
            {
                treeViewDataContext = treeView.DataContext;
                treeViewItems.Clear();
            }
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
                if(!treeView.Items.Contains(tvi))
                {
                    treeView.Items.Add(tvi);
                }
            }  
        }
        
        private Dictionary<string,dynamic> GetItems(System.Windows.Controls.TreeView treeView)
        {
            var result = new Dictionary<string, dynamic>();
            if (treeView.DataContext != null)
            {
                if (Internal.KeyValuePair.IsKeyValuePair(treeView.DataContext))
                {
                    result.Add(Internal.KeyValuePair.GetKey(treeView.DataContext).ToString(), Internal.KeyValuePair.GetValue(treeView.DataContext));
                }
                else
                {
                    return GetChildren(treeView);
                }
            }
            return result;
        }
        private static Dictionary<string,dynamic> GetChildren(System.Windows.Controls.TreeView treeView)
        {
            var children = new Dictionary<string, dynamic>();
            if(treeView.DataContext != null)
            {
                var dictionary = Internal.KeyValuePair.GetValue(treeView.DataContext) as IDictionary;
                if (dictionary != null)
                {

                    var keys = new List<string>(Internal.TreeViewItemUpdaters.TreeViewItemUpdater.GetChildKeys(dictionary));
                    foreach (var item in dictionary)
                    {
                        if (keys.Contains(Internal.KeyValuePair.GetKey(item).ToString()))
                        {
                            children.Add(Internal.KeyValuePair.GetKey(item).ToString(), Internal.KeyValuePair.GetValue(item));
                        }
                    }
                }
            }
            return children;
        }        
    }
}
