using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class Selector : Grid
    {
        private string _title = "Selector";
        private object _items = null;
        public Selector(string title, object items)
        {
            _title = title;
            _items = items;
            DataContextChanged += _DataContextChanged;
        }

        public event SelectionChangedHandler SelectionChanged;
        public delegate void SelectionChangedHandler(object selected_item);
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private ComboBox _comboBox = null;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition() { Width=GridLength.Auto});
            ColumnDefinitions.Add(new ColumnDefinition());

            Children.Add(new Label() { Content = _title });
            _comboBox = new ComboBox();
            _comboBox.SelectionChanged += _comboBox_SelectionChanged;
            Children.Add(_comboBox);
            Grid.SetColumn(_comboBox, 1);
            Update();
        }

        private void _comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!object.ReferenceEquals(null, SelectionChanged))
            {
                ComboBoxItem cbi = _comboBox.SelectedItem as ComboBoxItem;
                SelectionChanged(cbi.DataContext);
            }
        }

        private void Update()
        {
            if (!object.ReferenceEquals(null, _comboBox))
            {
                _comboBox.Items.Clear();
                IDictionary dictionary = _items as IDictionary;
                if (!object.ReferenceEquals(null, dictionary))
                {
                    foreach (string key in dictionary.Keys)
                    {
                        _comboBox.Items.Add(new ComboBoxItem() { Content = key, DataContext = dictionary[key] });
                    }
                    if (object.ReferenceEquals(null, _comboBox.SelectedItem) && _comboBox.Items.Count > 0)
                    {
                        _comboBox.SelectedIndex = 0;
                    }
                }
            }
        }
    }
}
