using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Test.Prototype
{
    public class ChildrenGrid : System.Windows.Controls.Grid
    {
        public ChildrenGrid()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        private void Update()
        {
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();
            Children.Clear();

            if (DataContext == null) return;
            var dictionary = ObjectExtension.GetValue(DataContext) as IDictionary;

            var typeNames = GetTypeNames(dictionary);
            foreach(var typeName in typeNames)
            {
                RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                var border = new Border { BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(2) };
                var type_grid = new Grid { DataContext = GetInstances(dictionary,typeName) };
                border.Child = type_grid;
                Children.Add(border);
                Grid.SetRow(border, RowDefinitions.Count - 1);
            }
        }

        private string[] GetTypeNames(IDictionary dictionary)
        {
            var names = new List<string>();
            foreach(string key in dictionary.Keys)
            {
                var value = dictionary[key] as IDictionary;
                if(value != null && value.Contains("Type"))
                {
                    var typeName = value.ToString();
                    if(typeName.Length > 0 && !names.Contains(typeName))
                    {
                        names.Add(typeName);
                    }
                }
            }
            return names.ToArray();
        }

        private IDictionary GetInstances(IDictionary dictionary,string type)
        {
            var instances = new Dictionary<string, dynamic>();
            foreach (string key in dictionary.Keys)
            {
                var value = dictionary[key] as IDictionary;
                if (value != null && value.Contains("Type"))
                {
                    var typeName = value.ToString();
                    if (typeName == type)
                    {
                        instances.Add(key, value);
                    }
                }
            }
            return instances;
        }
    }
}
