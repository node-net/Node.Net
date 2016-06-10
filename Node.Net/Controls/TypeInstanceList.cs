using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls
{
    public class TypeInstanceList : System.Windows.Controls.Grid
    {
        public TypeInstanceList()
        {
            DataContextChanged += TypeInstanceList_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            IgnoreKeys.Add("Type");
        }
        public TypeInstanceList(string type)
        {
            _type = type;
            DataContextChanged += TypeInstanceList_DataContextChanged;
        }

        public List<string> IgnoreKeys = new List<string>();

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; Update(); }
        }

        private void TypeInstanceList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private IDictionary GetColumnNamesAndKeys()
        {
            var results = new Dictionary<string, string>();
            var root = DataContext as IDictionary;
            if (!object.ReferenceEquals(null, root))
            {
                var model = root.Collect(new Node.Net.Filters.TypeFilter(Type));
                if (model.Count > 0)
                {
                    foreach (string key in model.Keys)
                    {
                        var instance = model[key] as IDictionary;
                        if (!object.ReferenceEquals(null, instance))
                        {
                            foreach (string ikey in instance.Keys)
                            {
                                if (!IgnoreKeys.Contains(ikey))
                                {
                                    results.Add(ikey, ikey);
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return results;
        }
        private void Update()
        {
            Children.Clear();
            RowDefinitions.Clear();
            ColumnDefinitions.Clear();
            var root = DataContext as IDictionary;
            if (!object.ReferenceEquals(null, root))
            {
                RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                // Setup Column Headers
                ColumnDefinitions.Add(new ColumnDefinition());  // Name
                Label label = new Label() { Content = "Name", Background = Brushes.Gray };
                Children.Add(label);
                IDictionary columnNames = GetColumnNamesAndKeys();
                foreach (string name in columnNames.Keys)
                {
                    ColumnDefinitions.Add(new ColumnDefinition());
                    label = new Label() { Content = name, Background = Brushes.Gray };
                    Children.Add(label);
                    Grid.SetColumn(label, ColumnDefinitions.Count - 1);
                }
                IDictionary model = root.Collect(new Node.Net.Filters.TypeFilter(Type));
                foreach (string key in model.Keys)
                {
                    Label keyLabel = new Label() { Content = key };
                    RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    Children.Add(keyLabel);
                    Grid.SetRow(keyLabel, RowDefinitions.Count - 1);

                    IDictionary instance = model[key] as IDictionary;
                    if (!object.ReferenceEquals(null, instance))
                    {
                        int column = 1;
                        foreach (string name in columnNames.Keys)
                        {
                            string ikey = columnNames[name].ToString();
                            if (!IgnoreKeys.Contains(ikey))
                            {
                                Label valueLabel = new Label() { Content = instance[ikey].ToString() };
                                Children.Add(valueLabel);
                                Grid.SetRow(valueLabel, RowDefinitions.Count - 1);
                                Grid.SetColumn(valueLabel, column);
                                ++column;
                            }
                        }
                    }

                }
            }
        }
    }
}
