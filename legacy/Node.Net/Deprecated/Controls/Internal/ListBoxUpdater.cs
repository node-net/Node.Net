using System.Collections;
using System.Collections.Generic;

namespace Node.Net.Controls.Internal
{
    class ListBoxUpdater
    {
        public ListBoxUpdater(System.Windows.Controls.ListBox listBox)
        {
            listBox.DataContextChanged += ListBox_DataContextChanged;
        }

        private void ListBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            UpdateListBox(sender as System.Windows.Controls.ListBox);
        }

        private System.Windows.Controls.ListBoxItem GetListBoxItem(string key, object value)
        {
            System.Windows.Controls.ListBoxItem lbi = null;
            if (listBoxItems.ContainsKey(key))
            {
                lbi = listBoxItems[key];
            }
            else
            {
                lbi = new System.Windows.Controls.ListBoxItem { Content = key };
                listBoxItems.Add(key, lbi);
            }
            lbi.DataContext = new KeyValuePair<string, dynamic>(key, value);
            return lbi;
        }

        private Dictionary<string, System.Windows.Controls.ListBoxItem> listBoxItems = new Dictionary<string, System.Windows.Controls.ListBoxItem>();
        public void UpdateListBox(System.Windows.Controls.ListBox listBox)
        {
            listBox.Items.Clear();
            int clear_count = listBox.Items.Count;
            var dictionary = KeyValuePair.GetValue(listBox.DataContext) as IDictionary;
            if (dictionary != null)
            {
                foreach (string key in dictionary.Keys)
                {
                    var lbi = GetListBoxItem(key, dictionary[key]);
                    if (!listBox.Items.Contains(lbi)) listBox.Items.Add(lbi);
                }
            }
        }
    }
}
