using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Internal.TreeViewItemUpdaters
{
    class TreeViewItemUpdater
    {
        public TreeViewItemUpdater(System.Windows.Controls.TreeViewItem treeViewItem)
        {
            treeViewItem.DataContextChanged += TreeViewItem_DataContextChanged;
            UpdateTreeViewItem(treeViewItem);
        }


        private void TreeViewItem_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            UpdateTreeViewItem(sender as System.Windows.Controls.TreeViewItem);
        }
        private void UpdateTreeViewItem(System.Windows.Controls.TreeViewItem treeViewItem)
        {
            
            treeViewItem.HorizontalAlignment = HorizontalAlignment.Stretch;
            UpdateHeader(treeViewItem);
            UpdateChildren(treeViewItem);
        }

        private void UpdateHeader(System.Windows.Controls.TreeViewItem treeViewItem)
        {
            if (treeViewItem.DataContext != null)
            {
                var key = treeViewItem.DataContext.GetKey();
                var value = treeViewItem.DataContext.GetValue();
                if (value == null) treeViewItem.Header = $"{treeViewItem.DataContext.GetKey()} null";
                else if (value.GetType().IsValueType) treeViewItem.Header = $"{treeViewItem.DataContext.GetKey()} {value.ToString()}";
                else if (value.GetType() == typeof(string)) treeViewItem.Header = $"{treeViewItem.DataContext.GetKey()} {value.ToString()}";
                else
                {
                    treeViewItem.Header = new Header { DataContext = treeViewItem.DataContext };
                }
            }
        }
        private Dictionary<object, System.Windows.Controls.TreeViewItem> treeViewItems = new Dictionary<object, System.Windows.Controls.TreeViewItem>();

        private void UpdateChildren(System.Windows.Controls.TreeViewItem treeViewItem)
        {
            treeViewItem.Items.Clear();
            var dictionary = treeViewItem.DataContext.GetValue() as IDictionary;
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
                            tvi = new System.Windows.Controls.TreeViewItem { DataContext = item, HorizontalAlignment = HorizontalAlignment.Stretch,HorizontalContentAlignment = HorizontalAlignment.Stretch };
                            tvi.Tag = new Internal.TreeViewItemUpdaters.TreeViewItemUpdater(tvi);
                            if (tvi != null) treeViewItems[item] = tvi;
                        }
                        if (tvi != null && !treeViewItem.Items.Contains(tvi)) treeViewItem.Items.Add(tvi);
                    }
                }
            }
        }

        public static string[] GetChildKeys(IDictionary dictionary)
        {
            var keys = new List<string>();
            foreach(string key in dictionary.Keys)
            {
                var value = dictionary[key];
                if(value != null && value.GetType() != typeof(string) && typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    keys.Add(key);
                }
            }
            return keys.ToArray();
        }
    }
}
