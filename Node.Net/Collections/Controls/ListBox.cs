using System.Collections;

namespace Node.Net.Collections.Controls
{
    public class ListBox : System.Windows.Controls.ListBox
    {
        public ListBox()
        {
            DataContextChanged += ListBox_DataContextChanged;
        }

        private void ListBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            Items.Clear();
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if (dictionary != null)
            {
                foreach (var key in dictionary.Keys)
                {
                    Items.Add(new System.Windows.Controls.ListBoxItem { Content = key.ToString(), DataContext = dictionary[key] });
                }
            }
        }
    }
}
