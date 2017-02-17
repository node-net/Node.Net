using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Controls
{
    public class ComboBox : System.Windows.Controls.ComboBox
    {
        public ComboBox()
        {
            DataContextChanged += ComboBox_DataContextChanged;
        }

        private void ComboBox_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private bool initialized = false;
        protected override void OnInitialized(EventArgs e)
        {
            initialized = true;
            base.OnInitialized(e);
            Update();
        }
        private void Update()
        {
            if (!initialized) return;
            Items.Clear();
            var dictionary = DataContext as IDictionary;
            if(dictionary != null)
            {
                foreach(string key in dictionary.Keys)
                {
                    Items.Add(new System.Windows.Controls.ComboBoxItem
                    {
                        Content = key,
                        DataContext = dictionary[key]
                    });
                }
                if (Items.Count > 0 && SelectedItem == null) SelectedItem = Items[0];
            }
        }
    }
}
