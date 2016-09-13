using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Node.Net.Controls.Internal.GridUpdaters
{
    class VerticalValuesGridUpdater : ValuesGridUpdater, IGridUpdater
    {
        public VerticalValuesGridUpdater(System.Windows.Controls.Grid grid) : base(grid) { }

        public override void UpdateGrid(System.Windows.Controls.Grid grid)
        {
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            grid.Children.Clear();

            if (grid.DataContext == null) return;
            var dictionary = grid.DataContext.GetValue() as IDictionary;
            if (dictionary != null)
            {
                var keys = new List<string>(GetKeys(dictionary));
                if (keys.Count > 0)
                {
                    var rowIndex = 0;
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    if (ShowKeys) grid.ColumnDefinitions.Add(new ColumnDefinition());
                    foreach (string key in keys)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                        if (ShowKeys)
                        {
                            var keyLabel = new Label { Content = key, Background = Brushes.LightGray };
                            grid.Children.Add(keyLabel);
                            Grid.SetRow(keyLabel, rowIndex);
                        }
                        var value = dictionary[key];
                        if (value != null)
                        {
                            //var keyValue = new Label { Content = GetStringValue(value) };
                            var keyValue = new TextBox { Text = GetStringValue(value), TextWrapping = TextWrapping.Wrap, VerticalContentAlignment = VerticalAlignment.Center, BorderBrush=null  };
                            grid.Children.Add(keyValue);
                            Grid.SetColumn(keyValue, ShowKeys ? 1 : 0);
                            Grid.SetRow(keyValue, rowIndex);
                        }
                        ++rowIndex;
                    }
                }
            }
        }

        private string GetStringValue(object value)
        {
            if (value == null) return "";
            if(value.GetType() != typeof(string) && typeof(IEnumerable).IsAssignableFrom(value.GetType()))
            {
                var ienumerable = value as IEnumerable;
                var stringBuilder = new StringBuilder();
                foreach(var item in ienumerable)
                {
                    if (stringBuilder.Length > 0) stringBuilder.Append(",");
                    stringBuilder.Append(item.ToString());
                }
                return stringBuilder.ToString();
            }
            return value.ToString();
        }
    }
}
