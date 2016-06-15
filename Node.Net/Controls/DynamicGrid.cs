using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class DynamicGrid : System.Windows.Controls.Grid
    {
        public DynamicGrid()
        {
            DataContextChanged += _DataContextChanged;
        }

        private List<string> columnNames = new List<string>();
        public string[] ColumnNames
        {
            get { return columnNames.ToArray(); }
            set
            {
                columnNames = new List<string>(value);
                Update();
            }
        }

        protected virtual void Update()
        {
            Children.Clear();
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            AddColumnHeaders();
            IDictionary items = Collections.KeyValuePair.GetValue(DataContext) as IDictionary;
            if(!object.ReferenceEquals(null, items))
            {
                int row = 1;
                foreach (string name in items.Keys)
                {
                    IDictionary item = items[name] as IDictionary;
                    if (!object.ReferenceEquals(null, item))
                    {
                        RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        for (int c = 0; c < columnNames.Count; ++c)
                        {
                            UIElement element = GetUIElement(name, item, ColumnNames[c]);
                            Children.Add(element);
                            Grid.SetRow(element, row);
                            Grid.SetColumn(element, c);
                        }
                        ++row;
                    }
                }
            }
        }

        protected UIElement GetUIElement(string name, IDictionary value, string key)
        {
            string svalue = "";
            if (value.Contains(key))
            {
                svalue = value[key].ToString();
            }
            else
            {
                if (key == "Name")
                {
                    svalue = name;
                }
                else
                {
                    PropertyInfo propertyInfo = GetType().GetProperty(key);
                    if(!object.ReferenceEquals(null,propertyInfo))
                    {
                        svalue = propertyInfo.GetValue(this).ToString();
                    }
                }
            }
            return new Label()
            {
                Content = svalue,
                HorizontalAlignment = HorizontalAlignment.Right
            };
        }

        private void AddColumnHeaders()
        {
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            for (int i = 0; i < columnNames.Count; ++i)
            {
                Label label = new Label()
                {
                    Content = columnNames[i],
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                Children.Add(label);
                Grid.SetColumn(label, i);
            }
        }
        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
    }
}
