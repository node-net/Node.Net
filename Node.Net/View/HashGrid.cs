using System;
using System.Windows.Controls;

namespace Node.Net.View
{
    public class HashGrid : System.Windows.Controls.Grid
    {
        private System.Collections.Generic.List<string> orderedKeys = null;
        public HashGrid() { }
        public HashGrid(Node.Net.Json.Hash hash,string[] ordered_keys= null)
        {
            DataContext = hash;
            if (!ReferenceEquals(null, ordered_keys))
            {
                orderedKeys = new System.Collections.Generic.List<string>(ordered_keys);
            }
            this.DataContextChanged += HashGrid_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            ColumnDefinitions.Add(new System.Windows.Controls.ColumnDefinition());
            Update();
        }

        private void HashGrid_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        { 
            Node.Net.Json.Hash hash = DataContext as Node.Net.Json.Hash;
            if(ReferenceEquals(null, hash))
            {
                RowDefinitions.Clear();
                Children.Clear();
            }
            else
            {
                int row_index = 0;
                foreach(string key in GetKeys())
                {
                    RowDefinitions.Add(new System.Windows.Controls.RowDefinition());
                    Label nameLabel = new Label() { Content = key};
                    Children.Add(nameLabel);
                    Grid.SetColumn(nameLabel, 0);
                    Grid.SetRow(nameLabel, row_index);
                    string svalue = "null";
                    if (!ReferenceEquals(null, hash[key])) { svalue = hash[key].ToString(); }
                    Label valueLabel = new Label() { Content = svalue };
                    Children.Add(valueLabel);
                    Grid.SetColumn(valueLabel, 1);
                    Grid.SetRow(valueLabel, row_index);
                    ++row_index;
                }
            }
        }

        private string[] GetKeys()
        {
            if (!ReferenceEquals(null, orderedKeys)) return orderedKeys.ToArray();
            Node.Net.Json.Hash hash = DataContext as Node.Net.Json.Hash;
            if (!ReferenceEquals(null, hash)) return new System.Collections.Generic.List<string>(hash.Keys).ToArray();
            return null;
        }
    }
}