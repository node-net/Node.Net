using System;
using System.Windows.Controls;

namespace Node.Net.View
{
    public class HashGridEditor : HashGrid
    {
        public HashGridEditor() { }
        public HashGridEditor(Node.Net.Collections.Hash hash, string[] ordered_keys = null)
            : base(hash, ordered_keys)
        {
        }

        protected override void Update()
        {
            RowDefinitions.Clear();
            Children.Clear();
            var hash = DataContext as Node.Net.Collections.Hash;
            if (ReferenceEquals(null, hash))
            {


            }
            else
            {
                var row_index = 0;
                foreach (string key in GetKeys())
                {
                    RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                    var nameLabel = new Label { Content = key };
                    Children.Add(nameLabel);
                    Grid.SetColumn(nameLabel, 0);
                    Grid.SetRow(nameLabel, row_index);
                    var svalue = "null";
                    if (!ReferenceEquals(null, hash[key])) { svalue = hash[key].ToString(); }
                    //Label valueLabel = new Label() { Content = svalue };
                    var valueTextBox = new TextBox { Text = svalue };
                    valueTextBox.TextChanged += ValueTextBox_TextChanged;
                    valueTextBox.Tag = key;
                    Children.Add(valueTextBox);
                    Grid.SetColumn(valueTextBox, 1);
                    Grid.SetRow(valueTextBox, row_index);
                    ++row_index;
                }
            }
        }

        private void ValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var hash = DataContext as Node.Net.Collections.Hash;
            if (!ReferenceEquals(null, hash))
            {
                var key = ((TextBox)sender).Tag.ToString();
                if (hash.ContainsKey(key))
                {
                    hash[key] = ((TextBox)sender).Text;
                }
            }
        }
    }
}