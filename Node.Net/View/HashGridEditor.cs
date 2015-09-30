using System;
using System.Windows.Controls;

namespace Node.Net.View
{
    public class HashGridEditor : HashGrid
    {
        public HashGridEditor() { }
        public HashGridEditor(Node.Net.Json.Hash hash, string[] ordered_keys = null)
            : base(hash, ordered_keys)
        {
        }

        protected override void Update()
        {
            RowDefinitions.Clear();
            Children.Clear();
            Node.Net.Json.Hash hash = DataContext as Node.Net.Json.Hash;
            if (ReferenceEquals(null, hash))
            {
                
                
            }
            else
            {
                int row_index = 0;
                foreach (string key in GetKeys())
                {
                    RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                    Label nameLabel = new Label() { Content = key };
                    Children.Add(nameLabel);
                    Grid.SetColumn(nameLabel, 0);
                    Grid.SetRow(nameLabel, row_index);
                    string svalue = "null";
                    if (!ReferenceEquals(null, hash[key])) { svalue = hash[key].ToString(); }
                    //Label valueLabel = new Label() { Content = svalue };
                    TextBox valueTextBox = new TextBox() { Text = svalue };
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
            Node.Net.Json.Hash hash = DataContext as Node.Net.Json.Hash;
            if (!ReferenceEquals(null, hash))
            {
                string key = ((TextBox)sender).Tag.ToString();
                if (hash.ContainsKey(key))
                {
                    hash[key] = ((TextBox)sender).Text;
                }
            }
        }
    }
}