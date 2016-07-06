using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls.TreeViewItems
{
    public class IDictionaryTreeViewItem : System.Windows.Controls.TreeViewItem
    {
        public IDictionaryTreeViewItem()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            Items.Clear();
            var dictionary = Node.Net.Collections.KeyValuePair.GetValue(DataContext) as IDictionary;
            if(dictionary == null)
            {
                Header = null;
            }
            else
            {
                var stackPanel = new System.Windows.Controls.StackPanel { Orientation = System.Windows.Controls.Orientation.Horizontal };
                stackPanel.Children.Add(new System.Windows.Controls.Label { Content = Node.Net.Collections.KeyValuePair.GetKey(DataContext).ToString() });
                stackPanel.Children.Add(new StackPanels.IDictionaryValuesStackPanel { DataContext = DataContext });
                Header = stackPanel;// Node.Net.Collections.KeyValuePair.GetKey(DataContext).ToString();
                foreach(object item in dictionary)
                {
                    //Items.Add(new )
                }
            }
        }
    }
}
