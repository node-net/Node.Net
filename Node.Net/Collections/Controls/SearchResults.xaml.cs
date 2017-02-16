using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Node.Net.Collections.Controls
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : UserControl
    {
        public SearchResults()
        {
            DataContextChanged += _DataContextChanged;
            InitializeComponent();
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
           Update();
        }

        private void Update()
        {
            listBox.Items.Clear();
            var dictionary = KeyValuePair.GetValue(DataContext) as IDictionary;
            if(dictionary != null)
            {
                foreach(var key in dictionary.Keys)
                {
                    listBox.Items.Add(new ListBoxItem { Content = key.ToString(), DataContext = dictionary[key] });
                }
            }
        }
    }
}
