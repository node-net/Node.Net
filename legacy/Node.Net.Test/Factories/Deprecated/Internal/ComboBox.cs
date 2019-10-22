using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Node.Net.Controls.Forms
{
    class ComboBox : System.Windows.Controls.ComboBox
    {
        public ComboBox()
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
            var dictionary = DataContext as IDictionary;
            if(dictionary != null)
            {
                foreach(string key in dictionary.Keys)
                {
                    Items.Add(new ComboBoxItem { Content = key, DataContext = dictionary[key] });
                }
                if (Items.Count > 0) SelectedIndex = 0;
            }
        }
    }
}
