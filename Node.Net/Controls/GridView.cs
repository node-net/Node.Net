using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class GridView : Grid
    {
        public GridView()
        {
            this.DataContextChanged += GridView_DataContextChanged;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Update();
        }
        private void GridView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            var data = DataContext as IEnumerable;
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            Children.Clear();
            if(data != null)
            {
                RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                int column = 0;
                foreach(string name in ColumnNames)
                {
                    ColumnDefinitions.Add(new ColumnDefinition());
                    var label = new Label { Content = name };
                    Children.Add(label);
                    Grid.SetColumn(label, column);
                    ++column;
                }
                var enumerator = data.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    var item = enumerator.Current;
                    if (item != null)
                    {
                        column = 0;
                        foreach (var name in ColumnNames)
                        {
                            var child = _GetChild(item, name);
                            Children.Add(child);
                            SetColumn(child, column);
                            SetRow(child, RowDefinitions.Count);
                            ++column;
                        }
                    }
                }
            }
        }

        private UIElement _GetChild(object item,string name)
        {
            return new Label { Content = "?" };
        }
        public string[] ColumnNames
        {
            get
            {
                if(columnNames == null)
                {
                    var names = new List<string> { "Name" };
                    return names.ToArray();
                }
                return columnNames;
            }
            set
            {
                columnNames = value;
                Update();
            }
        }
        private string[] columnNames;
    }
}
